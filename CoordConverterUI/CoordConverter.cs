using CoordinateConversionUtility;
using CoordinateConversionUtility.Helpers;
using System;
using System.Collections.Generic;

namespace CoordConverterUI
{
    class CoordConverter
    {
        static void Main(string[] args)
        {
            var ih = new InputHelper();
            Console.Clear();
            Console.WriteLine("***** Coordinate Converter *****\n");
            if (args == null || args.Length == 0)
            {
                PrintUsageInstructions();
            }

            else if (args.Length == 1)
            {
                string currentArg = args[0].Trim().ToUpper();

                if (currentArg.Contains("-H") || currentArg.Contains("--HELP"))
                {
                    Console.WriteLine("Help");
                    PrintUsageInstructions();
                }

                else if (currentArg.Length == 6)
                {
                    if (ih.IsGridsquare(currentArg, out string argGridsquare))
                    {
                        var ccu = new CoordinateConverter();
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
                    if(ih.IsDDM(currentArg, false, out string validDDM))
                    {
                        Console.WriteLine($"Resulting DDM: { validDDM }.");
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
                Queue<string> argsQueue = new Queue<string>(args);
                
                while(argsQueue.Count > 1)
                {
                    var inputCommand = ih.GetCommand(argsQueue.Dequeue().Trim().ToUpper());
                    var currentInput = argsQueue.Dequeue().Trim().ToUpper();
                    var outputCommand = string.Empty;
                    var result = string.Empty;

                    if (argsQueue.Count > 0)
                    {
                        outputCommand = ih.GetCommand(argsQueue.Dequeue().Trim().ToUpper());
                    }

                    switch (inputCommand)
                    {
                        case "-direwolf":
                            {
                                if (ih.IsDDM(currentInput, true, out string validDWDDM))
                                {
                                    if (outputCommand.Length > 0)
                                    {
                                        result = ih.OutputCommandProcessor(inputCommand, validDWDDM, outputCommand);
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
                                if (ih.IsGridsquare(currentInput, out string validGrid))
                                {
                                    if (outputCommand.Length > 0)
                                    {
                                        result = ih.OutputCommandProcessor(inputCommand, validGrid, outputCommand);
                                    }
                                    else
                                    {
                                        CoordinateConverter cc = new CoordinateConverter();
                                        var ddm = cc.ConvertGridsquareToDDM(validGrid);
                                        result = ddm.ToString();
                                    }
                                }
                                break;
                            }
                        case "-dms":
                            {
                                if (ih.IsDMS(currentInput, out string validDMS))
                                {
                                    if (outputCommand.Length > 0)
                                    {
                                        result = ih.OutputCommandProcessor(inputCommand, validDMS, outputCommand);
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
                                if (ih.IsDDM(currentInput, false, out string validDDM))
                                {
                                    if (outputCommand.Length > 0)
                                    {
                                        result = ih.OutputCommandProcessor(inputCommand, validDDM, outputCommand);
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
                                if (ih.IsDD(currentInput, out string validDD))
                                {
                                    if (outputCommand.Length > 0)
                                    {
                                        result = ih.OutputCommandProcessor(inputCommand, validDD, outputCommand);
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

            Console.WriteLine("Press <Enter> to exit. . .");
            Console.ReadLine();
        }

        private static void PrintUsageInstructions()
        {
            UserGuide ug = new UserGuide();
            foreach(string section in ug.UsageInstructions)
            {
                Console.WriteLine(section);
                Console.WriteLine();
            }

            Console.WriteLine();
        }

    }
}
