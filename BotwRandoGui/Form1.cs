using BotwRandoLib;
using Ookii.Dialogs.WinForms;
using System.ComponentModel;
using System.Reflection;

namespace BotwRandoGui
{
    public partial class Form1 : Form
    {
        public SettingsFile settingsFile = new SettingsFile();
        public const string SETTINGS_PATH = "settings.json";
        internal const string VERSION = Randomizer.VERSION;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The function that gets executed when the user clicks on <see cref="randomizeButton"></see>.
        /// </summary>
        private void RandomizeButtonClick(object sender, EventArgs e)
        {
            // Variable shortness
            string basePath = settingsFile.StringSettings.BasePath.Value;
            string updatePath = settingsFile.StringSettings.UpdatePath.Value;
            string dlcPath = settingsFile.StringSettings.DlcPath.Value;
            string gfxPackPath = settingsFile.StringSettings.GfxPackPath.Value;
            Dictionary<string, bool> randoOptions = settingsFile.CheckBoxSettings.ToDictionary();

            // Toggle everything off, randomize, toggle everything on again.
            ControlsToggle(false);

            // Run Method on different thread to not block UI thread
            int progress = 0;
            int maxProgress = 8;

            Task.Run(() => Randomizer.RandomizeGame(basePath, updatePath, dlcPath, gfxPackPath, randoOptions, out progress, seedTextBox.Text));

            progressBar1.Maximum = maxProgress;

            while (progress < maxProgress)
            {
                progressBar1.Value = progress;
                Application.DoEvents();
            }

            if (progress == maxProgress)
            {
                progressBar1.Value = progress;
                MessageBox.Show("Don't forget to restart Cemu and enable the graphic pack before playing!", "Randomization process successful!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("An error has occured while randomizing the game!");
            }

            // Toggle everything back on again
            ControlsToggle(true);

            progressBar1.Value = 0;
        }

        /// <summary>
        /// Toggles all Controls in <see cref="Form1"></see> to <paramref name="enabled"></paramref>.
        /// </summary>
        /// <param name="enabled">Boolean to set Control state to.</param>
        private void ControlsToggle(bool enabled)
        {
            // Toggle individual controls as disabled. If we disable the whole form, it'll get unresponsive
            foreach (Control c in Controls)
            {
                c.Enabled = enabled;
            }
        }

        /// <summary>
        /// Checks if all browse fields are filled in. 
        /// </summary>
        /// <returns>True if all browse fields are filled in, otherwise false.</returns>
        private bool AreAllBrowseFieldsFilledIn()
        {
            // Variable shortness
            string basePath = settingsFile.StringSettings.BasePath.Value;
            string updatePath = settingsFile.StringSettings.UpdatePath.Value;
            string dlcPath = settingsFile.StringSettings.DlcPath.Value;
            string gfxPackPath = settingsFile.StringSettings.GfxPackPath.Value;

            return !String.IsNullOrWhiteSpace(basePath) && !String.IsNullOrWhiteSpace(updatePath) && !String.IsNullOrWhiteSpace(dlcPath) && !String.IsNullOrWhiteSpace(gfxPackPath);
        }

        /// <summary>
        /// Recursive Function to get all Controls and Control-children of <paramref name="control"></paramref> of type <paramref name="type"></paramref>.
        /// </summary>
        /// <param name="control">The <see cref="Control"></see> to get all (Sub-) Children from.</param>
        /// <param name="type">The <see cref="Type"> to filter for.</see></param>
        /// <returns></returns>
        public IEnumerable<Control> GetAllFormControls(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetAllFormControls(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private void BrowseButtonClick(TextBox textBox, string dialogDescription, ref StringOption setting)
        {
            // Setup Folder dialog box
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = dialogDescription;
            dialog.UseDescriptionForTitle = true;

            // Show Folder dialog, get a path from it
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            string dirName = Helpers.GetDirName(dialog.SelectedPath);

            // If we're on a botw Path, it needs to be named "content", if we're on a cemu path, it needs be named graphicPacks
            string folderNameToCheck = textBox.Name != "gfxPackTextBox" ? "content" : "graphicPacks";

            if (dirName == folderNameToCheck)
            {
                // Naming scheme is correct, adjust text box and save path to Settings
                textBox.Text = dialog.SelectedPath;
                setting.Value = dialog.SelectedPath;
                Helpers.SaveSettings(SETTINGS_PATH, settingsFile);
            }
            else
            {
                // Naming scheme is incorrect, show error
                MessageBox.Show("The folder you have selected wasn't named \"" + folderNameToCheck + "\".", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Check if user did input all paths and if yes enable button
            randomizeButton.Enabled = AreAllBrowseFieldsFilledIn();
        }

        private void CheckedBox(CheckBox checkBox, ref CheckBoxOption setting)
        {
            // Second, save the setting
            setting.ComponentName = checkBox.Name;
            setting.Value = checkBox.Checked;
            Helpers.SaveSettings(SETTINGS_PATH, settingsFile);
        }

        /// <summary>
        /// Function that loads in all the settings from <see cref="settingsFile"></see> and refreshes the UI accordingly.
        /// </summary>
        private void ReplaceFormControls()
        {
            //thanks SO: https://stackoverflow.com/a/3426721
            //First set all the StringOptions to UI
            foreach (TextBox b in GetAllFormControls(this, typeof(TextBox)))
            {
                foreach (FieldInfo fieldInfo in settingsFile.StringSettings.GetType().GetFields())
                {
                    StringOption? option = fieldInfo.GetValue(settingsFile.StringSettings) as StringOption;

                    if (option == null)
                        break;

                    if (b.Name == option.ComponentName)
                    {
                        b.Text = option.Value;
                        break;
                    }
                }
            }

            //After that all CheckBoxOptions
            foreach (CheckBox c in GetAllFormControls(this, typeof(CheckBox)))
            {
                foreach (FieldInfo fieldInfo in settingsFile.CheckBoxSettings.GetType().GetFields())
                {
                    CheckBoxOption? option = fieldInfo.GetValue(settingsFile.CheckBoxSettings) as CheckBoxOption;

                    if (option == null)
                        break;

                    if (c.Name == option.ComponentName)
                    {
                        c.Checked = option.Value;
                        break;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Helpers.LoadSettings(SETTINGS_PATH, ref settingsFile);

            ReplaceFormControls();

            randomizeButton.Enabled = AreAllBrowseFieldsFilledIn();

            seedTextBox.Text = Randomizer.GenerateSeed();

            // String Options
            baseButton.Click += (sender, e) => BrowseButtonClick(baseTextBox, "Open the \"content\" folder that contains the base BotW game", ref settingsFile.StringSettings.BasePath);
            updateButton.Click += (sender, e) => BrowseButtonClick(updateTextBox, "Open the \"content\" folder that contains the Update of BotW", ref settingsFile.StringSettings.UpdatePath);
            dlcButton.Click += (sender, e) => BrowseButtonClick(dlcTextBox, "Open the \"content\" folder that contains the DLC of BotW", ref settingsFile.StringSettings.DlcPath);
            gfxPackButton.Click += (sender, e) => BrowseButtonClick(gfxPackTextBox, "Open the \"graphicPacks\" folder in your Cemu folder", ref settingsFile.StringSettings.GfxPackPath);

            // CheckBox Options
            randomizeArmorCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeArmorCheckbox, ref settingsFile.CheckBoxSettings.RandomizeArmorCheckbox);
            randomizeSwordsCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeSwordsCheckbox, ref settingsFile.CheckBoxSettings.RandomizeSwordsCheckbox);
            randomizeLongSwordsCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeLongSwordsCheckbox, ref settingsFile.CheckBoxSettings.RandomizeLongSwordsCheckbox);
            randomizeSpearsCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeSpearsCheckbox, ref settingsFile.CheckBoxSettings.RandomizeSpearsCheckbox);
            randomizeBowsCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeBowsCheckbox, ref settingsFile.CheckBoxSettings.RandomizeBowsCheckbox);
            randomizeShieldsCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeShieldsCheckbox, ref settingsFile.CheckBoxSettings.RandomizeShieldsCheckbox);
            randomizeEnemiesCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeEnemiesCheckbox, ref settingsFile.CheckBoxSettings.RandomizeEnemiesCheckbox);
            randomizeSubBossesCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeSubBossesCheckbox, ref settingsFile.CheckBoxSettings.RandomizeSubBossesCheckbox);
            randomizeInsectsCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeInsectsCheckbox, ref settingsFile.CheckBoxSettings.RandomizeInsectsCheckbox);
            randomizePlantsCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizePlantsCheckbox, ref settingsFile.CheckBoxSettings.RandomizePlantsCheckbox);
            randomizeMushroomsCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeMushroomsCheckbox, ref settingsFile.CheckBoxSettings.RandomizeMushroomsCheckbox);
            randomizeFruitsCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeFruitsCheckbox, ref settingsFile.CheckBoxSettings.RandomizeFruitsCheckbox);
            randomizeAnimalsCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeAnimalsCheckbox, ref settingsFile.CheckBoxSettings.RandomizeAnimalsCheckbox);
            randomizeFishesCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeFishesCheckbox, ref settingsFile.CheckBoxSettings.RandomizeFishesCheckbox);
            randomizeOresCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeOresCheckbox, ref settingsFile.CheckBoxSettings.RandomizeOresCheckbox);
            randomizeRupeesCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeOresCheckbox, ref settingsFile.CheckBoxSettings.RandomizeOresCheckbox);
            randomizeArrowsCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeArrowsCheckbox, ref settingsFile.CheckBoxSettings.RandomizeArrowsCheckbox);
            randomizeArmorShopsCheckbox.CheckedChanged += (sender, e) => CheckedBox(randomizeArmorShopsCheckbox, ref settingsFile.CheckBoxSettings.RandomizeArmorShops);
        }
    }
}