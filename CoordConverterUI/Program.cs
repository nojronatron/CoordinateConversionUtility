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

            CoordinateConverter cc = new CoordinateConverter();
            CoordinateConverter.GenerateTableLookups();
            string myDDM = "47*48.75'N,122*17.5'W";
            string myDD = "47.8125N,122.2917W";
            string myDMS = "47*48'45\"N,122*17'30\"W";
            string myGridsquare = "CN87ut";

            result = cc.ConvertDDMtoGridsquare(myDDM);
            Console.WriteLine($"Converted DDM {myDDM} to gridsquare {result}.");
            Console.WriteLine();

            result = cc.ConvertDDtoGridsquare(myDD);
            Console.WriteLine($"Converted DD {myDD} to gridsquare {result}.");
            Console.WriteLine();

            result = cc.ConvertDMStoGridsquare(myDMS);
            Console.WriteLine($"Converted DMS {myDMS} to gridsquare {result}.");
            Console.WriteLine();

            result = cc.ConvertDDtoDDM(myDD);
            Console.WriteLine($"Converted DD {myDD} to DDM {result}.");
            Console.WriteLine();

            result = cc.ConvertDMStoDDM(myDMS);
            Console.WriteLine($"Converted DMS {myDMS} to DDM {result}.");
            Console.WriteLine();

            result = cc.ConvertGridsquareToDDM(myGridsquare);
            Console.WriteLine($"Converted gridsquare {myGridsquare} to DDM {result}.");
            Console.WriteLine();

            cc.AddLatDegreesRemainder();
            cc.GetFirstGridsquareCharacter();
            Console.WriteLine($"LatDirection is now {cc.LatDirection}.");
            cc.ValidateDDMinput("");
            cc.ValidateGridsquareInput("");
            


            Console.WriteLine("Press <Enter> to exit. . .");
            Console.ReadLine();


        }
    }
}
