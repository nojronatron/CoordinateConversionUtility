using System;
using System.Collections.Generic;
using System.Text;

namespace CoordinateConversionUtility
{
    public class DDMCoordinateHelper
    {
        internal decimal DegreesLat { get; private set; }
        internal decimal DegreesLon { get; private set; }
        internal decimal MinutesLat { get; private set; }
        internal decimal MinutesLon { get; private set; }
        internal decimal DirectionLat { get; private set; }
        internal decimal DirectionLon { get; private set; }
        internal DDCoordindateHelper DDCoordinates { get; private set; }
        private static char DegreesSymbol => (char)176;     //  degree symbol
        private static char MinutesSymbol => (char)39;      //  single quote
        public DDMCoordinateHelper() { }
        public DDMCoordinateHelper(decimal ddLat, decimal ddLon)
        {
            DegreesLat = ddLat;
            if (ddLat < 0)
            {
                DirectionLat = -1;
            }
            else
            {
                DirectionLat = 1;
            }
            DegreesLon = ddLon;
            if (ddLon < 0)
            {
                DirectionLon = -1;
            }
            else DirectionLon = 1;
            DDCoordinates = new DDCoordindateHelper(ddLat, ddLon);
        }
        public DDMCoordinateHelper(decimal latDegrees, decimal latMinutes, decimal lonDegrees, decimal lonMinutes)
        {
            //  latDegrees and lonDegrees MUST BE SIGNED decimals if negative
            DegreesLat = latDegrees;
            if (DegreesLat < 0)
            {
                DirectionLat = -1;
            }
            else
            {
                DirectionLat = 1;
            }
            DegreesLon = lonDegrees;
            if (DegreesLon < 0)
            {
                DirectionLon = -1;
            }
            else
            {
                DirectionLon = 1;
            }
            MinutesLat = latMinutes;
            MinutesLon = lonMinutes;
        }
        public DDMCoordinateHelper(string ddmLatAndLon)
        {
            if (ddmLatAndLon is null)   //  check for null
            {
                throw new ArgumentNullException(nameof(ddmLatAndLon));
            }
            //  Split string into NS, DegreesLat, MinutesLat, EW, DegreesLon, MinutesLon
            char[] splitChars = { ' ', ',', DegreesSymbol, MinutesSymbol };
            string[] strDdmLatAndLon = ddmLatAndLon.Split(splitChars); // 47, 50.02, N, 122, 16.31, W
            Queue<string> qDdmLatAndLon = new Queue<string>();
            foreach (string item in strDdmLatAndLon)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    qDdmLatAndLon.Enqueue(item);
                }
            }
            //  TryParse degrees and minutes each into decimal format
            //  Convert Minutes of each to decimal portions of a degree
            decimal tempDegreesLat = 0m;
            if (decimal.TryParse(qDdmLatAndLon.Dequeue(), out decimal decLatDegrees))
            {
                tempDegreesLat = decLatDegrees; //  47
            }
            if (decimal.TryParse(qDdmLatAndLon.Dequeue(), out decimal decLatMinutes))
            {
                MinutesLat = decLatMinutes; //  50.02
            }
            DirectionLat = ConversionHelper.ExtractPolarityNSEW($"{ qDdmLatAndLon.Dequeue() }"); //  N
            decimal tempDegreesLon = 0m;
            if (decimal.TryParse(qDdmLatAndLon.Dequeue(), out decimal decLonDegrees))
            {
                tempDegreesLon = decLonDegrees; //  122
            }
            if (decimal.TryParse(qDdmLatAndLon.Dequeue(), out decimal decLonMinutes))
            {
                MinutesLon = decLonMinutes; //  16.31
            }
            DirectionLon = ConversionHelper.ExtractPolarityNSEW($"{ qDdmLatAndLon.Dequeue() }"); //  W
            //  Multiply 1/-1 to each degree
            DegreesLat = tempDegreesLat * DirectionLat; //  finalize Lattitude Degrees as whole signed decimal number
            DegreesLon = tempDegreesLon * DirectionLon; //  finalize Longitude Degrees as whole signed decimal number
        }

        public decimal GetLatDegrees()
        {
            return Math.Truncate(DegreesLat);
        }
        public decimal GetLonDegrees()
        {
            return Math.Truncate(DegreesLon);
        }
        public decimal GetLatMinutes()
        {
            return MinutesLat;
        }
        public decimal GetLonMinutes()
        {
            return MinutesLon;
        }

        public int GetLatDirection()
        {
            if (this.DegreesLat >= 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        public int GetLonDirection()
        {
            if (this.DegreesLon >= 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        public override string ToString()
        {
            //  TODO: fix output to include leading zeros in degrees e.g.: 5* => 05*
            //  TODO: fix output to include leading zeros in minutes e.g.: 5.23' => 05.23'
            //  TODO: fix output to include trailing zeros e.g.: 05.2' => 05.20'
            return $"{ Math.Abs(GetLatDegrees())}{ DegreesSymbol }" +
                   $"{ MinutesLat:00.00}{ MinutesSymbol }{ ConversionHelper.GetNSEW(DegreesLat, 1) }, " +
                   $"{ Math.Abs(GetLonDegrees())}{ DegreesSymbol }" +
                   $"{ MinutesLon:00.00}{ MinutesSymbol }{ ConversionHelper.GetNSEW(DegreesLon, 2) }";
        }
        public static bool IsValid(string DDMLatAndLon, out DDMCoordinateHelper validDdmCoordinates)
        {   //  e.g. CoordinateConverter.IsValid("47.8058,-122.2516")
            //  note: DegreeSymbol and MinutesSymbol could be included
            validDdmCoordinates = null;
            return false;
        }
    }
}
