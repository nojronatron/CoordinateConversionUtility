using CoordinateConversionLibrary;
using CoordinateConversionLibrary.Helpers;
using CoordinateConversionLibrary.Models;
using System;
using System.Collections.Generic;

namespace CoordinateConverterCmd
{
    internal class CoordConverter
    {
        private static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                PrintUsageInstructions();
            }

            else if (args.Length == 1)
            {
                string currentArg = args[0].Trim().ToUpper();

                if (currentArg.Contains("-H") || currentArg.Contains("--HELP"))
                {
                    PrintUsageInstructions();
                }

                else if (currentArg.Length == 6)
                {
                    if (InputHelper.IsGridsquare(currentArg, out string argGridsquare))
                    {
                        var ccu = new GridDdmExpert();
                        Console.WriteLine(ccu.ConvertGridsquareToDDM(argGridsquare).ToString());
                    }
                    else
                    {
                        Console.WriteLine("Command not recognized.");
                        PrintUsageInstructions();
                    }
                }

                else if (currentArg.Length > 6)
                {
                    if (InputHelper.ParseAsDDMCoordinate(currentArg, false, out string validDDM))
                    {
                        Console.WriteLine(validDDM);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                    PrintUsageInstructions();
                }

            }

            else if (args.Length > 1)
            {
                var argsQueue = new Queue<string>(args);

                while (argsQueue.Count > 1)
                {
                    string inputCommand = InputHelper.GetCommand(argsQueue.Dequeue().Trim().ToUpper());
                    string currentInput = argsQueue.Dequeue().Trim().ToUpper();
                    string outputCommand = string.Empty;
                    string result = string.Empty;

                    if (argsQueue.Count > 0)
                    {
                        outputCommand = InputHelper.GetCommand(argsQueue.Dequeue().Trim().ToUpper());
                    }

                    switch (inputCommand)
                    {
                        case "-direwolf":
                            {
                                if (InputHelper.ParseAsDDMCoordinate(currentInput, true, out string validDWDDM))
                                {
                                    if (outputCommand.Length > 0)
                                    {
                                        result = InputHelper.OutputCommandProcessor(inputCommand, validDWDDM, outputCommand);
                                    }
                                    else
                                    {
                                        result = validDWDDM;
                                    }
                                }

                                break;
                            }
                        case "-grid":
                            {
                                if (InputHelper.IsGridsquare(currentInput, out string validGrid))
                                {
                                    if (outputCommand.Length > 0)
                                    {
                                        result = InputHelper.OutputCommandProcessor(inputCommand, validGrid, outputCommand);
                                    }
                                    else
                                    {
                                        var cc = new GridDdmExpert();
                                        DDMCoordinate ddm = cc.ConvertGridsquareToDDM(validGrid);
                                        result = ddm.ToString();
                                    }
                                }
                                break;
                            }
                        case "-dms":
                            {
                                if (InputHelper.ParseAsDMSCoordinate(currentInput, out string validDMS))
                                {
                                    if (outputCommand.Length > 0)
                                    {
                                        result = InputHelper.OutputCommandProcessor(inputCommand, validDMS, outputCommand);
                                    }
                                    else
                                    {
                                        result = validDMS;
                                    }
                                }
                                break;
                            }
                        case "-ddm":
                            {
                                if (InputHelper.ParseAsDDMCoordinate(currentInput, false, out string validDDM))
                                {
                                    if (outputCommand.Length > 0)
                                    {
                                        result = InputHelper.OutputCommandProcessor(inputCommand, validDDM, outputCommand);
                                    }
                                    else
                                    {
                                        result = validDDM;
                                    }
                                }
                                break;
                            }
                        case "-dd":
                            {
                                if (InputHelper.ParseAsDDCoordinate(currentInput, out string validDD))
                                {
                                    if (outputCommand.Length > 0)
                                    {
                                        result = InputHelper.OutputCommandProcessor(inputCommand, validDD, outputCommand);
                                    }
                                    else
                                    {
                                        result = validDD;
                                    }
                                }
                                break;
                            }
                        default:
                            {
                                //  -h, --help, or something else
                                Console.WriteLine("Invalid input.");
                                PrintUsageInstructions();
                                break;
                            }
                    }

                    Console.WriteLine(result);
                }

            }
            else
            {
                Console.WriteLine("Invalid input.");
                PrintUsageInstructions();
            }
        }

        private static void PrintUsageInstructions()
        {
            var ug = new UserGuide();

            foreach (string section in ug.UsageInstructions)
            {
                Console.WriteLine(section);
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
