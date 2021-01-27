using System;

namespace CoordinateConversionUtility.Models
{
    public class DMSCoordinate : DDMCoordinate
    {
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
                    _secondsLattitude = value;
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
                    _secondsLongitude = value;
                }
            }
        }

        public DMSCoordinate() { }
        public DMSCoordinate(decimal ddLat, decimal ddLon)
        {
            DegreesLattitude = ddLat;
            DegreesLongitude = ddLon;
            MinutesLattitude = Math.Abs( (ddLat - Math.Truncate(ddLat)) * 60 );
            MinutesLongitude = Math.Abs( (ddLon - Math.Truncate(ddLon)) * 60 );
            SecondsLattitude = Math.Abs( (MinutesLattitude - Math.Truncate(MinutesLattitude)) * 60 );
            SecondsLongitude = Math.Abs( (MinutesLongitude - Math.Truncate(MinutesLongitude)) * 60 );
        }

        public DMSCoordinate(decimal ddmDegreesLat, decimal ddmMinsLat, decimal ddmDegreesLon, decimal ddmMinsLon)
        {
            DegreesLattitude = Math.Truncate(ddmDegreesLat);
            MinutesLattitude = Math.Truncate(ddmMinsLat);
            SecondsLattitude = (ddmMinsLat - MinutesLattitude) * 60;

            DegreesLongitude = Math.Truncate(ddmDegreesLon);
            MinutesLongitude = Math.Truncate(ddmMinsLon);
            SecondsLongitude = (ddmMinsLon - MinutesLongitude) * 60;
        }

        public DMSCoordinate(
            decimal dmsDegreesLat, decimal dmsMinsLat, decimal dmsSecondsLat,
            decimal dmsDegreesLon, decimal dmsMinsLon, decimal dmsSecondsLon)
        {
            DegreesLattitude = dmsDegreesLat;
            DegreesLongitude = dmsDegreesLon;
            MinutesLattitude = dmsMinsLat;
            MinutesLongitude = dmsMinsLon;
            SecondsLattitude = dmsSecondsLat;
            SecondsLongitude = dmsSecondsLon;
        }

        public DMSCoordinate(string dmsLatAndLon)
        {
            if (string.IsNullOrEmpty(dmsLatAndLon) || string.IsNullOrWhiteSpace(dmsLatAndLon))   //  check for null
            {
                DegreesLattitude = 0.0m;
                DegreesLongitude = 0.0m;
                MinutesLattitude = 0.0m;
                MinutesLongitude = 0.0m;
                SecondsLattitude = 0.0m;
                SecondsLongitude = 0.0m;
                throw new ArgumentNullException(nameof(dmsLatAndLon));
            }

            string[] splitLatAndLon = dmsLatAndLon.Split(CommaSymbol);
            string dmsLat = splitLatAndLon[0];
            string dmsLon = splitLatAndLon[1];

            int degreeIDX = dmsLat.IndexOf(DegreesSymbol);
            int minutesIDX = dmsLat.IndexOf(MinutesSymbol);
            int secondsIDX = dmsLat.IndexOf(SecondsSymbol);

            string temp = dmsLat.Substring(1, degreeIDX).Trim(trimChars).Trim();
            if (decimal.TryParse(temp, out decimal decLatDegrees))
            {
                DegreesLattitude = decLatDegrees;
            }

            temp = dmsLat.Substring(degreeIDX, (minutesIDX - degreeIDX)).Trim(trimChars);
            if (decimal.TryParse(temp, out decimal decLatMinutes))
            {
                MinutesLattitude = decLatMinutes;
            }

            temp = dmsLat.Substring(minutesIDX, (secondsIDX - minutesIDX)).Trim(trimChars);
            if (decimal.TryParse(temp, out decimal decLatSeconds))
            {
                SecondsLattitude = decLatSeconds;
            }

            dmsLon = dmsLon.Trim();
            degreeIDX = dmsLon.IndexOf(DegreesSymbol);
            minutesIDX = dmsLon.IndexOf(MinutesSymbol);
            secondsIDX = dmsLon.IndexOf(SecondsSymbol);

            temp = dmsLon.Substring(1, degreeIDX);
            temp = temp.Trim(trimChars);

            if (decimal.TryParse(temp, out decimal decLonDegrees))
            {
                DegreesLongitude = decLonDegrees;
            }

            temp = dmsLon.Substring(degreeIDX, (minutesIDX - degreeIDX)).Trim(trimChars);
            if (decimal.TryParse(temp, out decimal decLonMinutes))
            {
                MinutesLongitude = decLonMinutes;
            }

            temp = dmsLon.Substring(minutesIDX, (secondsIDX - minutesIDX)).Trim(trimChars);
            if (decimal.TryParse(temp, out decimal decLonSeconds))
            {
                SecondsLongitude = decLonSeconds;
            }

            int north = ConversionHelper.ExtractPolarityNS(dmsLatAndLon);
            int east = ConversionHelper.ExtractPolarityEW(dmsLatAndLon);
            DegreesLattitude *= north;
            DegreesLongitude *= east;
        }

        public decimal GetShortMinutesLattitude()
        {
            return Math.Truncate(MinutesLattitude);
        }

        public decimal GetShortMinutesLongitude()
        {
            return Math.Truncate(MinutesLongitude);
        }

        public decimal GetSecondsLattitude()
        {
            return SecondsLattitude;
        }

        public decimal GetSecondsLongitude()
        {
            return SecondsLongitude;
        }

        public override string ToString()
        {
            return $"{ ConversionHelper.GetNSEW(DegreesLattitude, 1) } { Math.Abs(GetShortDegreesLat()) }{ DegreesSymbol }" +
                   $"{ GetShortMinutesLattitude():00}{ MinutesSymbol }" +
                   $"{ Math.Round(SecondsLattitude, 1):00.0}{ SecondsSymbol }, " +
                   $"{ ConversionHelper.GetNSEW(DegreesLongitude, 2) } { Math.Abs(GetShortDegreesLon()) }{ DegreesSymbol }" +
                   $"{ GetShortMinutesLongitude():00}{ MinutesSymbol }" +
                   $"{ Math.Round(SecondsLongitude, 1):00.0}{ SecondsSymbol }";
        }
    }
}
