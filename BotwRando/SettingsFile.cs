using System.Reflection;

namespace BotwRando
{
    public class SettingsFile
    {
        public StringSettings StringSettings { get; set; } = new StringSettings();
        public CheckBoxSettings CheckBoxSettings { get; set; } = new CheckBoxSettings();
    }

    public class StringSettings
    {
        public StringOption BasePath = new StringOption("baseTextBox", "");
        public StringOption UpdatePath = new StringOption("updateTextBox", "");
        public StringOption DlcPath = new StringOption("dlcTextBox", "");
        public StringOption GfxPackPath = new StringOption("gfxPackTextBox", "");
    }

    public class CheckBoxSettings
    {
        public CheckBoxOption RandomizeArmorCheckbox = new CheckBoxOption("randomizeArmorCheckbox", true);
        public CheckBoxOption RandomizeSwordsCheckbox = new CheckBoxOption("randomizeSwordsCheckbox", true);
        public CheckBoxOption RandomizeLongSwordsCheckbox = new CheckBoxOption("randomizeLongSwordsCheckbox", true);
        public CheckBoxOption RandomizeSpearsCheckbox = new CheckBoxOption("randomizeSpearsCheckbox", true);
        public CheckBoxOption RandomizeBowsCheckbox = new CheckBoxOption("randomizeBowsCheckbox", true);
        public CheckBoxOption RandomizeShieldsCheckbox = new CheckBoxOption("randomizeShieldsCheckbox", true);
        public CheckBoxOption RandomizeEnemiesCheckbox = new CheckBoxOption("randomizeEnemiesCheckbox", true);
        public CheckBoxOption RandomizeSubBossesCheckbox = new CheckBoxOption("randomizeSubBossesCheckbox", true);
        public CheckBoxOption RandomizeInsectsCheckbox = new CheckBoxOption("randomizeInsectsCheckbox", true);
        public CheckBoxOption RandomizePlantsCheckbox = new CheckBoxOption("randomizePlantsCheckbox", true);
        public CheckBoxOption RandomizeMushroomsCheckbox = new CheckBoxOption("randomizeMushroomsCheckbox", true);
        public CheckBoxOption RandomizeFruitsCheckbox = new CheckBoxOption("randomizeFruitsCheckbox", true);
        public CheckBoxOption RandomizeAnimalsCheckbox = new CheckBoxOption("randomizeAnimalsCheckbox", true);
        public CheckBoxOption RandomizeFishesCheckbox = new CheckBoxOption("randomizeFishesCheckbox", true);
        public CheckBoxOption RandomizeOresCheckbox = new CheckBoxOption("randomizeOresCheckbox", true);
        public CheckBoxOption RandomizeRupeesCheckbox = new CheckBoxOption("randomizeRupeesCheckbox", true);
        public CheckBoxOption RandomizeArrowsCheckbox = new CheckBoxOption("randomizeArrowsCheckbox", true);
        public CheckBoxOption RandomizeArmorShops = new CheckBoxOption("randomizeArmorShopsCheckbox", true);

        public Dictionary<string, bool> ToDictionary()
        {
            Dictionary<string, bool> dict = new Dictionary<string, bool>();

            foreach (FieldInfo opt in GetType().GetFields())
            {
                CheckBoxOption? cbo = (CheckBoxOption?)opt.GetValue(this);

                if (cbo != null)
                    dict.Add(cbo.ComponentName, cbo.Value);
            }

            return dict;
        }
    }

    public class StringOption
    {
        public string ComponentName { get; set; }
        public string Value { get; set; }

        public StringOption(string compname, string val)
        {
            this.ComponentName = compname;
            this.Value = val;
        }
    }

    public class CheckBoxOption
    {
        public string ComponentName { get; set; }
        public bool Value { get; set; }

        public CheckBoxOption(string compname, bool val)
        {
            this.ComponentName = compname;
            this.Value = val;
        }
    }
}