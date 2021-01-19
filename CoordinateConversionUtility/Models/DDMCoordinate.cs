using System;

namespace CoordinateConversionUtility.Models
{
    public class DDMCoordinate : DDCoordinate
    {
        internal Decimal _minutesLattitude;
        internal Decimal MinutesLattitude
        {
            get
            {
                return _minutesLattitude;
            }
            set
            {
                if (value >= -90 && value <= 90)
                {
                    _minutesLattitude = Math.Round(value, 2);
                }
            }
        }

        internal Decimal _minutesLongitude;
        internal Decimal MinutesLongitude
        {
            get
            {
                return _minutesLongitude;
            }
            set
            {
                if (value >= -180 && value <= 180)
                {
                    _minutesLongitude = Math.Round(value, 2);
                }
            }
        }
        //internal DDCoordinate DDCoordinates { get; private set; }
        //private static char DegreesSymbol => (char)176;     //  degree symbol
        public DDMCoordinate() { }
        public DDMCoordinate(decimal ddLat, decimal ddLon)
        {
            DegreesLattitude = ddLat;
            DegreesLongitude = ddLon;
            //DDCoordinates = new DDCoordinate(ddLat, ddLon);
        }
        public DDMCoordinate(decimal latDegrees, decimal latMinutes, decimal lonDegrees, decimal lonMinutes)
        {
            //  latDegrees and lonDegrees MUST BE SIGNED decimals if negative
            DegreesLattitude = latDegrees;
            DegreesLongitude = lonDegrees;
            MinutesLattitude = latMinutes;
            MinutesLongitude = lonMinutes;
        }
        public DDMCoordinate(string ddmLatAndLon)
        {
            if (ddmLatAndLon is null)   //  check for null
            {
                throw new ArgumentNullException(nameof(ddmLatAndLon));
            }

            char[] splitChars = { ',', (char)176, (char)39 };
            string[] strDdmLatAndLon = ddmLatAndLon.Split(splitChars);

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
                MinutesLattitude = decLatMinutes;// / 60;
            }

            temp = strDdmLatAndLon[3];//.Split((char)176)[0];

            if (decimal.TryParse(temp, out decimal decLonDegrees))
            {
                tempDegreesLon = decLonDegrees;
            }

            temp = strDdmLatAndLon[4];//.Split((char)176)[1];

            if (decimal.TryParse(temp, out decimal decLonMinutes))
            {
                MinutesLongitude = decLonMinutes;// / 120;
            }

            //  Multiply 1/-1 to each degree
            int north = ConversionHelper.ExtractPolarityNS($"{ strDdmLatAndLon[2] }");
            int east = ConversionHelper.ExtractPolarityEW($"{ strDdmLatAndLon[5] }");
            DegreesLattitude = tempDegreesLat * north;
            DegreesLongitude = tempDegreesLon * east;

            ////  Store as a DDCoordinate
            //DDCoordinates = new DDCoordindateHelper(DegreesLat + ( MinutesLat / 60 ), DegreesLon + (MinutesLon / 60 ));
        }

        public override string ToString()
        {
            //  TODO: fix output to include leading zeros in degrees e.g.: 5* => 05*
            //  TODO: fix output to include leading zeros in minutes e.g.: 5.23' => 05.23'
            //  TODO: fix output to include trailing zeros e.g.: 05.2' => 05.20'
            return $"{ Math.Abs(GetLatDegrees())}{ DegreesSymbol }" +
                   $"{ MinutesLattitude:00.00}{ MinutesSymbol }{ ConversionHelper.GetNSEW(DegreesLattitude, 1) }, " +
                   $"{ Math.Abs(GetLonDegrees())}{ DegreesSymbol }" +
                   $"{ MinutesLongitude:00.00}{ MinutesSymbol }{ ConversionHelper.GetNSEW(DegreesLongitude, 2) }";
        }
    }
}
