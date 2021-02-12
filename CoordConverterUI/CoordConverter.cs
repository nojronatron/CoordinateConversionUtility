using CoordinateConversionUtility;
using CoordinateConversionUtility.Helpers;
using System;

namespace CoordConverterUI
{
    class CoordConverter
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("***** Coordinate Converter *****\n");
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Provide an input to get a result.");
                Console.WriteLine();
                PrintUsageInstructions();
            }

            else if (args.Length == 1)
            {
                string currentArg = args[0].Trim().ToUpper();

                //  HELP option called
                if (currentArg.Contains("-H") || currentArg.Contains("--HELP"))
                {
                    Console.WriteLine("Help");
                    PrintUsageInstructions();
                }

                //  GRIDSQUARE input default so return a DDM
                else if (currentArg.Length == 6)
                {
                    Console.WriteLine("Maybe a gridsquare. . .");
                    var ccu = new CoordinateConverter();
                    Console.WriteLine(ccu.ConvertGridsquareToDDM(currentArg).ToString());
                }

                //  No args but longer than a gridsquare so probably a DDM
                else if (currentArg.Length > 6)
                {
                    var ih = new InputHelper();
                    if(ih.IsDDM(currentArg, false, out string validDDM))
                    {
                        Console.WriteLine($"Resulting DDM: { validDDM }.");
                    }
                }

            }

            else if (args.Length > 1)
            {
                for (int idx=0; idx < args.Length; idx++)
                {
                    if (args[idx].Contains("-ddm"))
                    {
                        Console.WriteLine("-ddm command detected.");
                        var ih = new InputHelper();
                        if (ih.GetCommand(args[idx]) == "-ddm")
                        {
                            Console.WriteLine("-ddm command detected.");
                        }

                        if (ih.IsDDM(args[idx + 1], false, out string validDDM))
                        {
                            Console.WriteLine($"Resulting DDM: { validDDM }.");
                        }
                    }

                    else if (args[idx].Contains("-direwolf"))
                    {
                        Console.WriteLine("-direwolf command detected.");
                        var ih = new InputHelper();

                        if (ih.IsDDM(args[idx + 1], true, out string validDWDDM))
                        {
                            Console.WriteLine($"Resulting DIREWOLF DDM: { validDWDDM }.");
                        }
                    }

                    else if (args[idx].Contains("-dd"))
                    {
                        Console.WriteLine("-dd command detected.");
                        var ih = new InputHelper();

                        if (ih.GetCommand(args[idx]) == "-dd")
                        {
                            Console.WriteLine("-dd command detected.");
                        }

                        if (ih.IsDD(args[idx + 1], out string validDD))
                        {
                            Console.WriteLine($"Resulting DD: { validDD }.");
                        }
                    }

                    else if (args[idx].Contains("-dms"))
                    {
                        Console.WriteLine("-dms command detected.");
                        var ih = new InputHelper();

                        if (ih.GetCommand(args[idx]) == "-dms")
                        {
                            Console.WriteLine("-dms command detected.");
                        }

                        if (ih.IsDMS(args[idx + 1], out string validDMS))
                        {
                            Console.WriteLine($"Resulting DMS: { validDMS }.");
                        }
                    }

                    else if (args[idx].Contains("-grid"))
                    {
                        Console.WriteLine("-grid command detected.");
                        var ih = new InputHelper();

                        if (ih.GetCommand(args[idx]) == "-grid")
                        {
                            Console.WriteLine("-grid command detected.");
                        }

                        var currentArg = args[idx + 1];
                        if (ih.IsGridsquare(currentArg, out string validatedGridsquare))
                        {
                            var ccu = new CoordinateConverter();
                            var ccuString = ccu.ConvertGridsquareToDDM(validatedGridsquare);
                            Console.WriteLine($"Resulting DDM: { ccuString }.");
                        }
                    }

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
