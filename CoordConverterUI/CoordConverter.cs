using CoordinateConversionUtility;
using CoordinateConversionUtility.Helpers;

using System;
using System.Text.RegularExpressions;

namespace CoordConverterUI
{
    class CoordConverter
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("***** Coordinate Conversion Utility *****\n");
            if (args.Length == 0)
            {
                Console.WriteLine("Provide an input to get a result...");
                Console.WriteLine();
                PrintUsageInstructions();
            }

            if (args.Length == 1)
            {
                string currentArg = args[0].Trim().ToLower();

                if (currentArg.Contains("-h") || currentArg.Contains("--help"))
                {
                    Console.WriteLine("Help");
                    PrintUsageInstructions();
                    return;
                }

                if (currentArg.Length >= 6)
                {
                    var ccu = new CoordinateConverter();
                    Console.WriteLine(ccu.ConvertGridsquareToDDM(currentArg).ToString());
                    return;
                }

                //  Core functionality:
                //  input: --help | -h; return: usage instructions

                //  input: string gridsquare; return: string ddm

                //  input: string ddm; return: string gridsquare

                //  input: string DD e.g. 47.54321,-122.12345; return: string DD well-formatted

                //  input: string DMS e.g. N47 32 35.6, W122 7 24.4; return: string DMS well-formatted

                //  input: string DDM; return: string DDM well-formatted


                //  Secondary functionality:
                //  input: DD; return string DDM well-formatted
                //  input: DD; return string DMS well-formatted
                //  input: DDM; return string DD well-formatted
                //  input: DDM; return string DMS well-formatted
                //  input: DMS; return string DD well-formatted
                //  input: DMS; return string DDM well-formatted

                //  input: gridsquare; return: string DD
                //  input: gridsquare; return: string DMS
                //  input: DD; return string gridsquare
                //  input: DMS; return string gridsquare

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
