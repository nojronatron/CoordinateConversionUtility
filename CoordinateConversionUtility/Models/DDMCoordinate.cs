﻿using System;
using System.Collections.Generic;

namespace CoordinateConversionUtility.Models
{
    public class DDMCoordinate : DDCoordinate, IEquatable<DDMCoordinate>
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
                if (ValidateLatMinutes(value))
                {
                    _minutesLattitude = value;
                    LatMinsValid = true;
                }
                else
                {
                    _minutesLattitude = 0.0m;
                    LatMinsValid = false;
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
                if (ValidateLonMinutes(value))
                {
                    _minutesLongitude = value;
                    LonMinsValid = true;
                }
                else
                {
                    _minutesLongitude = 0.0m;
                    LonMinsValid = false;
                }
            }
        }
        internal static bool ValidateLatMinutes(decimal minutesLattitude)
        {
            return (minutesLattitude >= -60 && minutesLattitude <= 60);
        }
        internal static bool ValidateLonMinutes(decimal minutesLongitude)
        {
            return (minutesLongitude >= -60 && minutesLongitude <= 60);
        }
        internal virtual bool LatMinsValid { get; set; }
        internal virtual bool LonMinsValid { get; set; }
        new public bool IsValid => (LatIsValid && LonIsValid && LatMinsValid && LonMinsValid);

        public DDMCoordinate()
        {
            LatIsValid = false;
            LonIsValid = false;
            LatMinsValid = false;
            LonMinsValid = false;
        }
        public DDMCoordinate(decimal ddLat, decimal ddLon)
        {
            DegreesLattitude = ddLat;
            DegreesLongitude = ddLon;
            MinutesLattitude = Math.Abs((ddLat - Math.Truncate(ddLat)) * 60);
            MinutesLongitude = Math.Abs((ddLon - Math.Truncate(ddLon)) * 60);
        }

        public DDMCoordinate(decimal latDegrees, decimal latMinutes, decimal lonDegrees, decimal lonMinutes)
        {
            DegreesLattitude = latDegrees;
            DegreesLongitude = lonDegrees;
            MinutesLattitude = latMinutes;
            MinutesLongitude = lonMinutes;
        }

        public DDMCoordinate(decimal dmsLatDegrees, decimal dmsLatMinutes, decimal dmsLatSeconds,
            decimal dmsLonDegrees, decimal dmsLonMinutes, decimal dmsLonSeconds)
        {
            DegreesLattitude = dmsLatDegrees;
            DegreesLongitude = dmsLonDegrees;
            MinutesLattitude = dmsLatMinutes + (dmsLatSeconds / 60);
            MinutesLongitude = dmsLonMinutes + (dmsLonSeconds / 60);
        }

        public DDMCoordinate(string ddmLatAndLon)
        {
            if (ddmLatAndLon is null || string.IsNullOrEmpty(ddmLatAndLon) || string.IsNullOrWhiteSpace(ddmLatAndLon))   //  check for null
            {
                DegreesLattitude = 0.0m;
                DegreesLongitude = 0.0m;
                LatIsValid = false;
                LonIsValid = false;
            }
            else
            {
                char[] splitChars = { CommaSymbol, DegreesSymbol, MinutesSymbol };
                string[] strDdmLatAndLon = ddmLatAndLon.Split(splitChars);

                string tempParseParameter = strDdmLatAndLon[0];
                decimal tempDegreesLat = 0m;
                decimal tempDegreesLon = 0m;

                if (decimal.TryParse(tempParseParameter, out decimal decLatDegrees))
                {
                    tempDegreesLat = decLatDegrees;
                }

                tempParseParameter = strDdmLatAndLon[1];
                if (decimal.TryParse(tempParseParameter, out decimal decLatMinutes))
                {
                    MinutesLattitude = decLatMinutes;
                }

                tempParseParameter = strDdmLatAndLon[3];

                if (decimal.TryParse(tempParseParameter, out decimal decLonDegrees))
                {
                    tempDegreesLon = decLonDegrees;
                }

                tempParseParameter = strDdmLatAndLon[4];

                if (decimal.TryParse(tempParseParameter, out decimal decLonMinutes))
                {
                    MinutesLongitude = decLonMinutes;
                }

                int north = ConversionHelper.ExtractPolarityNS($"{ strDdmLatAndLon[2] }");
                int east = ConversionHelper.ExtractPolarityEW($"{ strDdmLatAndLon[5] }");
                DegreesLattitude = tempDegreesLat * north;
                DegreesLongitude = tempDegreesLon * east;
            }
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

        public override bool Equals(object obj)
        {
            return Equals(obj as DDMCoordinate);
        }

        public bool Equals(DDMCoordinate other)
        {
            return other != null &&
                   base.Equals(other) &&
                   DegreesLattitude == other.DegreesLattitude &&
                   DegreesLongitude == other.DegreesLongitude &&
                   MinutesLattitude == other.MinutesLattitude &&
                   MinutesLongitude == other.MinutesLongitude;
        }

        public override int GetHashCode()
        {
            int hashCode = -248276796;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + DegreesLattitude.GetHashCode();
            hashCode = hashCode * -1521134295 + DegreesLongitude.GetHashCode();
            hashCode = hashCode * -1521134295 + MinutesLattitude.GetHashCode();
            hashCode = hashCode * -1521134295 + MinutesLongitude.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(DDMCoordinate left, DDMCoordinate right)
        {
            return EqualityComparer<DDMCoordinate>.Default.Equals(left, right);
        }

        public static bool operator !=(DDMCoordinate left, DDMCoordinate right)
        {
            return !(left == right);
        }
    }
}
