using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class RootCoordinateModel
    {
        public static char DegreesSymbol => (char)176;     //  degree symbol
        public static char MinutesSymbol => (char)39;      //  single quote
        public static char SecondsSymbol => (char)34;      //  double quote
        public virtual decimal DegreesLat { get; set; }
        public virtual decimal DegreesLon { get; set; }
        //public decimal MinutesLat { get; set; }
        //public decimal MinutesLon { get; set; }
        //public decimal SecondsLat { get; set; }
        //public decimal SecondsLon { get; set; }
        //public string Gridsquare { get; set; }
        //public string NS { get; set; }
        //public string EW { get; set; }

    }
}
