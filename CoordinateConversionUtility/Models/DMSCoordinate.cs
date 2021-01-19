using System;

namespace CoordinateConversionUtility.Models
{
    public class DMSCoordinate : DDMCoordinate
    {
        new internal Decimal MinutesLattitude
        {
            get
            {
                return base.MinutesLattitude;
            }
            set
            {
                base.MinutesLattitude = Math.Truncate(value);
            }
        }

        new internal Decimal MinutesLongitude
        {
            get
            {
                return base.MinutesLongitude;
            }
            set
            {
                if (value >= 0 && value <= 60)
                {
                    base.MinutesLongitude = Math.Truncate(value);
                }
            }
        }

        internal decimal _secondsLattitude;
        internal decimal SecondsLattitude
        {
            get
            {
                return _secondsLattitude;
            }
            private set
            {
                if (value >= 0 && value <= 60)
                {
                    _secondsLattitude = Math.Round(value, 2);
                }
            }
        }

        internal decimal _secondsLongitude;
        internal decimal SecondsLongitude
        {
            get
            {
                return _secondsLongitude;
            }
            private set
            {
                if (value >= 0 && value <= 60)
                {
                    _secondsLongitude = Math.Round(value, 2);
                }
            }
        }

        //internal DDCoordinate DDCoordinates { get; private set; }
        public DMSCoordinate() { }
        public DMSCoordinate(decimal ddLat, decimal ddLon)
        {
            DegreesLattitude = ddLat;
            DegreesLongitude = ddLon;
            //DDCoordinates = new DDCoordinate(ddLat, ddLon);
        }

        public DMSCoordinate(string dmsLatAndLon)
        {
            if (dmsLatAndLon is null)   //  check for null
            {
                throw new ArgumentNullException(nameof(dmsLatAndLon));
            }
            //  Split string into NS, DegreesLat, MinutesLat, EW, DegreesLon, MinutesLon
            char[] splitChars = { ',', (char)176, (char)39 };
            string[] strDdmLatAndLon = dmsLatAndLon.Split(splitChars);

            //  TryParse degrees and minutes each into decimal format
            //  Convert Minutes of each to decimal portions of a degree
            string temp = string.Empty;
            temp = strDdmLatAndLon[0].Split((char)176)[0];
            if (decimal.TryParse(temp, out decimal decLatDegrees))
            {
                DegreesLattitude = decLatDegrees;
            }
            temp = strDdmLatAndLon[0].Split((char)176)[1];
            if (decimal.TryParse(temp, out decimal decLatMinutes))
            {
                MinutesLattitude = decLatMinutes / 60;
            }
            temp = strDdmLatAndLon[1].Split((char)176)[0];
            if (decimal.TryParse(temp, out decimal decLonDegrees))
            {
                DegreesLongitude = decLonDegrees;
            }
            temp = strDdmLatAndLon[1].Split((char)176)[1];
            if (decimal.TryParse(temp, out decimal decLonMinutes))
            {
                MinutesLongitude = decLonMinutes / 120;
            }
            //  Multiply 1/-1 to each degree
            int north = ConversionHelper.ExtractPolarityNS(dmsLatAndLon);
            int east = ConversionHelper.ExtractPolarityEW(dmsLatAndLon);
            DegreesLattitude *= north;
            DegreesLongitude *= east;

            ////  Store as a DDCoordinate
            //DDCoordinates = new DDCoordinate(DegreesLattitude + MinutesLattitude, DegreesLongitude + MinutesLongitude);
        }

        public override string ToString()
        {
            return $"{ ConversionHelper.GetNSEW(DegreesLattitude, 1) } { Math.Abs(GetLatDegrees()) }{ DegreesSymbol }" +
                   $"{ MinutesLattitude:00}{ MinutesSymbol }" +
                   $"{ SecondsLattitude:00.0}{ SecondsSymbol }, " +
                   $"{ ConversionHelper.GetNSEW(DegreesLongitude, 2) } { Math.Abs(GetLonDegrees()) }{ DegreesSymbol }" +
                   $"{ MinutesLongitude:00}{ MinutesSymbol }" +
                   $"{ SecondsLongitude:00.0}{ SecondsSymbol }";
        }
    }
}
