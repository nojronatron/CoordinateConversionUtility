using System;
using System.Text;

namespace CoordinateConversionUtility
{
    public class DDMCoordinateHelper
    {
        internal decimal DegreesLat { get; private set; }
        internal decimal DegreesLon { get; private set; }
        internal decimal MinutesLat { get; private set; }
        internal decimal MinutesLon { get; private set; }
        internal DDCoordindateHelper DDCoordinates { get; private set; }
        private static char DegreesSymbol => (char)176;     //  degree symbol
        private static char MinutesSymbol => (char)39;      //  single quote
        public DDMCoordinateHelper() { }
        public DDMCoordinateHelper(decimal ddLat, decimal ddLon)
        {
            DegreesLat = ddLat;
            DegreesLon = ddLon;
            DDCoordinates = new DDCoordindateHelper(ddLat, ddLon);
        }
        public DDMCoordinateHelper(decimal latDegrees, decimal latMinutes, decimal lonDegrees, decimal lonMinutes)
        {
            //  latDegrees and lonDegrees MUST BE SIGNED decimals if negative
            DegreesLat = latDegrees;
            DegreesLon = lonDegrees;
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
            char[] splitChars = { ',', (char)176, (char)39 };
            string[] strDdmLatAndLon = ddmLatAndLon.Split(splitChars);

            //  TryParse degrees and minutes each into decimal format
            //  Convert Minutes of each to decimal portions of a degree
            string temp = string.Empty;
            decimal tempDegreesLat = 0m;
            decimal tempDegreesLon = 0m;
            temp = strDdmLatAndLon[0];//.Split((char)176)[0];
            if (decimal.TryParse(temp, out decimal decLatDegrees))
            {
                tempDegreesLat = decLatDegrees;
            }
            temp = strDdmLatAndLon[1];//.Split((char)176)[1];
            if (decimal.TryParse(temp, out decimal decLatMinutes))
            {
                MinutesLat = decLatMinutes;// / 60;
            }
            temp = strDdmLatAndLon[3];//.Split((char)176)[0];
            if (decimal.TryParse(temp, out decimal decLonDegrees))
            {
                tempDegreesLon = decLonDegrees;
            }
            temp = strDdmLatAndLon[4];//.Split((char)176)[1];
            if (decimal.TryParse(temp, out decimal decLonMinutes))
            {
                MinutesLon = decLonMinutes;// / 120;
            }
            //  Multiply 1/-1 to each degree
            int north = ExtractPolarityNSEW($"{ strDdmLatAndLon[2] }");
            int east = ExtractPolarityNSEW($"{ strDdmLatAndLon[5] }");
            DegreesLat = tempDegreesLat * north;
            DegreesLon = tempDegreesLon * east;

            ////  Store as a DDCoordinate
            //DDCoordinates = new DDCoordindateHelper(DegreesLat + ( MinutesLat / 60 ), DegreesLon + (MinutesLon / 60 ));
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
        private static string GetNSEW(string strDdmLatOrLon)
        {
            StringBuilder nsew = new StringBuilder(2);
            if (strDdmLatOrLon.IndexOf('N') > -1)
            {
                nsew.Append("N");
            }
            else
            {
                nsew.Append("S");
            }
            if (strDdmLatOrLon.IndexOf('E') > -1)
            {
                nsew.Append("E");
            }
            else
            {
                nsew.Append("W");
            }
            return nsew.ToString();
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
                   $"{ MinutesLat:00.00}{ MinutesSymbol }{ GetNSEW(DegreesLat, 1) }, " +
                   $"{ Math.Abs(GetLonDegrees())}{ DegreesSymbol }" +
                   $"{ MinutesLon:00.00}{ MinutesSymbol }{ GetNSEW(DegreesLon, 2) }";
        }
    }
}
