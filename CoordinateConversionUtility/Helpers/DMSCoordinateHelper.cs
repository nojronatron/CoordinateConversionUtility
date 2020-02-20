using System;

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
        internal DDCoordindateHelper DDCoordinates { get; private set; }
        private static char DegreesSymbol => (char)176;     //  degree symbol
        private static char MinutesSymbol => (char)39;      //  single quote
        private static char SecondsSymbol => (char)34;      //  double quote
        public DMSCoordinateHelper() { }
        public DMSCoordinateHelper(decimal ddLat, decimal ddLon)
        {
            DegreesLat = ddLat;
            DegreesLon = ddLon;
            DDCoordinates = new DDCoordindateHelper(ddLat, ddLon);
        }
        public DMSCoordinateHelper(string ddmLatAndLon)
        {
            if (ddmLatAndLon is null)   //  check for null
            {
                throw new ArgumentNullException(nameof(ddmLatAndLon));
            }
            //  Split string into NS, DegreesLat, MinutesLat, EW, DegreesLon, MinutesLon
            char[] splitChars = { ',', (char)176, (char)39 };
            string[] strDdmLatAndLon = ddmLatAndLon.Split(splitChars);

            //  TryParse degrees and minutes each into decimal format
            //  Convert Minutes of each to decimal portions of a degree
            string temp = string.Empty;
            temp = strDdmLatAndLon[0].Split((char)176)[0];
            if (decimal.TryParse(temp, out decimal decLatDegrees))
            {
                DegreesLat = decLatDegrees;
            }
            temp = strDdmLatAndLon[0].Split((char)176)[1];
            if (decimal.TryParse(temp, out decimal decLatMinutes))
            {
                MinutesLat = decLatMinutes / 60;
            }
            temp = strDdmLatAndLon[1].Split((char)176)[0];
            if (decimal.TryParse(temp, out decimal decLonDegrees))
            {
                DegreesLon = decLonDegrees;
            }
            temp = strDdmLatAndLon[1].Split((char)176)[1];
            if (decimal.TryParse(temp, out decimal decLonMinutes))
            {
                MinutesLon = decLonMinutes / 120;
            }
            //  Multiply 1/-1 to each degree
            int north = ExtractPolarityNSEW(ddmLatAndLon);
            int east = ExtractPolarityNSEW(ddmLatAndLon);
            DegreesLat *= north;
            DegreesLon *= east;

            //  Store as a DDCoordinate
            DDCoordinates = new DDCoordindateHelper(DegreesLat + MinutesLat, DegreesLon + MinutesLon);
        }
        private static int ExtractPolarityNSEW(string strDdmLatOrLon)
        {
            int nsew = 1;
            if (strDdmLatOrLon.IndexOf('S') > -1)
            {
                nsew = -1;
            }
            else if (strDdmLatOrLon.IndexOf('W') > -1)
            {
                nsew = -1;
            }
            return nsew;
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
        private static string GetNSEW(decimal degreesLatOrLon, int LatOrLon)
        {
            //  lat = 1; lon = 2
            switch (LatOrLon)
            {
                case 1:
                    {
                        string NS = "N";
                        if (degreesLatOrLon < 0)
                        {
                            NS = "S";
                        }
                        return NS;
                    }
                case 2:
                    {
                        string EW = "E";
                        if (degreesLatOrLon < 0)
                        {
                            EW = "W";
                        }
                        return EW;
                    }
                default:
                    return string.Empty;
            }
        }
        public override string ToString()
        {
            return $"{ GetNSEW(DegreesLat, 1) } { Math.Abs(GetLatDegrees()) }{ DegreesSymbol }" +
                   $"{ GetLatMinutes() }{ MinutesSymbol }" +
                   $"{ SecondsLat }{ SecondsSymbol }, " +
                   $"{ GetNSEW(DegreesLon, 2) } { Math.Abs(GetLonDegrees()) }{ DegreesSymbol }" +
                   $"{ GetLonMinutes() }{ MinutesSymbol }" +
                   $"{ SecondsLon }{ SecondsSymbol }";
        }
    }
}
