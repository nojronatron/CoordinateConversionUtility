using System;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class RootCoordinateModel
    {
        public static char DegreesSymbol => (char)176;     //  degree symbol
        public static char MinutesSymbol => (char)39;      //  single quote
        public static char SecondsSymbol => (char)34;      //  double quote
        public virtual decimal DegreesLat { get; set; }
        public virtual decimal DegreesLon { get; set; }

    }
}
