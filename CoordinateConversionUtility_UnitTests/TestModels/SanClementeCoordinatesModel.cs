using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class SanClementeCoordinatesModel : SymbolHelper
    {
        public string ArrlDDM()
        {
            return $"33{ DegreesSymbol }26.25{ MinutesSymbol }N, 117{ DegreesSymbol }37.50{ MinutesSymbol}W";
        }   //  	    ARRL DDM:		    33*26.25'N, 117*37.5'W
        public string ArrlGridsquare()
        {
            return "DM13ek";
        }
        public decimal DegreesLat()
        {
            return 33.4375m;
        }
        public decimal DegreesLon()
        {
            return -117.6250m;
        }   //     //  	    GoogleMapped DD:    33.4375, -117.6250
        public string GoogleMapsDD()
        {
            return $"33.4375{ DegreesSymbol }, -117.6250{ DegreesSymbol }";
        }
        public string GoogleMapsDMS()
        {
            return $"N 33{ DegreesSymbol }26{ MinutesSymbol }15.0{ SecondsSymbol }, " +
                   $"W 117{ DegreesSymbol }37{ MinutesSymbol }30.0{ SecondsSymbol }";
        }   //  GoogleMapped DMS:	N 33*26'15", W 117*37'30"
        public string CalculatedDDM()
        {
            return $"33{ DegreesSymbol }26.25{ MinutesSymbol }N, 117{ DegreesSymbol }37.50{ MinutesSymbol }W";
        }
        public string AttainableDDM()
        {
            return $"33{ DegreesSymbol }26.25{ MinutesSymbol }N, 117{ DegreesSymbol }37.50{ MinutesSymbol }W";
            //  <33°27.50'N, 117°40.00'W>. Actual:<33°26.25'N, 117°37.50'W>
        }
        /*
	    ARRL DDM:		    33*26.25'N, 117*37.5'W
	    ARRL Gridsquare:    DM13ek
	    GoogleMapped DD:    33.4375, -117.6250
	    GoogleMapped DMS:	N 33*26'15", W 117*37'30"
	    Calculated DDM:	    33*26.25'N, 117*37.50'W
        Attainable DDM:     33*27.50'N, 117*40.00'W
        */
    }
}
