using BotwRandoLib;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using System.Linq;

namespace BotwRandoCli
{

    public class CMDOptions
    {
        public string BasePath { get; set; }
        public string UpdatePath { get; set; }
        public string DlcPath { get; set; }
        public string GfxPath { get; set; }
        public string Seed { get; set; }


        public bool Armors { get; set; } = true;
        public bool Swords { get; set; } = true;
        public bool Longswords { get; set; } = true;
        public bool Shields { get; set; } = true;
        public bool Spears { get; set; } = true;
        public bool Bows { get; set; } = true;
        public bool Enemies { get; set; } = true;
        public bool Insects { get; set; } = true;
        public bool Plants { get; set; } = true;
        public bool Mushrooms { get; set; } = true;
        public bool Fruits { get; set; } = true;
        public bool Animals { get; set; } = true;
        public bool Fishes { get; set; } = true;
        public bool Ores { get; set; } = true;
        public bool Subbosses { get; set; } = true;
        public bool Rupees { get; set; } = true;
        public bool Arrows { get; set; } = true;
        public bool Shops { get; set; } = true;
    }

    public class Program
    {
        public static List<KeyValuePair<string, bool>> randoOptions = GenerateRandoOptions();

        public static int Main(string[] args)
        {
            // If no args are given, use interactive mode
            if ( args.Length == 0)
            {
                InteractiveMode();
                return 0;
            }

            var cmd = new RootCommand
            {
                new Option<string>(new[] {"--basePath", "-b" }, "Required. Path to your BotW Base Content Folder."),
                new Option<string>(new[] {"--updatePath", "-u" }, "Required. Path to your BotW Update Content Folder."),
                new Option<string>(new[] {"--dlcPath", "-d" }, "Required. Path to your BotW DLC Content Folder."),
                new Option<string>(new[] {"--gfxPath", "-g" }, "Required. Path to your Cemu graphic pack Folder."),
                new Option<string>(new [] {"--seed", "-s" }, "The seed to use. Default is a random one."),

                new Option<bool>("--armors", getDefaultValue: () => true, "Randomize Armors."),
                new Option<bool>("--swords", getDefaultValue: () => true, "Randomize Swords."),
                new Option<bool>("--longswords", getDefaultValue: () => true, "Randomize Long Swords."),
                new Option<bool>("--shields", getDefaultValue: () => true, "Randomize Shields."),
                new Option<bool>("--spears", getDefaultValue: () => true, "Randomize Spears."),
                new Option<bool>("--bows", getDefaultValue: () => true, "Randomize Bows."),
                new Option<bool>("--enemies", getDefaultValue: () => true, "Randomize Enemies."),
                new Option<bool>("--insects", getDefaultValue: () => true, "Randomize Insects."),
                new Option<bool>("--plants", getDefaultValue: () => true, "Randomize Plants."),
                new Option<bool>("--mushrooms", getDefaultValue: () => true, "Randomize Mushrooms."),
                new Option<bool>("--fruits", getDefaultValue: () => true, "Randomize Fruits."),
                new Option<bool>("--animals", getDefaultValue: () => true, "Randomize Animals."),
                new Option<bool>("--fishes", getDefaultValue: () => true, "Randomize Fishes."),
                new Option<bool>("--ores", getDefaultValue: () => true, "Randomize Ores."),
                new Option<bool>("--subbosses", getDefaultValue: () => true, "Randomize Sub-Bosses."),
                new Option<bool>("--rupees", getDefaultValue: () => true, "Randomize Rupees."),
                new Option<bool>("--arrows", getDefaultValue: () => true, "Randomize Arrows."),
                new Option<bool>("--shops", getDefaultValue: () => true, "Randomize Armor Shops.")
            };

            cmd.Handler = CommandHandler.Create<CMDOptions>(Program.DealWithRandoParameters); 

            return cmd.Invoke(args);



        }

        public static void DealWithRandoParameters(CMDOptions options)
        {
           if (string.IsNullOrWhiteSpace(options.BasePath) || string.IsNullOrWhiteSpace(options.UpdatePath) ||
                string.IsNullOrWhiteSpace(options.DlcPath) || string.IsNullOrWhiteSpace(options.GfxPath))
           {
               Console.WriteLine("One of the required paths was not supplied!");
               return;
           }

            if (string.IsNullOrWhiteSpace(options.Seed)) options.Seed = Randomizer.GenerateSeed();


            //create dictionary
            randoOptions = GenerateRandoOptions(options.Armors, options.Swords, options.Longswords, options.Shields,
                options.Spears, options.Bows, options.Enemies, options.Insects, options.Plants, options.Mushrooms, options.Fruits,
                options.Animals, options.Fishes, options.Ores, options.Subbosses, options.Rupees, options.Arrows, options.Shops);

            Randomizer.RandomizeGame(options.BasePath, options.UpdatePath, options.DlcPath, options.GfxPath, randoOptions.ToDictionary(x => x.Key, x => x.Value), out int progress, options.Seed);


        }

        static void InteractiveMode()
        {
            // get base paths
            Console.WriteLine("Please enter the Path to the Base BotW Content Folder:");
            string basePath = GetRidOfBeginAndEndQuotes(Console.ReadLine());
            Console.WriteLine("Please enter the Path to the Update BotW Content Folder:");
            string updatePath = GetRidOfBeginAndEndQuotes(Console.ReadLine());
            Console.WriteLine("Please enter the Path to the DLC BotW Content Folder:");
            string dlcPath = GetRidOfBeginAndEndQuotes(Console.ReadLine());
            Console.WriteLine("Please enter the Path to your Cemu Graphic Packs Folder");
            string gfxPath = GetRidOfBeginAndEndQuotes(Console.ReadLine());

            //rando options
            string input = string.Empty;
            do
            {
                Console.WriteLine("If you want to change any of these options, press its corresponding number. Otherwise just hit Enter.");
                for (int i = 0; i < randoOptions.Count; i++)
                {
                    KeyValuePair<string, bool> randoOption = randoOptions[i];
                    Console.WriteLine($"{i+1} - {randoOption.Key}: {randoOption.Value}");
                }
                Console.WriteLine();
                input = Console.ReadLine();

                // If user provided a number, and that number is valid, toggle the value at that position
                if (Int32.TryParse(input, out int value) == true && (value-1) < randoOptions.Count)
                {
                    KeyValuePair<string, bool> current = randoOptions[value - 1];
                    randoOptions[value - 1] = new KeyValuePair<string, bool>(current.Key, !current.Value);
                }
                else if (!string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid input!");
                }


            } while (!string.IsNullOrWhiteSpace(input));

            //custom seed
            Console.WriteLine("If you want a custom seed, enter it now. Press Enter for a random one.");
            string seed = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(seed))
            {
                seed = Randomizer.GenerateSeed();
                Console.WriteLine("Using seed " + seed);
            }

            int progress = 0;

            Randomizer.RandomizeGame(basePath, updatePath, dlcPath, gfxPath, randoOptions.ToDictionary(x => x.Key, x => x.Value), out progress, seed);
        }

        private static string GetRidOfBeginAndEndQuotes(string? input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            char beginChar = input[0];

            // If the first char is either " or ' and the last char is the same, get rid of them
            if ((beginChar == '"' || beginChar == '\'') && input[input.Length-1] == beginChar)
                input = input.Substring(1, input.Length - 2);
            
            return input;
        }

        static List<KeyValuePair<string, bool>> GenerateRandoOptions(bool armor = true, bool swords = true, bool longswords = true, bool shields = true,
                    bool spears = true, bool bows = true, bool enemies = true, bool insects = true, bool plants = true, bool mush = true, bool fruits = true,
                    bool animal = true, bool fish = true, bool ores = true, bool subboss = true, bool rupees = true, bool arrows = true, bool shops = true)
        {
            List<KeyValuePair<string, bool>> randoOptions = new List<KeyValuePair<string, bool>>();

            randoOptions.Add(new KeyValuePair<string, bool>("randomizeArmorCheckbox", armor));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeSwordsCheckbox", swords));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeLongSwordsCheckbox", longswords));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeShieldsCheckbox", shields));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeSpearsCheckbox", spears));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeBowsCheckbox", bows));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeEnemiesCheckbox", enemies));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeInsectsCheckbox", insects));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizePlantsCheckbox", plants));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeMushroomsCheckbox", mush));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeFruitsCheckbox", fruits));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeAnimalsCheckbox", animal));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeFishesCheckbox", fish));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeOresCheckbox", ores));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeSubBossesCheckbox", subboss));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeRupeesCheckbox", rupees));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeArrowsCheckbox", arrows));
            randoOptions.Add(new KeyValuePair<string, bool>("randomizeArmorShopsCheckbox", shops));

            return randoOptions;
        }
    }
}