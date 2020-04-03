using CoordinateConversionUtility;
using CoordinateConversionUtility.Helpers;
using System.Collections.Generic;
using System;

namespace CoordConverterUI
{
    class CoordConverter
    {
        public static char DegreesSymbol => (char)176;     //  degree symbol
        public static char MinutesSymbol => (char)39;      //  single quote
        public static char SecondsSymbol => (char)34;      //  double quote

        static void Main(string[] args)
        {
            //  take STRING input(s) and attempt to convert to and display all results
            //      ex: input: CN87ut; output: DD, DDM, and DMS coordinates
            //      ex: input: DD coordinates; output: Gridsquare, DDM, and DMS coordinates
            //  take this one step at a time

            Console.Clear();
            Console.WriteLine("***** Coordinate Conversion Utility *****\n");


            //  args to use for debugging
            string dmsCoordinateInput = $"N 47{ DegreesSymbol }48{ MinutesSymbol }25.5{ SecondsSymbol }," +
                                        $"W 122{ DegreesSymbol }15{ MinutesSymbol }15.25{ SecondsSymbol }";

            //if (CoordinateConversionUtility.Helpers.GridSquareHelper.ValidateGridsquareInput(args[0], out System.Text.StringBuilder validGrid))
            //{
            //    //  convert validGrid into DDM via CoordianteConversionUtil.cs
            //    //  convert DDM to DD
            //    //  convert DDM to DMS
            //    //  display results (use native ToString() overrides)

            //}
            //else if (DDCoordindateHelper.IsValid("0m, 0m", out decimal validDdLat, out decimal validDdLon)) //out DDCoordindateHelper validDdCoordinates))
            //{
            //    //  convert validDD to DDM via DDMCoordianteHelper.cs
            //    //  convert DDM to DMS via DMSCoordinateHelper.cs
            //    //  convert DDM to gridsquare using CoordinateConversionUtil.cs
            //    //  display results (use native ToString() overrides)

            //}
            //else if (DDMCoordinateHelper.IsValid("", out DDMCoordinateHelper validDdmCoordinates))
            //{
            //    //  create ISVALID() method in DDMCoordinateHelper.cs
            //    //  converd DDM to DMS via DMSCoordinateHelper.cs
            //    //  convert DDM to Gridsquare using CoordinateConversionutil.cs
            //    //  convert DDM to DD via ??
            //    //  display results (use native ToString() overrides)

            //}
            //else 
            if (DMSCoordinateHelper.IsValid(dmsCoordinateInput, out DMSCoordinateHelper validDmsCoordinates))
            {
                //  create ISVALID() method in DMSCoordinateHelper.cs
                //  convert DMS to DD via ??
                //  convert DD to DDM via ??
                //  convert DDM to Gridsquare using CoordinateConversationUtil.cs
                Console.WriteLine($"DMSCoordinateHelper.isValid() result: { validDmsCoordinates.ToString() }");
            }



            Console.WriteLine("Press <Enter> to exit. . .");
            Console.ReadLine();
        }
    }
}
