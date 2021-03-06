using System;
using System.Collections.Generic;

namespace CoordinateConversionUtility.Models
{
    public class CoordinateBase : IEquatable<CoordinateBase>
    {
        internal decimal _degreesLattitude;
        internal decimal _degreesLongitude;
        internal bool LatIsValid { get; set; }
        internal bool LonIsValid { get; set; }
        internal decimal DegreesLattitude
        {
            get
            {
                return _degreesLattitude;
            }
            set
            {
                if (-90m <= value && value <= 90m)
                {
                    _degreesLattitude = value;
                    LatIsValid = true;
                }
                else
                {
                    DegreesLattitude = 0.0m;
                    LatIsValid = false;
                }
            }
        }
        internal decimal DegreesLongitude
        {
            get
            {
                return _degreesLongitude;
            }
            set
            {
                if (-180m <= value && value <= 180m)
                {
                    _degreesLongitude = value;
                    LonIsValid = true;
                }
                else
                {
                    DegreesLongitude = 0.0m;
                    LonIsValid = false;
                }
            }
        }
        internal static char CommaSymbol => (char)44;
        internal static char MinusSymbol => (char)45;
        internal static char DegreesSymbol => (char)176;
        internal static char MinutesSymbol => (char)39;
        internal static char SecondsSymbol => (char)34;
        internal static char SpaceCharacter => (char)32;
        internal char[] trimChars = { CommaSymbol, DegreesSymbol, MinutesSymbol, SecondsSymbol, SpaceCharacter };
        /// <summary>
        /// Constructors set this bit true if successful, false if failures or ambiguity occurs during initialization.
        /// </summary>
        public virtual bool IsValid => LatIsValid && LonIsValid;

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
