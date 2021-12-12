using ByamlExt.Byaml;
using SARCExt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Toolbox.Library;
using Toolbox.Library.Security.Cryptography;

namespace BotwRandoLib
{
    public class Randomizer
    {
        private static List<string> dungeonFiles = new List<string>();
        private static List<uint> eventsToDisable = new List<uint>();
        private static uint paragliderChest;
        private static Random random;

        private static BotwObjects overworldObjectsTable;
        private static BotwRandoTable chestObjectsTable;

        private static Dictionary<string, string> modifiedActors = new Dictionary<string, string>();

        private static string spoilerLogPath = "";

        private const int SPIRIT_ORB_COUNT = 240;
        private const int CHEST_COUNT = 1398;

        public const string VERSION = "2.1.0";

        /// <summary>
        /// Creates a Randomizer Graphic pack based on parameters. <paramref name="progress"></paramref> is used to keep track in which randomization phase its currently in.
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="updatePath"></param>
        /// <param name="dlcPath"></param>
        /// <param name="gfxPackPath"></param>
        /// <param name="randomizationSettings"></param>
        /// <param name="progress">Has values from 0-8, 100 being exit due to error.</param>
        /// <param name="seed"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void RandomizeGame(string basePath, string updatePath, string dlcPath, string gfxPackPath, Dictionary<string, bool> randomizationSettings, out int progress, string seed = null)
        {
            if (String.IsNullOrWhiteSpace(seed)) seed = GenerateSeed();

            random = new Random(unchecked((int)Crc32.Compute(seed)));

            progress = 0;

            List<uint> paragliderChests = LibHelpers.GetParagliderChests();
            paragliderChest = paragliderChests[random.Next(paragliderChests.Count)];

            // Add event IDs to disable in a list
            eventsToDisable = LibHelpers.GetEventsToDisable();

            overworldObjectsTable = new BotwObjects();
            chestObjectsTable = new BotwRandoTable(CHEST_COUNT);

            if (!String.IsNullOrWhiteSpace(basePath) && !String.IsNullOrWhiteSpace(updatePath) && !String.IsNullOrWhiteSpace(dlcPath) && !String.IsNullOrWhiteSpace(gfxPackPath))
            {
                // Check if directories are valid
                if (!(LibHelpers.IsDirectoryValid(basePath) || LibHelpers.IsDirectoryValid(updatePath) || LibHelpers.IsDirectoryValid(dlcPath) || LibHelpers.IsDirectoryValid(gfxPackPath)))
                    throw new ArgumentException("One of the supplied Paths was not valid or doesn't exist!");

                // Delete the graphic pack if one already exists, and re-create it
                string gfxPackNewPath = Path.Combine(gfxPackPath, "BotW Randomizer");

                try
                {
                    if (Directory.Exists(gfxPackNewPath))
                    {
                        Directory.Delete(gfxPackNewPath, true);
                    }

                    Directory.CreateDirectory(gfxPackNewPath);
                }
                // TODO: have randomize function return *something* so one can check if it executed successfully or not?
                catch
                {
                    progress = 100;
                    return;
                }

                spoilerLogPath = Path.Combine(gfxPackNewPath, "spoiler-log.txt");
                File.WriteAllText(spoilerLogPath, "Seed: " + seed + "\n");

                string gfxPackRulesFile = Path.Combine(gfxPackNewPath, "rules.txt");
                File.WriteAllLines(gfxPackRulesFile, RulesTextFile(VERSION));

                // Useful path variables
                string dlcMainFieldPath = Path.Combine(dlcPath, "0010", "Map", "MainField");
                string gfxPackMainFieldPath = Path.Combine(gfxPackNewPath, "aoc", "0010", "Map", "MainField");
                string updateRstbFile = Path.Combine(updatePath, "System", "Resource", "ResourceSizeTable.product.srsizetable");
                string gfxPackRstbFile = Path.Combine(gfxPackNewPath, "content", "System", "Resource", "ResourceSizeTable.product.srsizetable");

                // Create a corrupted "AocMainField.pack" file so the game uses the map files from the "MainField" folder instead
                string gfxPackCorruptedFile = Path.Combine(gfxPackNewPath, "aoc", "0010", "Pack", "AocMainField.pack");
                // TODO: this can crash
                Directory.CreateDirectory(Path.GetDirectoryName(gfxPackCorruptedFile));
                File.WriteAllText(gfxPackCorruptedFile, String.Empty);

                // Copy the necessary files over from the game to the graphic pack
                LibHelpers.CopyRstbFile(updateRstbFile, gfxPackRstbFile);

                string versionFile = Path.Combine(gfxPackNewPath, "content", "System", "Version.txt");
                File.WriteAllText(versionFile, seed);

                progress++;

                if (!LibHelpers.CopyMapFiles(dlcMainFieldPath, gfxPackMainFieldPath))
                {
                    progress = 100;
                    return;
                }

                progress++;

                // Copy all Base game and Dlc shrine pack files into the graphic pack

                string baseShrinesPath = Path.Combine(basePath, "Pack");
                string dlcShrinesPath = Path.Combine(dlcPath, "0010", "Pack");
                string gfxPackBaseShrinesPath = Path.Combine(gfxPackNewPath, "content", "Pack");
                string gfxPackDlcShrinesPath = Path.Combine(gfxPackNewPath, "aoc", "0010", "Pack");
                if (!LibHelpers.CopyShrineFiles(baseShrinesPath, dlcShrinesPath, gfxPackBaseShrinesPath, gfxPackDlcShrinesPath, ref dungeonFiles))
                {
                    progress = 100;
                    return;
                }

                progress++;

                // Reset the progress bar and set it's maximum size to the amount of map files
                string[] mapFiles = Directory.GetFiles(gfxPackMainFieldPath, "*.smubin", SearchOption.AllDirectories);
                // For every map file, open it, patch it's contents, add it to the RSTB list and close it
                File.WriteAllText(spoilerLogPath, File.ReadAllText(spoilerLogPath) + "\n" + "\n" + "=== Overworld ===" + "\n");
                foreach (string mapFile in mapFiles)
                {
                    OpenMainFieldMapFile(mapFile, "MainField", randomizationSettings);
                }

                progress++;

                // For every dungeon pack file, open it, patch it's contents, add it to the RSTB list and close it
                File.WriteAllText(spoilerLogPath, File.ReadAllText(spoilerLogPath) + "\n" + "\n" + "=== Shrines ===" + "\n");
                foreach (string dungeonFile in dungeonFiles)
                {
                    OpenDungeonPackFile(dungeonFile, "CDungeon", randomizationSettings);
                }

                progress++;

                string updateBootupPath = Path.Combine(updatePath, "Pack", "Bootup.pack");
                string gfxPackBootupPath = Path.Combine(gfxPackNewPath, "content", "Pack", "Bootup.pack");

                // Update GameData and SaveData with the modified actors
                File.Copy(updateBootupPath, gfxPackBootupPath, true);
                UpdateGameData(gfxPackBootupPath);
                UpdateSaveData(gfxPackBootupPath);

                progress++;

                //string titleBgUpdatePath = Path.Combine(updatePath, "Pack", "TitleBG.pack");
                //string titleBgGfxPackPath = Path.Combine(gfxPackNewPath, "content", "Pack", "TitleBG.pack");
                //File.Copy(titleBgUpdatePath, titleBgGfxPackPath, true);

                string updateEventsPath = Path.Combine(updatePath, "Event");
                string gfxPackEventsPath = Path.Combine(gfxPackNewPath, "content", "Event");
                // Copy and patch the custom event files into the originals, and place them in the graphic pack
                LibHelpers.CopyAndInjectEventFile(Properties.Resources.Demo003_0, "Demo003_0", updateEventsPath, gfxPackEventsPath);
                LibHelpers.CopyAndInjectEventFile(Properties.Resources.Demo033_0, "Demo033_0", updateEventsPath, gfxPackEventsPath);
                LibHelpers.CopyAndInjectEventFile(Properties.Resources.Demo700_0, "Demo700_0", updateEventsPath, gfxPackEventsPath);
                LibHelpers.CopyAndInjectEventFile(Properties.Resources.Demo701_0, "Demo701_0", updateEventsPath, gfxPackEventsPath);
                LibHelpers.CopyAndInjectEventFile(Properties.Resources.Demo333_0, "Demo333_0", updateEventsPath, gfxPackEventsPath);
                LibHelpers.CopyAndInjectEventFile(Properties.Resources.HyruleCastle, "HyruleCastle", updateEventsPath, gfxPackEventsPath);
                //LibHelpers.CopyAndInjectEventFile(Properties.Resources.Demo002_0, "Demo002_0", titleBgUpdatePath, titleBgGfxPackPath, true);

                progress++;

                Console.WriteLine(currentChestCount);

                // Change the size of all modified files in the RSTB of the graphic pack
                LibHelpers.RstbFiles(gfxPackRstbFile);

                progress++;
            }
            else if (String.IsNullOrWhiteSpace(basePath))
                throw new ArgumentException("basePath is null or empty!");

            else if (String.IsNullOrWhiteSpace(updatePath))
                throw new ArgumentException("updatePath is null or empty!");

            else if (String.IsNullOrWhiteSpace(dlcPath))
                throw new ArgumentException("dlcPath is null or empty!");

            else if (String.IsNullOrWhiteSpace(gfxPackPath))
                throw new ArgumentException("gfxPackPath is null or empty!");
        }

        private static void OpenDungeonPackFile(string dungeonFile, string mapType, Dictionary<string, bool> randomizationSettings)
        {
            SarcData dungeonSarcData;

            FileStream fs = File.OpenRead(dungeonFile);
            dungeonSarcData = SARC.UnpackRamN(fs);
            fs.Close();

            string dungeonName = Path.GetFileNameWithoutExtension(dungeonFile);
            string staticDungeonPath = $"Map/CDungeon/{dungeonName}/{dungeonName}_Static.smubin";
            string dynamicDungeonPath = $"Map/CDungeon/{dungeonName}/{dungeonName}_Dynamic.smubin";

            RandomizeDungeon(ref dungeonSarcData, staticDungeonPath, "Static", dungeonName, "CDungeon", randomizationSettings);
            RandomizeDungeon(ref dungeonSarcData, dynamicDungeonPath, "Dynamic", dungeonName, "CDungeon", randomizationSettings);

            // Save the modified dungeon file as a file
            byte[] decompressedData = SARC.PackN(dungeonSarcData).Item2;
            File.WriteAllBytes(dungeonFile, decompressedData);
        }

        private static bool IsYaz0(byte[] fileData)
        {
            return fileData[0] == 0x59 && fileData[1] == 0x61 && fileData[2] == 0x7A && fileData[3] == 0x30;
        }

        private static void RandomizeDungeon(ref SarcData dungeonSarcData, string dungeonPath, string staticDynamic, string dungeonName, string mapType, Dictionary<string, bool> randomizationSettings)
        {
            // Yaz0 decompress the .smubin file and open it as a Byaml
            byte[] dungeonStaticData = dungeonSarcData.Files[dungeonPath];
            MemoryStream ms = new MemoryStream(dungeonStaticData);
            Yaz0 yaz = new Yaz0();

            Stream s = yaz.Decompress(ms);
            BymlFileData byaml = ByamlFile.LoadN(s);
            s.Close();

            ms.Close();

            //Prepare a new dictionary instance for each obj to remove any references
            List<dynamic> objectList = (List<dynamic>)byaml.RootNode["Objs"];
            // For every object in the map, randomize it!
            for (int i = 0; i < objectList.Count; i++)
            {
                Dictionary<string, dynamic> actorObj = new Dictionary<string, dynamic>();
                foreach (var item in (Dictionary<string, dynamic>)objectList[i])
                {
                    actorObj.Add(item.Key, item.Value);
                }

                //Prepare a new dictionary instance for each obj to remove any references
                Dictionary<string, dynamic> paramDict = new Dictionary<string, dynamic>();
                if (objectList[i].ContainsKey("!Parameters"))
                {
                    foreach (var item in (Dictionary<string, dynamic>)objectList[i]["!Parameters"])
                    {
                        paramDict.Add(item.Key, item.Value);
                    }
                }

                RandomizeMapObject(ref paramDict, ref actorObj, mapType, randomizationSettings);

                actorObj["!Parameters"] = paramDict;
                objectList[i] = actorObj;
            }

            // Save the modified byaml data
            byte[] dungeonData = ByamlFile.SaveN(byaml);

            ms = new MemoryStream(dungeonData);
            byte[] dungeonByamlData = yaz.Compress(ms).ToArray();
            ms.Close();
            dungeonSarcData.Files[dungeonPath] = dungeonByamlData;

            // Add the new map file size in the RSTB list
            string rstbPath = $"Map/CDungeon/{dungeonName}/{dungeonName}_{staticDynamic}.mubin";

            ms = new MemoryStream(dungeonData);
            LibHelpers.RstbFile(rstbPath, ms, false);
            ms.Close();
        }

        private static string[] RulesTextFile(string version)
        {
            List<string> lines = new List<string>();
            lines.Add("[Definition]");
            lines.Add("titleIds = 00050000101C9300,00050000101C9400,00050000101C9500");
            lines.Add("name = BotW Randomizer");
            lines.Add("path = \"The Legend of Zelda: Breath of the Wild/BotW Randomizer\"");
            lines.Add($"description = Randomizer Version {version}|You need to enable this to play the Randomizer!");
            lines.Add("version = 4");

            return lines.ToArray();
        }

        public static string GenerateSeed()
        {
            string seed = "";
            Random rng = new Random();

            for (int i = 0; i < 15; i++)
            {
                int number = rng.Next(0, 62);
                seed += ConvertToBase62(number).ToUpper();
            }

            char[] chars = seed.ToCharArray();
            chars[4] = '-';
            chars[chars.Length - 5] = '-';
            seed = new string(chars);

            return seed;
        }

        static string ConvertToBase62(int number)
        {
            const string b62Values = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string b62 = "";

            do
            {
                b62 += b62Values[number % 62];
                number /= 62;
            } while (number != 0);

            return b62;
        }

        private static void UpdateSaveData(string bootupFile)
        {
            FileStream fs = File.OpenRead(bootupFile);
            SarcData bootupSarcData = SARC.UnpackRamN(fs);
            fs.Close();

            byte[] gameData = bootupSarcData.Files["GameData/savedataformat.ssarc"];

            Yaz0 yaz = new Yaz0();
            MemoryStream ms = new MemoryStream(gameData);
            SarcData gameDataSarcData = SARC.UnpackRamN(yaz.Decompress(ms));
            ms.Close();

            List<string> fileNames = new List<string>();

            foreach (KeyValuePair<string, byte[]> gameDataFile in gameDataSarcData.Files)
            {
                fileNames.Add(gameDataFile.Key);
            }

            for (int i = 0; i < fileNames.Count; i++)
            {
                if (fileNames[i].StartsWith("/saveformat_"))
                {
                    bool modified = false;

                    MemoryStream gdMs = new MemoryStream(gameDataSarcData.Files[fileNames[i]]);
                    BymlFileData gdByaml = ByamlFile.LoadN(gdMs);
                    gdMs.Close();

                    dynamic botwObjects = gdByaml.RootNode["file_list"];

                    for (int j = 0; j < botwObjects[1].Count; j++)
                    {
                        if (modifiedActors.ContainsKey(botwObjects[1][j]["DataName"]))
                        {
                            string newActorName = modifiedActors[botwObjects[1][j]["DataName"]];

                            gdByaml.RootNode["file_list"][1][j]["HashValue"] = unchecked((int)Crc32.Compute(newActorName));
                            gdByaml.RootNode["file_list"][1][j]["DataName"] = newActorName;
                            modified = true;
                        }
                    }

                    if (modified)
                        gameDataSarcData.Files[fileNames[i]] = ByamlFile.SaveN(gdByaml);
                }

                byte[] newData = SARC.PackN(gameDataSarcData).Item2;
                ms = new MemoryStream(newData);
                Stream compressed = yaz.Compress(ms);

                bootupSarcData.Files["GameData/savedataformat.ssarc"] = compressed.ToArray();

                ms = new MemoryStream(newData);
                LibHelpers.RstbFile("GameData/savedataformat.sarc", ms, false);
                ms.Close();

                byte[] newBootupFile = SARC.PackN(bootupSarcData).Item2;
                File.WriteAllBytes(bootupFile, newBootupFile);
            }
        }

        private static void UpdateGameData(string bootupFile)
        {
            FileStream fs = File.OpenRead(bootupFile);
            SarcData bootupSarcData = SARC.UnpackRamN(fs);
            fs.Close();

            byte[] gameData = bootupSarcData.Files["GameData/gamedata.ssarc"];

            Yaz0 yaz = new Yaz0();

            MemoryStream ms = new MemoryStream(gameData);
            Stream s = yaz.Decompress(ms);

            SarcData gameDataSarcData = SARC.UnpackRamN(s);

            List<string> fileNames = new List<string>();

            foreach (KeyValuePair<string, byte[]> gameDataFile in gameDataSarcData.Files)
            {
                fileNames.Add(gameDataFile.Key);
            }

            for (int i = 0; i < fileNames.Count; i++)
            {
                bool modified = false;

                MemoryStream gdMs = new MemoryStream(gameDataSarcData.Files[fileNames[i]]);
                BymlFileData gdByaml = ByamlFile.LoadN(gdMs);
                gdMs.Close();

                if (gdByaml.RootNode.ContainsKey("bool_data"))
                {
                    dynamic botwObjects = gdByaml.RootNode["bool_data"];
                    for (int j = 0; j < botwObjects.Count; j++)
                    {
                        if (modifiedActors.ContainsKey(botwObjects[j]["DataName"]))
                        {
                            string newActorName = modifiedActors[botwObjects[j]["DataName"]];

                            gdByaml.RootNode["bool_data"][j]["HashValue"] = unchecked((int)Crc32.Compute(newActorName));
                            gdByaml.RootNode["bool_data"][j]["DataName"] = newActorName;
                            modified = true;
                        }

                        if (botwObjects[j]["DataName"].Equals("IsGet_AncientArrow") ||
                            botwObjects[j]["DataName"].StartsWith("IsGet_Animal") ||
                            botwObjects[j]["DataName"].StartsWith("IsGet_App") ||
                            botwObjects[j]["DataName"].Equals("IsGet_BeeHome") ||
                            botwObjects[j]["DataName"].Equals("IsGet_BombArrow_A") ||
                            botwObjects[j]["DataName"].Equals("IsGet_ElectricArrow") ||
                            botwObjects[j]["DataName"].Equals("IsGet_FireArrow") ||
                            botwObjects[j]["DataName"].Equals("IsGet_IceArrow") ||
                            (botwObjects[j]["DataName"].StartsWith("IsGet_Weapon") && !botwObjects[j]["DataName"].Contains("Weapon_Sword_070")) ||
                            botwObjects[j]["DataName"].StartsWith("IsGet_Item") ||
                            botwObjects[j]["DataName"].Equals("IsGet_NormalArrow") ||
                            botwObjects[j]["DataName"].Equals("IsGet_KeySmall") ||

                            botwObjects[j]["DataName"].Equals("IsGet_Obj_Camera") ||
                            botwObjects[j]["DataName"].Equals("IsGet_Obj_IceMaker") ||
                            botwObjects[j]["DataName"].Equals("IsGet_Obj_RemoteBomb") ||
                            botwObjects[j]["DataName"].Equals("IsGet_Obj_StopTimer") ||
                            botwObjects[j]["DataName"].Equals("IsGet_Obj_RemoteBombLv2") ||
                            botwObjects[j]["DataName"].Equals("IsGet_Obj_StopTimerLv2") ||
                            botwObjects[j]["DataName"].Equals("IsGet_Obj_Magnetglove") ||
                            botwObjects[j]["DataName"].Equals("IsGet_Obj_Motorcycle") ||
                            botwObjects[j]["DataName"].Equals("IsGet_Obj_Maracas") ||
                            botwObjects[j]["DataName"].Equals("IsGet_Obj_AmiiboItem") ||
                            botwObjects[j]["DataName"].Equals("IsGet_Obj_Album") ||
                            botwObjects[j]["DataName"].Equals("IsGet_Obj_PictureBook") ||
                            botwObjects[j]["DataName"].Equals("IsGet_Obj_FireWoodBundle") ||

                            botwObjects[j]["DataName"].StartsWith("Guide") ||
                            botwObjects[j]["DataName"].StartsWith("IsHelp") ||
                            botwObjects[j]["DataName"].Equals("FirstTips") ||
                            botwObjects[j]["DataName"].StartsWith("Clear_Dungeon") ||

                            botwObjects[j]["DataName"].Equals("IsPlayed_Demo103_0") ||
                            botwObjects[j]["DataName"].Equals("Demo042_0") ||
                            botwObjects[j]["DataName"].Equals("Demo042_1") ||
                            botwObjects[j]["DataName"].Equals("MapTower_07") ||
                            botwObjects[j]["DataName"].Equals("MapTower_07_Demo") ||
                            botwObjects[j]["DataName"].Equals("Open_StartPoint") ||
                            botwObjects[j]["DataName"].Equals("IsPlayed_Demo164_0") ||
                            botwObjects[j]["DataName"].Equals("IsPlayed_Demo166_0") ||
                            botwObjects[j]["DataName"].Equals("IsPlayed_Demo042_0") ||
                            botwObjects[j]["DataName"].Equals("IsPlayed_Demo042_1") ||
                            botwObjects[j]["DataName"].Equals("MapTower_DemoFirst")
                            )
                        {
                            gdByaml.RootNode["bool_data"][j]["InitValue"] = unchecked(1);
                            modified = true;
                        }
                    }
                }

                if (gdByaml.RootNode.ContainsKey("s32_data"))
                {
                    dynamic botwObjects = gdByaml.RootNode["s32_data"];
                    for (int j = 0; j < botwObjects.Count; j++)
                    {
                        if (botwObjects[j]["DataName"].Equals("Location_MapTower07"))
                        {
                            gdByaml.RootNode["s32_data"][j]["InitValue"] = unchecked(1);
                            modified = true;
                        }
                    }
                }

                if (modified)
                {
                    gameDataSarcData.Files[fileNames[i]] = ByamlFile.SaveN(gdByaml);
                }
            }

            byte[] newData = SARC.PackN(gameDataSarcData).Item2;
            ms = new MemoryStream(newData);
            byte[] compressed = yaz.Compress(ms).ToArray();

            bootupSarcData.Files["GameData/gamedata.ssarc"] = compressed;

            ms = new MemoryStream(newData);
            LibHelpers.RstbFile("GameData/gamedata.sarc", ms, false);
            ms.Close();

            byte[] newBootupFile = SARC.PackN(bootupSarcData).Item2;
            File.WriteAllBytes(bootupFile, newBootupFile);
        }

        private static void OpenMainFieldMapFile(string mapFile, string mapType, Dictionary<string, bool> randomizationSettings)
        {
            // Yaz0 decompress the .smubin file and open it as a Byaml
            byte[] dungeonStaticData = File.ReadAllBytes(mapFile);
            BymlFileData byaml;
            MemoryStream ms = new MemoryStream(dungeonStaticData);
            Yaz0 yaz = new Yaz0();

            if (IsYaz0(dungeonStaticData))
            {
                Stream s = yaz.Decompress(ms);
                byaml = ByamlFile.LoadN(s);
                s.Close();
            }
            else
            {
                byaml = ByamlFile.LoadN(ms);
            }

            ms.Close();

            //Prepare a new dictionary instance for each obj to remove any references
            List<dynamic> objectList = (List<dynamic>)byaml.RootNode["Objs"];
            // For every object in the map, randomize it!
            for (int i = 0; i < objectList.Count; i++)
            {
                Dictionary<string, dynamic> actorObj = new Dictionary<string, dynamic>();
                foreach (var item in (Dictionary<string, dynamic>)objectList[i])
                {
                    actorObj.Add(item.Key, item.Value);
                }

                //Prepare a new dictionary instance for each obj to remove any references
                Dictionary<string, dynamic> paramDict = new Dictionary<string, dynamic>();
                if (objectList[i].ContainsKey("!Parameters"))
                {
                    foreach (var item in (Dictionary<string, dynamic>)objectList[i]["!Parameters"])
                    {
                        paramDict.Add(item.Key, item.Value);
                    }
                }

                RandomizeMapObject(ref paramDict, ref actorObj, mapType, randomizationSettings);

                actorObj["!Parameters"] = paramDict;
                objectList[i] = actorObj;
            }

            // Save the modified byaml data
            byte[] dungeonData = ByamlFile.SaveN(byaml);
            ms = new MemoryStream(dungeonData);
            byte[] dungeonByamlData = yaz.Compress(ms).ToArray();
            ms.Close();

            File.WriteAllBytes(mapFile, dungeonByamlData);

            // Add the new map file size in the RSTB list
            string fileName = Path.GetFileNameWithoutExtension(mapFile);
            string mapName = fileName.Split('_')[0];
            string rstbPath = $"Aoc/0010/Map/MainField/{mapName}/{fileName}.mubin";

            ms = new MemoryStream(dungeonData);
            LibHelpers.RstbFile(rstbPath, ms, false);
            ms.Close();
        }

        private static bool ShouldBeRandomized(string unitconfigname, Dictionary<string, bool> randomizationSettings)
        {
            if (randomizationSettings.ContainsKey("randomizeArmorCheckbox") && randomizationSettings["randomizeArmorCheckbox"] == true && unitconfigname.StartsWith("Armor"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeSwordsCheckbox") && randomizationSettings["randomizeSwordsCheckbox"] == true && unitconfigname.StartsWith("Weapon_Sword"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeLongSwordsCheckbox") && randomizationSettings["randomizeLongSwordsCheckbox"] == true && unitconfigname.StartsWith("Weapon_Lsword"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeSpearsCheckbox") && randomizationSettings["randomizeSpearsCheckbox"] == true && unitconfigname.StartsWith("Weapon_Spear"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeBowsCheckbox") && randomizationSettings["randomizeBowsCheckbox"] == true && unitconfigname.StartsWith("Weapon_Bow"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeShieldsCheckbox") && randomizationSettings["randomizeShieldsCheckbox"] == true && unitconfigname.StartsWith("Weapon_Shield"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeEnemiesCheckbox") && randomizationSettings["randomizeEnemiesCheckbox"] == true && unitconfigname.StartsWith("Enemy"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeInsectsCheckbox") && randomizationSettings["randomizeInsectsCheckbox"] == true && unitconfigname.StartsWith("Animal_Insect"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeFishesCheckbox") && randomizationSettings["randomizeFishesCheckbox"] == true && unitconfigname.StartsWith("Animal_Fish"))
                return true;
            if (randomizationSettings.ContainsKey("randomizePlantsCheckbox") && randomizationSettings["randomizePlantsCheckbox"] == true && unitconfigname.StartsWith("Item_Plant"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeMushroomsCheckbox") && randomizationSettings["randomizeMushroomsCheckbox"] == true && unitconfigname.StartsWith("Item_Mushroom"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeFruitsCheckbox") && randomizationSettings["randomizeFruitsCheckbox"] == true && unitconfigname.StartsWith("Item_Fruit"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeAnimalsCheckbox") && randomizationSettings["randomizeAnimalsCheckbox"] == true && unitconfigname.StartsWith("Animal"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeOresCheckbox") && randomizationSettings["randomizeOresCheckbox"] == true && unitconfigname.StartsWith("Item_Ore"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeRupeesCheckbox") && randomizationSettings["randomizeRupeesCheckbox"] == true && unitconfigname.StartsWith("PutRupee"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeArrowsCheckbox") && randomizationSettings["randomizeArrowsCheckbox"] == true && unitconfigname.Contains("Arrow"))
                return true;
            if (randomizationSettings.ContainsKey("randomizeArmorShopsCheckbox") && randomizationSettings["randomizeArmorShopsCheckbox"] == true && unitconfigname.StartsWith("Mannequin"))
                return true;

            return false;
        }

        private static int currentChestCount = 0;

        private static void RandomizeMapObject(ref Dictionary<string, dynamic> actorParams, ref Dictionary<string, dynamic> actorObj, string mapType, Dictionary<string, bool> randomizationSettings)
        {
            string unitConfigName = actorObj["UnitConfigName"];

            // If the enemy is supposed to spawn in an arena, don't randomize it
            if (actorParams.ContainsKey("IsNearCreate") && actorParams["IsNearCreate"] == true)
            {
                return;
            }
            // Ignore Master Mode objects
            else if (actorParams.ContainsKey("IsHardModeActor") && actorParams["IsHardModeActor"] == true)
            {
                return;
            }
            // Remove unwanted objects
            else if (eventsToDisable.Contains(actorObj["HashId"]))
            {
                actorObj["UnitConfigName"] = "Dummy";
            }
            // Remove old man by FORCE!
            else if (actorObj["UnitConfigName"].StartsWith("Npc_King"))
            {
                actorObj["UnitConfigName"] = "Dummy";
            }
            // If the map actor is a treasure chest, check if it should have a spirit orb inside, and place it if so
            else if (unitConfigName.StartsWith("TBox_") && !unitConfigName.Contains("Gamble"))
            {
                currentChestCount++;

                // Place Paraglider in Plateau
                if (paragliderChest == actorObj["HashId"])
                {
                    // Set the chest contents to the Paraglider
                    actorParams["DropActor"] = "PlayerStole2";
                    File.WriteAllText(spoilerLogPath, File.ReadAllText(spoilerLogPath) + "\n" + "Paraglider: " + actorObj["HashId"]);
                }
                else
                {
                    // Only randomize actor if the player chose to
                    if (ShouldBeRandomized(actorParams["DropActor"], randomizationSettings))
                    {
                        RandomizeParameter("DropActor", ref actorParams, actorObj);
                    }
                }
            }
            else if (unitConfigName.Equals("TwnObj_GanonGrudgeSolid_Generator_A_01"))
            {
                actorParams["ActorName"] = "Enemy_Bokoblin_Gold";
                RandomizeParameter("ActorName", ref actorParams);
            }
            else
            {
                // Only randomize actor if the player chose to
                if (ShouldBeRandomized(unitConfigName, randomizationSettings))
                {
                    // Randomize the unit name of a map actor
                    string newObject = GetRandomMapObject(unitConfigName);
                    if (newObject != null)
                    {
                        ModifyActorName(ref actorObj, newObject, mapType);
                    }
                }

                // Randomize the parameters of a map actor
                RandomizeParameter("EquipItem1", ref actorParams);
                RandomizeParameter("EquipItem2", ref actorParams);
                RandomizeParameter("EquipItem3", ref actorParams);
                RandomizeParameter("EquipItem4", ref actorParams);
                RandomizeParameter("ArrowName", ref actorParams);
            }
        }

        private static void ModifyActorName(ref Dictionary<string, dynamic> actorObj, dynamic value, string mapType)
        {
            long hashId = actorObj["HashId"];

            string originalActorName = mapType + "_" + actorObj["UnitConfigName"] + "_" + hashId;
            string modifiedActorName = mapType + "_" + value + "_" + hashId;

            if (!modifiedActors.ContainsKey(originalActorName))
            {
                modifiedActors.Add(originalActorName, modifiedActorName);
            }

            actorObj["UnitConfigName"] = value;
        }

        private static void RandomizeParameter(string paramName, ref Dictionary<string, dynamic> actorParams, Dictionary<string, dynamic> actorObj = null)
        {
            // If a parameter with a specific name is found
            if (actorParams.ContainsKey(paramName))
            {
                string paramValue = actorParams[paramName];
                if (paramValue != "Default")
                {
                    string randomObj;
                    if (paramName == "DropActor")
                    {
                        KeyValuePair<string, string> newObject = GetChestLootObject();
                        randomObj = newObject.Key;
                        if (actorObj != null && !String.IsNullOrEmpty(newObject.Value))
                        {
                            File.WriteAllText(spoilerLogPath, File.ReadAllText(spoilerLogPath) + "\n" + $"{newObject.Value}: " + actorObj["HashId"]);
                        }
                    }
                    else
                    {
                        randomObj = GetRandomMapObject(paramValue);
                    }

                    if (randomObj != null)
                    {
                        actorParams[paramName] = randomObj;
                    }
                }
            }
        }

        private static KeyValuePair<string, string> GetChestLootObject()
        {
            // Get a random value from a list if the object is found within said list
            chestObjectsTable.ChestItems = chestObjectsTable.ChestItems.OrderBy(x => random.Next()).ToList();
            KeyValuePair<string, string> objectInList = chestObjectsTable.ChestItems[0];
            chestObjectsTable.ChestItems.Remove(objectInList);
            return objectInList;
        }

        private static string GetRandomMapObject(string objectName)
        {
            // Get a random value from a list if the object is found within said list
            for (int i = 0; i < overworldObjectsTable.OverworldObjects.Count; i++)
            {
                for (int j = 0; j < overworldObjectsTable.OverworldObjects.ElementAt(i).Key.Count; j++)
                {
                    if (overworldObjectsTable.OverworldObjects.ElementAt(i).Key[j] == objectName)
                    {
                        int listRandomIndex = random.Next(overworldObjectsTable.OverworldObjects.ElementAt(i).Key.Count);
                        string newObject = overworldObjectsTable.OverworldObjects.ElementAt(i).Key[listRandomIndex];

                        if (overworldObjectsTable.OverworldObjects.ElementAt(i).Value == true)
                        {
                            overworldObjectsTable.OverworldObjects.Remove(overworldObjectsTable.OverworldObjects.ElementAt(i).Key);
                        }

                        return newObject;
                    }
                }
            }

            return null;
        }
    }
}