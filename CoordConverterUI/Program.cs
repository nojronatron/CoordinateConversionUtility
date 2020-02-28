using CoordinateConversionUtility;
using System.Collections.Generic;
using System;

namespace CoordConverterUI
{
    class Program
    {
        public static char DegreesSymbol => (char)176;     //  degree symbol
        public static char MinutesSymbol => (char)39;      //  single quote
        public static char SecondsSymbol => (char)34;      //  double quote

        static void Main(string[] args)
        {

            Console.WriteLine("***** Fun with Coordinate Conversion Utility dll *****");
            Console.WriteLine();

            /*
            Lynnwood WA
            ARRL:	n/a
            DMS:	
            Model DDM:		47*49.21'N, 122*18.02'W
            Corrected DDM to try:	47*50.00'N, 122*17.5'W
            Corrected DD to try:	47.8334, -122.2719
            Corrected DMS to try:	N47*50'00", W122*17'30"
            */
            decimal LynLatDD = 47.8334m;
            decimal LynLonDD = -122.2719m;
            string LynDmsLattitude = DDtoDMS(LynLatDD);
            string LynDmsLongitude = DDtoDMS(LynLonDD);
            Console.WriteLine($"Lynnwood DMS: { LynDmsLattitude }, { LynDmsLongitude }");


            decimal LynLatDegrees = 47m;
            decimal LynLatMinutes = 50m;
            decimal LynLatSeconds = 00m;
            decimal LynDD = DMStoDD(LynLatDegrees, LynLatMinutes, LynLatSeconds);
            Console.WriteLine($"Lynnwood DD Lattitude: { LynDD }{ DegreesSymbol }");

            decimal LynLonDegrees = -122m;
            decimal LynLonMinutes = 17m;
            decimal LynLonSeconds = 30m;
            LynDD = DMStoDD(LynLonDegrees, LynLonMinutes, LynLonSeconds);
            Console.WriteLine($"Lynnwood DD Longitude: { LynDD }{ DegreesSymbol }");
            

            Console.WriteLine("Press <Enter> to exit. . .");
            Console.ReadLine();
        }
        static decimal DDMtoDD(decimal D, decimal M)
        {
            decimal direction = 1;
            if (D < 0)
            {
                direction = -1;
            }
            decimal DD = D;
            DD += ((M / 60) * direction);
            return DD;
        }
        static string DDtoDMS(decimal DD)
        {
            decimal D = Math.Truncate(DD);
            decimal M = Math.Truncate(60 * Math.Abs(DD - D));
            decimal S = 3600 * Math.Abs(DD - D) - 60 * M;
            return $"{ D }{ DegreesSymbol }{ M }{ MinutesSymbol }{ S }{ SecondsSymbol } [N/S]|[E/W]";
        }
        static decimal DMStoDD(decimal D, decimal M, decimal S)
        {
            decimal direction = 1;
            if(D < 0)
            {
                direction = -1;
            }
            decimal DD = D;
            DD += ((M / 60) * direction);
            DD += ((S / 3600) * direction);
            return DD;
        }
    }
}
