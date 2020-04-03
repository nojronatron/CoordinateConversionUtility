using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CoordinateConversionUtility
{
    public class DMSCoordinateHelper
    {
        internal decimal DegreesLat { get; private set; }
        internal decimal DegreesLon { get; private set; }
        internal decimal MinutesLat { get; private set; }
        internal decimal MinutesLon { get; private set; }
        internal decimal SecondsLat { get; private set; }
        internal decimal SecondsLon { get; private set; }
        internal decimal DirectionLat { get; private set; }
        internal decimal DirectionLon { get; private set; }
        internal DDCoordindateHelper DDCoordinates { get; private set; }
        private static char DegreesSymbol => (char)176;     //  degree symbol
        private static char MinutesSymbol => (char)39;      //  single quote
        private static char SecondsSymbol => (char)34;      //  double quote
        public DMSCoordinateHelper() { }
        public DMSCoordinateHelper(decimal ddLat, decimal ddLon)
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
        public DMSCoordinateHelper(string dmsLatAndLon)
        {
            if (dmsLatAndLon is null)   //  check for null
            {
                throw new ArgumentNullException(nameof(dmsLatAndLon));
            }
            //  Split string into NS, DegreesLat, MinutesLat, EW, DegreesLon, MinutesLon
            char[] splitChars = { ' ', ',', DegreesSymbol, MinutesSymbol, SecondsSymbol };
            string[] strDdmLatAndLon = dmsLatAndLon.Split(splitChars); // N, 47, 50, 1.20, W, 122, 16, 18.60
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
            DirectionLat = ConversionHelper.ExtractPolarityNSEW($"{ qDdmLatAndLon.Dequeue() }"); //  N
            decimal tempDegreesLat = 0m;
            if (decimal.TryParse(qDdmLatAndLon.Dequeue(), out decimal decLatDegrees))
            {
                tempDegreesLat = decLatDegrees; //  47
            }
            if (decimal.TryParse(qDdmLatAndLon.Dequeue(), out decimal decLatMinutes))
            {
                MinutesLat = decLatMinutes; //  50
            }
            if (decimal.TryParse(qDdmLatAndLon.Dequeue(), out decimal decLatSeconds))
            {
                SecondsLat = decLatSeconds; //  1.20
            }
            DirectionLon = ConversionHelper.ExtractPolarityNSEW($"{ qDdmLatAndLon.Dequeue() }"); //  W
            decimal tempDegreesLon = 0m;
            if (decimal.TryParse(qDdmLatAndLon.Dequeue(), out decimal decLonDegrees))
            {
                tempDegreesLon = decLonDegrees; //  122
            }
            if (decimal.TryParse(qDdmLatAndLon.Dequeue(), out decimal decLonMinutes))
            {
                MinutesLon = decLonMinutes; //  16
            }
            if (decimal.TryParse(qDdmLatAndLon.Dequeue(), out decimal decLonSeconds))
            {
                SecondsLon = decLonSeconds; //  18.60
            }
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
        public decimal GetLatSeconds()
        {
            return SecondsLat;
        }
        public decimal GetLonSeconds()
        {
            return SecondsLon;
        }
        public override string ToString()
        {
            return $"{ ConversionHelper.GetNSEW(DegreesLat, 1) } { Math.Abs(GetLatDegrees()) }{ DegreesSymbol }" +
                   $"{ GetLatMinutes() }{ MinutesSymbol }" +
                   $"{ SecondsLat }{ SecondsSymbol }, " +
                   $"{ ConversionHelper.GetNSEW(DegreesLon, 2) } { Math.Abs(GetLonDegrees()) }{ DegreesSymbol }" +
                   $"{ GetLonMinutes() }{ MinutesSymbol }" +
                   $"{ SecondsLon }{ SecondsSymbol }";
        }
        public static bool IsValid(string DMSLatAndLon, out DMSCoordinateHelper validDMScoords)
        {   //  e.g. CoordinateConverter.IsValid("47.8058,-122.2516")
            //  note: DegreeSymbol, MinutesSymbol, and SecondsSymbol could be included
            if(string.IsNullOrEmpty(DMSLatAndLon) || string.IsNullOrWhiteSpace(DMSLatAndLon))
            {
                //  input nothing return null and out bool false
                validDMScoords = null;
                return false;
            }

            Regex rx = new Regex(@"[NS]"); // [00-90][DegreesSymbol][00-59][MinutesSymbol][0.0-59.59][,] [E,W] [000-180][DegreesSymbol][00-59][MinutesSymbol][0.0-59.59]");
            MatchCollection matches = rx.Matches(DMSLatAndLon);



            StringBuilder sb = new StringBuilder();
            Queue<char> symbols = new Queue<char>();
            symbols.Enqueue(DegreesSymbol);
            symbols.Enqueue(MinutesSymbol);
            symbols.Enqueue(SecondsSymbol);
            symbols.Enqueue(DegreesSymbol);
            symbols.Enqueue(MinutesSymbol);
            symbols.Enqueue(SecondsSymbol);

            string[] splitDMSLatAndLon = DMSLatAndLon.Split(',');

            foreach (string latOrLon in splitDMSLatAndLon)
            {
                foreach (char item in DMSLatAndLon)
                {
                    if (char.IsDigit(item) || item == '.')
                    {
                        sb.Append(item.ToString());
                    }
                    else
                    {
                        sb.Append(symbols.Dequeue());
                    }
                }
            }

            validDMScoords = new DMSCoordinateHelper(sb.ToString());
            return true;
        }
    }
}
