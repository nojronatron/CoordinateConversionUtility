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
                if (CoordinateBase.ValidateLatDegrees(value))
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
                if (CoordinateBase.ValidateLonDegrees(value))
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
        
        internal static bool ValidateLatDegrees(decimal degreesLattitude)
        {
            return (-90m <= degreesLattitude && degreesLattitude <= 90m);
        }
        
        internal static bool ValidateLonDegrees(decimal degreesLongitude)
        {
            return (-180m <= degreesLongitude && degreesLongitude <= 180m);
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

        /// <summary>
        /// Take a string and convert it to a decimal then return the result from asking CoordinateBase if it is valid or not.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="validLatDegrees"></param>
        /// <returns></returns>
        public static bool ValidateIsLatDegrees(string number, out decimal validLatDegrees)
        {
            validLatDegrees = 0.0m;

            if (decimal.TryParse(number, out decimal lattitude))
            {
                if (CoordinateBase.ValidateLatDegrees(lattitude))
                {
                    validLatDegrees = lattitude;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Take a string and convert it to a decimal then return the result from asking CoordinateBase if it is valid or not.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="validLonDegrees"></param>
        /// <returns></returns>
        public static bool ValidateIsLonDegrees(string number, out decimal validLonDegrees)
        {
            validLonDegrees = 0.0m;

            if (decimal.TryParse(number, out decimal longitude))
            {
                if (CoordinateBase.ValidateLonDegrees(longitude))
                {
                    validLonDegrees = longitude;
                    return true;
                }
            }

            return false;
        }

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
