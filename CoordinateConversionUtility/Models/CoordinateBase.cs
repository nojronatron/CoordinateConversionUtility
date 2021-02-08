using System;
using System.Collections.Generic;

namespace CoordinateConversionUtility.Models
{
    public class CoordinateBase : IEquatable<CoordinateBase>
    {
        internal decimal DegreesLattitude { get;  set; }
        internal decimal DegreesLongitude { get;  set; }
        internal static char CommaSymbol => (char)44;    //  comma symbol
        internal static char DegreesSymbol => (char)176; //  degree symbol
        internal static char MinutesSymbol => (char)39;      //  single quote
        internal static char SecondsSymbol => (char)34;      //  double quote
        internal static char SpaceCharacter => (char)32;    //  spacebar
        internal char[] trimChars = { CommaSymbol, DegreesSymbol, MinutesSymbol, SecondsSymbol, SpaceCharacter };

        public override bool Equals(object obj)
        {
            return Equals(obj as CoordinateBase);
        }

        public bool Equals(CoordinateBase other)
        {
            return other != null &&
                   DegreesLattitude == other.DegreesLattitude &&
                   DegreesLongitude == other.DegreesLongitude;
        }

        public override int GetHashCode()
        {
            int hashCode = 1673689655;
            hashCode = hashCode * -1521134295 + DegreesLattitude.GetHashCode();
            hashCode = hashCode * -1521134295 + DegreesLongitude.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(CoordinateBase left, CoordinateBase right)
        {
            return EqualityComparer<CoordinateBase>.Default.Equals(left, right);
        }

        public static bool operator !=(CoordinateBase left, CoordinateBase right)
        {
            return !(left == right);
        }
    }
}
