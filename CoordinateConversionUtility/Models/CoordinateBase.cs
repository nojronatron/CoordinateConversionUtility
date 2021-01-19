using System;

namespace CoordinateConversionUtility.Models
{
    public class CoordinateBase
    {
        internal decimal DegreesLattitude { get;  set; }
        internal decimal DegreesLongitude { get;  set; }
        internal static char CommaSymbol => (char)44;    //  comma symbol
        internal static char DegreesSymbol => (char)176; //  degree symbol
        internal static char MinutesSymbol => (char)39;      //  single quote
        internal static char SecondsSymbol => (char)34;      //  double quote

    }
}
