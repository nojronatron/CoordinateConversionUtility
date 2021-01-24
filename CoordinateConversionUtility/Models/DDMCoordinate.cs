using System;

namespace CoordinateConversionUtility.Models
{
    public class DDMCoordinate : DDCoordinate
    {
        internal decimal _minutesLattitude;
        internal decimal MinutesLattitude
        {
            get
            {
                return _minutesLattitude;
            }
            set
            {
                if (value >= -60 && value <= 60)
                {
                    _minutesLattitude = value;
                }
            }
        }

        internal decimal _minutesLongitude;
        internal decimal MinutesLongitude
        {
            get
            {
                return _minutesLongitude;
            }
            set
            {
                if (value >= -60 && value <= 60)
                {
                    _minutesLongitude = value;
                }
            }
        }

        public DDMCoordinate() { }
        public DDMCoordinate(decimal ddLat, decimal ddLon)
        {
            DegreesLattitude = ddLat;
            DegreesLongitude = ddLon;
            MinutesLattitude = Math.Abs( (ddLat - Math.Truncate(ddLat)) * 60 );
            MinutesLongitude = Math.Abs( (ddLon - Math.Truncate(ddLon)) * 60 );
        }

        public DDMCoordinate(decimal latDegrees, decimal latMinutes, decimal lonDegrees, decimal lonMinutes)
        {
            DegreesLattitude = latDegrees;
            DegreesLongitude = lonDegrees;
            MinutesLattitude = latMinutes;
            MinutesLongitude = lonMinutes;
        }

        public DDMCoordinate(decimal dmsLatDegrees, decimal dmsLonDegrees, decimal dmsLatMinutes, decimal dmsLonMinutes,
            decimal dmsLatSeconds, decimal dmsLonSeconds)
        {
            DegreesLattitude = dmsLatDegrees;
            DegreesLongitude = dmsLonDegrees;
            MinutesLattitude = dmsLatMinutes + (dmsLatSeconds / 60);
            MinutesLongitude = dmsLonMinutes + (dmsLonSeconds / 60);
        }

        public DDMCoordinate(string ddmLatAndLon)
        {
            if (ddmLatAndLon is null)   //  check for null
            {
                throw new ArgumentNullException(nameof(ddmLatAndLon));
            }

            char[] splitChars = { ',', DegreesSymbol, MinutesSymbol };
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

            int north = ConversionHelper.ExtractPolarityNS($"{ strDdmLatAndLon[2] }");
            int east = ConversionHelper.ExtractPolarityEW($"{ strDdmLatAndLon[5] }");
            DegreesLattitude = tempDegreesLat * north;
            DegreesLongitude = tempDegreesLon * east;
        }

        public decimal GetMinsLat()
        {
            return MinutesLattitude;
        }

        public decimal GetMinsLon()
        {
            return MinutesLongitude;
        }

        public override string ToString()
        {
            return $"{ Math.Abs(GetShortDegreesLat())}{ DegreesSymbol }" +
                   $"{ Math.Round(MinutesLattitude, 2):00.00}{ MinutesSymbol }{ ConversionHelper.GetNSEW(DegreesLattitude, 1) }, " +
                   $"{ Math.Abs(GetShortDegreesLon())}{ DegreesSymbol }" +
                   $"{ Math.Round(MinutesLongitude, 2):00.00}{ MinutesSymbol }{ ConversionHelper.GetNSEW(DegreesLongitude, 2) }";
        }
    }
}
