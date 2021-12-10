using ByamlExt.Byaml;
using EveryFileExplorer;
using SARCExt;
using System;
using System.Collections.Generic;
using System.IO;
using Toolbox.Library;
using Toolbox.Library.IO;

namespace BotwRandoLib
{
    internal class LibHelpers
    {
        public static RSTB rstb = new RSTB();

        public static bool CopyMapFiles(string sourcePath, string targetPath)
        {
            try
            {
                // Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                {
                    if (Path.GetFileNameWithoutExtension(dirPath).Contains("-"))
                    {
                        Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                    }
                }

                // Copy all the files & replaces any file with the same name
                foreach (string newPath in Directory.GetFiles(sourcePath, "*.smubin", SearchOption.AllDirectories))
                {
                    if (Path.GetFileNameWithoutExtension(newPath).Contains("-"))
                    {
                        File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        internal static void CopyRstbFile(string updateRstbFile, string gfxPackRstbFile)
        {
            // Create the graphic pack System folder if necessary
            if (!File.Exists(gfxPackRstbFile))
                Directory.CreateDirectory(Path.GetDirectoryName(gfxPackRstbFile));

            // Copy over the rstb file to the graphic pack folder
            File.Copy(updateRstbFile, gfxPackRstbFile);

            rstb.LoadFile(gfxPackRstbFile);
        }

        internal static void CopyAndInjectEventFile(byte[] demoFile, string demoName, string updateEventsPath, string gfxPackEventsPath, bool isInBootup = false)
        {
            if (isInBootup)
            {
                // Create the graphic pack Event folder if necessary
                if (!Directory.Exists(Path.GetDirectoryName(gfxPackEventsPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(gfxPackEventsPath));

                string eventPath = $"EventFlow/{demoName}.bfevfl";

                FileStream fs = File.OpenRead(gfxPackEventsPath);
                SarcData bootupSarcData = SARC.UnpackRamN(fs);
                fs.Close();

                bootupSarcData.Files[eventPath] = demoFile;
                byte[] newBootupData = SARC.PackN(bootupSarcData).Item2;
                File.WriteAllBytes(gfxPackEventsPath, newBootupData);

                MemoryStream ms = new MemoryStream(demoFile);
                RstbFile(eventPath, ms, false);
                ms.Close();
            }
            else
            {
                // Create the graphic pack Event folder if necessary
                if (!Directory.Exists(gfxPackEventsPath))
                    Directory.CreateDirectory(gfxPackEventsPath);

                // Copy the game's event file to the graphic pack
                string newEventFile = Path.Combine(gfxPackEventsPath, $"{demoName}.sbeventpack");
                File.Copy(Path.Combine(updateEventsPath, $"{demoName}.sbeventpack"), newEventFile);

                // Yaz0 decompress the event file and open it as a SARC 
                FileStream fs = File.OpenRead(newEventFile);
                SarcData demoData = SARC.UnpackRamN(YAZ0.Decompress(newEventFile));
                fs.Close();

                // Set the event data to the one provided by the Randomizer
                demoData.Files[$"EventFlow/{demoName}.bfevfl"] = demoFile;

                // Save the event file back as a SARC
                fs = File.OpenWrite(newEventFile);
                Tuple<int, byte[]> newSarcData = SARC.PackN(demoData);
                fs.Close();

                // Yaz0 compress the SARC file
                File.WriteAllBytes(newEventFile, YAZ0.Compress(newSarcData.Item2));

                // Add the modified files to the RSTB
                MemoryStream ms = new MemoryStream(demoFile);
                RstbFile($"EventFlow/{demoName}.bfevfl", ms, false);
                ms.Close();

                ms = new MemoryStream(newSarcData.Item2);
                RstbFile($"Event/{demoName}.beventpack", ms, false);
                ms.Close();
            }
        }

        internal static List<uint> GetEventsToDisable()
        {
            List<uint> events = new List<uint>();
            events.Add(956566239);
            events.Add(1667561929);
            events.Add(2158631089);
            events.Add(857276579);
            events.Add(2619496973);
            events.Add(1470985759);
            events.Add(2229064910);
            events.Add(342083935);
            events.Add(4091274328);
            events.Add(146112473);
            events.Add(1754945523);
            events.Add(1110838087);
            events.Add(892419025);
            events.Add(376831062);
            events.Add(1634913472);
            events.Add(3593817625);
            events.Add(2295470152);
            events.Add(3612460687);
            events.Add(3758829974);
            events.Add(2520755267);
            events.Add(4141537481);
            return events;
        }

        internal static List<uint> GetParagliderChests()
        {
            List<uint> chests = new List<uint>();
            chests.Add(473644037);
            chests.Add(759164510);
            chests.Add(4093217196);
            chests.Add(1650509316);
            chests.Add(3746546895);
            chests.Add(3791240011);
            chests.Add(2029054705);
            chests.Add(2009260213);
            chests.Add(591392381);
            chests.Add(217594417);
            chests.Add(3138298400);
            chests.Add(350171171);
            chests.Add(3005657673);
            chests.Add(2080206557);
            chests.Add(407446232);
            chests.Add(614466614);
            chests.Add(1403466912);
            chests.Add(3273140529);
            chests.Add(4046343091);
            chests.Add(2990124756);
            chests.Add(3381003323);
            chests.Add(3729177928);
            chests.Add(3817004620);
            chests.Add(1757302122);
            chests.Add(950861039);
            chests.Add(2798152012);
            chests.Add(946822747);
            chests.Add(3063385243);
            chests.Add(2728429337);
            chests.Add(1313140003);
            chests.Add(305852247);
            chests.Add(4125758579);
            return chests;
        }

        internal static void RstbFiles(string rstbFile)
        {
            FileWriter fw = new FileWriter(rstbFile);
            rstb.Write(fw);
            fw.Close();

            Yaz0 yaz = new Yaz0();

            FileStream fs = File.OpenRead(rstbFile);
            Stream compressed = yaz.Compress(fs);
            fs.Close();

            File.WriteAllBytes(rstbFile, compressed.ToArray());
        }

        internal static void RstbFile(string fileName, Stream fileStream, bool isCompressed)
        {
            rstb.SetEntry(fileName, fileStream, isCompressed);
        }

        internal static bool CopyShrineFiles(string baseShrinesPath, string dlcShrinesPath, string gfxPackBaseShrinesPath, string gfxPackDlcShrinesPath, ref List<string> dungeonFiles)
        {
            try
            {
                Directory.CreateDirectory(gfxPackBaseShrinesPath);
                Directory.CreateDirectory(gfxPackDlcShrinesPath);

                foreach (string baseShrine in Directory.GetFiles(baseShrinesPath, "Dungeon*.pack"))
                {
                    string newLocation = Path.Combine(gfxPackBaseShrinesPath, Path.GetFileName(baseShrine));
                    File.Copy(baseShrine, newLocation);
                    dungeonFiles.Add(newLocation);
                }

                foreach (string dlcShrine in Directory.GetFiles(dlcShrinesPath, "Dungeon*.pack"))
                {
                    string newLocation = Path.Combine(gfxPackDlcShrinesPath, Path.GetFileName(dlcShrine));
                    File.Copy(dlcShrine, newLocation);
                    dungeonFiles.Add(newLocation);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool IsDirectoryValid(string directory)
        {
            return new DirectoryInfo(directory).Exists;
        }
    }
}