using System;
using System.Collections.Generic;

namespace CoordinateConversionUtility.Models
{
    /// <summary>
    /// Decimal Degrees coordinate class
    /// </summary>
    public class DDCoordinate : CoordinateBase, IEquatable<DDCoordinate>
    {
        public override bool IsValid => base.IsValid;
        public DDCoordinate()
        {
            LatIsValid = false;
            LonIsValid = false;
        }

        public DDCoordinate(decimal lattitude, decimal longitude)
        {
            DegreesLattitude = lattitude;
            DegreesLongitude = longitude;
        }

        public DDCoordinate(decimal ddmLatDegrees, decimal ddmLatMins, decimal ddmLonDegrees, decimal ddmLonMins)
        {
            if (ddmLatDegrees < 0)
            {
                ddmLatMins *= -1;
            }

            if (ddmLonDegrees < 0)
            {
                ddmLonMins *= -1;
            }

            DegreesLattitude = Math.Truncate(ddmLatDegrees) + (ddmLatMins / 60);
            DegreesLongitude = Math.Truncate(ddmLonDegrees) + (ddmLonMins / 60);
        }

        public DDCoordinate(decimal dmsLatDegrees, decimal dmsLatMinutes, decimal dmsLatSeconds,
            decimal dmsLonDegrees, decimal dmsLonMinutes, decimal dmsLonSeconds)
        {
            dmsLatMinutes = Math.Truncate(dmsLatMinutes);
            dmsLonMinutes = Math.Truncate(dmsLonMinutes);

            if (dmsLatDegrees < 0)
            {
                dmsLatMinutes *= -1;
                dmsLatSeconds *= -1;
            }

            if (dmsLonDegrees < 0)
            {
                dmsLonMinutes *= -1;
                dmsLonSeconds *= -1;
            }

            DegreesLattitude = dmsLatDegrees + (dmsLatMinutes / 60) + (dmsLatSeconds / 3600);
            DegreesLongitude = dmsLonDegrees + (dmsLonMinutes / 60) + (dmsLonSeconds / 3600);
        }

        public DDCoordinate(string ddLatAndLon)
        {
            if (string.IsNullOrEmpty(ddLatAndLon) || string.IsNullOrWhiteSpace(ddLatAndLon))   //  check for null
            {
                DegreesLattitude = 0.0m;
                LonIsValid = false;
                DegreesLongitude = 0.0m;
                LatIsValid = false;
            }

            string[] splitLatAndLon = ddLatAndLon.Split(CommaSymbol);
            string ddLat = splitLatAndLon[0];
            string ddLon = splitLatAndLon[1];

            ddLat = ddLat.Trim();
            int degreeIDX = ddLat.IndexOf(DegreesSymbol);

            string tempParseParameter = ddLat.Substring(0, degreeIDX).Trim(trimChars).Trim();

            if (decimal.TryParse(tempParseParameter, out decimal decLatDegrees))
            {
                DegreesLattitude = decLatDegrees;
            }
            else
            {
                DegreesLattitude = 0.0m;
                LonIsValid = false;
            }

            degreeIDX = ddLon.IndexOf(DegreesSymbol);
            tempParseParameter = ddLon.Substring(0, degreeIDX);
            tempParseParameter = tempParseParameter.Trim(trimChars);

            if (decimal.TryParse(tempParseParameter, out decimal decLonDegrees))
            {
                DegreesLongitude = decLonDegrees;
            }
            else
            {
                DegreesLongitude = 0.0m;
                LatIsValid = false;
            }
        }

        public decimal GetLattitudeDD()
        {
            return DegreesLattitude;
        }

        public decimal GetLongitudeDD()
        {
            return DegreesLongitude;
        }

        public decimal GetShortDegreesLat()
        {
            return Math.Truncate(this.DegreesLattitude);
        }

        public decimal GetShortDegreesLon()
        {
            return Math.Truncate(this.DegreesLongitude);
        }

        public decimal GetFractionalLattitude()
        {
            return DegreesLattitude - Math.Truncate(DegreesLattitude);
        }

        public decimal GetFractionalLongitude()
        {
            return DegreesLongitude - Math.Truncate(DegreesLongitude);
        }

        public override string ToString()
        {
            return $"{ DegreesLattitude:f5}{ DegreesSymbol }, { DegreesLongitude:f5}{ DegreesSymbol }";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DDCoordinate);
        }

        public bool Equals(DDCoordinate other)
        {
            return other != null &&
                   base.Equals(other) &&
                   DegreesLattitude == other.DegreesLattitude &&
                   DegreesLongitude == other.DegreesLongitude;
        }

        public override int GetHashCode()
        {
            int hashCode = 2104951357;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + DegreesLattitude.GetHashCode();
            hashCode = hashCode * -1521134295 + DegreesLongitude.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(DDCoordinate left, DDCoordinate right)
        {
            return EqualityComparer<DDCoordinate>.Default.Equals(left, right);
        }

        public static bool operator !=(DDCoordinate left, DDCoordinate right)
        {
            return !(left == right);
        }
    }
}
