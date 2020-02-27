using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class MontevideoCoordinateModel : SymbolHelper
    {
        //  gridsquare2ddm failed. Expected:<34°54.60'S, 56°12.70'W>. Actual:<34°55.00'S, 56°15.00'W>.
        public string ArrlDDM()
        {
            return $"34{ DegreesSymbol }54.60{ MinutesSymbol }S, 56{ DegreesSymbol }12.70{ MinutesSymbol}W";
        }   //  ARRL DDM: 34*54.6'S, 56*12.7'W
        public string ArrlGridsquare()
        {
            return "GF15vc";
        }
        public decimal DegreesLat()
        {
            return -34.9100m;
        }
        public decimal DegreesLon()
        {
            return -56.2117m;
        }   //  -34.9100,-56.2117
        public string GoogleMapsDD()
        {
            return $"-34.9100{ DegreesSymbol }, -56.2117{ DegreesSymbol }";
        }
        public string GoogleMapsDMS()
        {
            return $"S 34{ DegreesSymbol }54{ MinutesSymbol }36.0{ SecondsSymbol }, " +
                   $"W 56{ DegreesSymbol }12{ MinutesSymbol }42.1{ SecondsSymbol }";
        }   //  S 34°54'36.0", W 56°12'42.1"
        public string CalculatedDDM()
        {
            return $"34{ DegreesSymbol }54.60{ MinutesSymbol }S, 56{ DegreesSymbol }12.70{ MinutesSymbol }W";
        }
        public string AttainableDDM()
        {
            //  Expected:<34°55.00'S, 56°15.00'W>. Actual:<34°53.75'S, 56°12.50'W>.
            return $"34{ DegreesSymbol }53.75{ MinutesSymbol }S, 56{ DegreesSymbol }12.50{ MinutesSymbol }W";
        }
        /*  24-Feb-2020 calculations
	    ARRL DDM: 34*54.6'S, 56*12.7'W
	    ARRL Gridsquare: GF15vc
	    GoogleMapped DD:	-34.9100,-56.2117
    	GoogleMapped DMS:	S 34°54'36.0", W 56°12'42.1"
	    Calculated DDM:		34*54.60'S, 56*12.70'W
    	Attainable DDM:		34*53.75'S, 56*12.50'W
        */
    }
}
