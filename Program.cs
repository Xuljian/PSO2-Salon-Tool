using CharacterMakingFileTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PSO2SalonTool
{
    class Program
    {
        static string outputPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        static string ext = "default";
        static bool fixNaHeight = false;
        static string inputPath = null;
        static string inputFilename = null;
        static bool toWait = false;
        static List<string> ALLOWED_EXT = new List<string>()
        {
            "fhp", "fnp", "fcp", "fdp",
            "mhp", "mnp", "mcp", "mdp",
            "default"
        };

        static void Main(string[] args)
        {
            try
            {
                List<string> argList = args.ToList();
                if (argList.Count > 0)
                {
                    ProcessArguments(argList);
                    bool validationResult = ValidateAndPrepArguments();

                    if (validationResult)
                    {
                        Process();
                        if (toWait)
                        {
                            Console.WriteLine("Complete!\nPress any key to continue...");
                            Console.ReadLine();
                        }
                    }
                }
                else
                {
                    string message = $"You would need to run this program with a batch file to utilize the 2 options that you can pass into this program\n" +
                        $"1. -o, this option is to set the output path of the processed / converted file to its respective folder\n" +
                        $"2. -ext, this option is to set the race along with the gender of your choice. Example: fhp.\n" +
                        $"3. -na this option is to fix na height problem. Not specifying this will give you the height given by the CML file" +
                        $"PS: this program currently only supports .cml files" +
                        $"The libraries used in this solution are from https://github.com/omegatari/PSO2-Salon-Tool, only the Character_Making_File_Tool library along with its dependencies is used.\n\n";

                    Console.Write(message);
                    if (toWait)
                    {
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                string message = $"You would need to run this program with a batch file to utilize the 2 options that you can pass into this program\n" +
                    $"1. -o, this option is to set the output path of the processed / converted file to its respective folder\n" +
                    $"2. -ext, this option is to set the race along with the gender of your choice. Example: fhp.\n" +
                    $"3. -na this option is to fix na height problem. Not specifying this will give you the height given by the CML file" +
                    $"PS: this program currently only supports .cml files" +
                    $"The libraries used in this solution are from https://github.com/omegatari/PSO2-Salon-Tool, only the Character_Making_File_Tool library along with its dependencies is used.\n\n";

                Console.Write(message);
                if (toWait)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }
            }
        }

        static void Process()
        {
            CharacterHandler characterHandler = new CharacterHandler();
            characterHandler.ParseCML(inputPath);

            if (ext.ToLower() == "default")
            {
                ext = "p";
                switch ((Race)(int)characterHandler.xxpGeneral.baseDOC.race)
                {
                    case Race.Cast:
                        {
                            ext = "c" + ext;
                            break;
                        }
                    case Race.Deuman:
                        {
                            ext = "d" + ext;
                            break;
                        }
                    case Race.Human:
                        {
                            ext = "h" + ext;
                            break;
                        }
                    case Race.Newman:
                        {
                            ext = "n" + ext;
                            break;
                        }
                }
                switch ((Gender)(int)characterHandler.xxpGeneral.baseDOC.gender)
                {
                    case Gender.Female:
                        {
                            ext = "f" + ext;
                            break;
                        }
                    case Gender.Male:
                        {
                            ext = "m" + ext;
                            break;
                        }
                }
            }

            characterHandler.EncryptAndSaveFile($"{outputPath}\\{inputFilename}.{ext}", 0, fixNaHeight, false, out string windowVersion);
            Console.Write($"Output: {outputPath}\\{inputFilename}.{ext}");
        }

        static bool ValidateAndPrepArguments()
        {
            if (!ALLOWED_EXT.Contains(ext))
            {
                Console.WriteLine("Invalid -ext option");
                return false;
            }
            if (string.IsNullOrEmpty(inputPath))
            {
                FileInfo file = new FileInfo(inputPath);
                if (!file.Exists)
                {
                    Console.WriteLine("Input file is invalid");
                    return false;
                }
                
            }
            if (!outputPath.Contains("\\"))
            {
                outputPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + "\\" + outputPath;
            }
            int lastIndex = inputPath.LastIndexOf("\\");
            inputFilename = inputPath.Substring(lastIndex);
            inputFilename = inputFilename.Split(".")[0];

            FileInfo output = new FileInfo(outputPath + "\\");
            if (!output.Exists)
            {
                output.Directory.Create();
            }
            return true;
        }

        static void ProcessArguments(List<string> args)
        {
            if (args.Count == 1)
            {
                inputPath = args[0];
                if (inputPath.ToLower().IndexOf(".cml") < 0)
                {
                    inputPath = null;
                    return;
                }
            }
            else
            {
                bool oFlag = false;
                bool oFlagReset = false;
                bool extFlag = false;
                bool extFlagReset = false;
                args.ForEach(arg =>
                {
                    bool occupied = false;
                    if (!oFlag && !extFlag && arg.Length >= 4 && arg.ToLower().Substring(arg.Length - 4, 4) == ".cml" && string.IsNullOrEmpty(inputPath))
                    {
                        inputPath = arg;
                        occupied = true;
                    }
                    if (arg.ToLower() == "-o")
                    {
                        oFlag = true;
                        occupied = true;
                    }
                    if (arg.ToLower() == "-ext")
                    {
                        extFlag = true;
                        occupied = true;
                    }
                    if (arg.ToLower() == "-w")
                    {
                        toWait = true;
                        occupied = true;
                    }
                    if (arg.ToLower() == "-na")
                    {
                        fixNaHeight = true;
                        occupied = true;
                    }
                    if (!occupied)
                    {
                        if (oFlag)
                        {
                            outputPath = arg;
                            oFlag = false;
                        }
                        if (extFlag)
                        {
                            ext = arg;
                            extFlag = false;
                        }
                    }
                    if (oFlag)
                    {
                        if (!oFlagReset)
                        {
                            oFlagReset = true;
                        }
                        else
                        {
                            oFlagReset = false;
                            oFlag = false;
                        }
                    }
                    if (extFlag)
                    {
                        if (!extFlagReset)
                        {
                            extFlagReset = true;
                        }
                        else
                        {
                            extFlagReset = false;
                            extFlag = false;
                        }
                    }
                });
            }
        }
    }
}
