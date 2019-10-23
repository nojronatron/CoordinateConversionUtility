using CoordinateConversionUtility;
using System;

namespace CoordConverterUI
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("***** Fun with Coordinate Conversion Utility dll *****");
            Console.WriteLine();

            string result = "";
            string lynnwoodDD = "47.8125N,122.2917W";
            string lynnwoodDMS = "47*48'45\"N,122*17'30\"W";
            string[] lynnwoodWA = { "47*48.75'N", "122*17.5'W", "CN87ut" };
            string[] munich = { "48*8.8'N", "11*36.5'E", "JN58td" };
            string[] montevideo = { "34*54.6'S", "56*12.7'W", "GF15vc" };
            string[] washingonDC = { "38*55.2'N", "77*3.9'W", "FM18lw" };
            string[] wellingon = { "41*17.0'S", "174*44.7'E", "RE78ir" };
            string someOtherGridsquare = "DM72dx";
            string anotherGridsquare = "DM13ek";

            CoordinateConverter cc = new CoordinateConverter();
            CoordinateConverter.GenerateTableLookups();

            Console.WriteLine($"LatDirection is now {cc.LatDirection}.");
            Console.WriteLine($"LonDirection is now {cc.LonDirection}.");
            if (cc.ValidateDDMinput(""))
            {
                Console.WriteLine("Wow somehow an empty string was accepted by ValidateDDMinput()!");
            }
            if (cc.ValidateGridsquareInput(""))
            {
                Console.WriteLine("Wow somehow and empty string was accepted by ValidateGridsquareInput()!");
            }

            result = cc.ConvertDDMtoGridsquare($"{munich[0].ToString()},{munich[1].ToString()}");
            Console.WriteLine($"Converted DDM {munich[0].ToString()},{munich[1].ToString()} to gridsquare {result} (should be {munich[2].ToString()}).");

           
            result = cc.ConvertDDMtoGridsquare($"{lynnwoodWA[0].ToString()},{lynnwoodWA[1].ToString()}");
            Console.WriteLine($"Converted DDM {lynnwoodWA[0].ToString()},{lynnwoodWA[1].ToString()} to gridsquare {result}.");
            Console.WriteLine();

            result = cc.ConvertDDtoGridsquare(lynnwoodDD);
            Console.WriteLine($"Converted DD {lynnwoodDD} to gridsquare {result}.");
            Console.WriteLine();

            result = cc.ConvertDMStoGridsquare(lynnwoodDMS);
            Console.WriteLine($"Converted DMS {lynnwoodDMS} to gridsquare {result}.");
            Console.WriteLine();

            result = cc.ConvertDDtoDDM(lynnwoodDD);
            Console.WriteLine($"Converted DD {lynnwoodDD} to DDM {result} (should be {lynnwoodWA[0].ToString()},{lynnwoodWA[1].ToString()}).");
            Console.WriteLine();

            result = cc.ConvertDMStoDDM(lynnwoodDMS);
            Console.WriteLine($"Converted DMS {lynnwoodDMS} to DDM {result}.");
            Console.WriteLine();

            result = cc.ConvertGridsquareToDDM($"{lynnwoodWA[2].ToString()}");
            Console.WriteLine($"Converted gridsquare {lynnwoodWA[2].ToString()} to DDM {result} (should be {lynnwoodWA[0].ToString()},{lynnwoodWA[1].ToString()}).");
            Console.WriteLine();

            Console.WriteLine($"LatDirection is now {cc.LatDirection}.");
            Console.WriteLine($"LonDirection is now {cc.LonDirection}.");
            cc.ValidateDDMinput("");
            cc.ValidateGridsquareInput("");
            


            Console.WriteLine("Press <Enter> to exit. . .");
            Console.ReadLine();


        }
    }
}
