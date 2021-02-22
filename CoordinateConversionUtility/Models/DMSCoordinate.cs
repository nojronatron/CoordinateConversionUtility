﻿using System;
using System.Collections.Generic;

namespace CoordinateConversionUtility.Models
{
    public class DMSCoordinate : DDMCoordinate, IEquatable<DMSCoordinate>
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
                    LatSecsValid = true;
                }
                else
                {
                    _secondsLattitude = 0.0m;
                    LatSecsValid = false;
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
                    LonSecsValid = true;
                }
                else
                {
                    _secondsLongitude = 0.0m;
                    LonSecsValid = false;
                }
            }
        }
        internal bool LatSecsValid { get; set; }
        internal bool LonSecsValid { get; set; }
        new public bool IsValid => (LatIsValid && LonIsValid && LatMinsValid && LonMinsValid && LatSecsValid && LonSecsValid);

        public DMSCoordinate() 
        {
            DegreesLattitude = 0.0m;
            DegreesLongitude = 0.0m;
            MinutesLattitude = 0.0m;
            MinutesLongitude = 0.0m;
            SecondsLattitude = 0.0m;
            SecondsLongitude = 0.0m;
            LatIsValid = false;
            LonIsValid = false;
            LatMinsValid = false;
            LonMinsValid = false;
            LatSecsValid = false;
            LonSecsValid = false;
        }
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
                LatIsValid = false;
                LonIsValid = false;
                LatMinsValid = false;
                LonMinsValid = false;
                LatSecsValid = false;
                LonSecsValid = false;
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
            else
            {
                LatIsValid = false;
            }

            temp = dmsLat.Substring(degreeIDX, (minutesIDX - degreeIDX)).Trim(trimChars);
            if (decimal.TryParse(temp, out decimal decLatMinutes))
            {
                MinutesLattitude = decLatMinutes;
            }
            else
            {
                LatMinsValid = false;
            }

            temp = dmsLat.Substring(minutesIDX, (secondsIDX - minutesIDX)).Trim(trimChars);
            if (decimal.TryParse(temp, out decimal decLatSeconds))
            {
                SecondsLattitude = decLatSeconds;
            }
            else
            {
                LatSecsValid = false;
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
            else
            {
                LatIsValid = false;
            }

            temp = dmsLon.Substring(degreeIDX, (minutesIDX - degreeIDX)).Trim(trimChars);
            if (decimal.TryParse(temp, out decimal decLonMinutes))
            {
                MinutesLongitude = decLonMinutes;
            }
            else
            {
                LonMinsValid = false;
            }

            temp = dmsLon.Substring(minutesIDX, (secondsIDX - minutesIDX)).Trim(trimChars);
            if (decimal.TryParse(temp, out decimal decLonSeconds))
            {
                SecondsLongitude = decLonSeconds;
            }
            else
            {
                LonSecsValid = false;
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

        public override bool Equals(object obj)
        {
            return Equals(obj as DMSCoordinate);
        }

        public bool Equals(DMSCoordinate other)
        {
            return other != null &&
                   base.Equals(other) &&
                   DegreesLattitude == other.DegreesLattitude &&
                   DegreesLongitude == other.DegreesLongitude &&
                   MinutesLattitude == other.MinutesLattitude &&
                   MinutesLongitude == other.MinutesLongitude &&
                   SecondsLattitude == other.SecondsLattitude &&
                   SecondsLongitude == other.SecondsLongitude;
        }

        public override int GetHashCode()
        {
            int hashCode = 2097953291;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + DegreesLattitude.GetHashCode();
            hashCode = hashCode * -1521134295 + DegreesLongitude.GetHashCode();
            hashCode = hashCode * -1521134295 + MinutesLattitude.GetHashCode();
            hashCode = hashCode * -1521134295 + MinutesLongitude.GetHashCode();
            hashCode = hashCode * -1521134295 + SecondsLattitude.GetHashCode();
            hashCode = hashCode * -1521134295 + SecondsLongitude.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(DMSCoordinate left, DMSCoordinate right)
        {
            return EqualityComparer<DMSCoordinate>.Default.Equals(left, right);
        }

        public static bool operator !=(DMSCoordinate left, DMSCoordinate right)
        {
            return !(left == right);
        }
    }
}
