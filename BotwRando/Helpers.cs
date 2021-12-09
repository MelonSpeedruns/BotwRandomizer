using Newtonsoft.Json;

namespace BotwRando
{
    /// <summary>
    /// Class having various helper functions.
    /// </summary>
    public class Helpers
    {
        /// <summary>
        /// Gets the Directory name of a given path.
        /// </summary>
        /// <param name="path">The path to get the Directory name from.</param>
        /// <returns>The Directory name as a <see cref="String"></see>.</returns>
        public static string GetDirName(string path)
        {
            var dirName = new DirectoryInfo(path).Name;
            return dirName;
        }

        /// <summary>
        /// Loads settings from the <paramref name="settingsFile"></paramref> into the <paramref name="settings"></paramref> object.
        /// </summary>
        /// <param name="settingsFile">The path where the settings file is located.</param>
        /// <param name="settings">The object to load in the settings to.</param>
        public static void LoadSettings(string settingsFile, ref SettingsFile settings)
        {
            if (!File.Exists(settingsFile))
                return;

            SettingsFile? json = JsonConvert.DeserializeObject<SettingsFile>(File.ReadAllText(settingsFile));

            if (json != null)
                settings = json;
        }

        /// <summary>
        /// Saves settings from the <paramref name="settings"></paramref> object into the <paramref name="settingsFile"></paramref>.
        /// </summary>
        /// <param name="settingsFile">The path where the settings file is located.</param>
        /// <param name="settings">The object to load in the settings to.</param>
        public static void SaveSettings(string settingsFile, SettingsFile settings)
        {
            string json = JsonConvert.SerializeObject(settings);
            // TODO: this can crash as well
            File.WriteAllText(settingsFile, json);
        }
    }
}