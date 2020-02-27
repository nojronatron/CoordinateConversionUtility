using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class LynnwoodCoordinatesModel : SymbolHelper
    {
        public string ArrlDDM()
        {
            //  DONE: Ensure this is the starting point desired
            return $"47{ DegreesSymbol }50.02{ MinutesSymbol }N, 122{ DegreesSymbol }16.31{ MinutesSymbol}W";
        }
        public string ArrlGridsquare()
        {
            //  DONE: After all other validations then calculate correct Gridsquare here
            return "CN87uu";    
            //  using 47.8334,-122.2719
            //  via https://www.karhukoti.com/maidenhead-grid-square-locator/?grid=CN87
        }
        public decimal DegreesLat()
        {
            return 47.8334m;
        }
        public decimal DegreesLon()
        {
            return -122.2719m;
        }
        public string GoogleMapsDD()
        {
            //  DONE: Validate via GoogleMaps
            return $"47.8334{ DegreesSymbol }, -122.2719{ DegreesSymbol }";
        }
        public string GoogleMapsDMS()
        {
            //  TODO: Validate via GoogleMaps
            return $"N 47{ DegreesSymbol }50{ MinutesSymbol }1.20{ SecondsSymbol }, " +
                   $"W 122{ DegreesSymbol }16{ MinutesSymbol }18.60{ SecondsSymbol }";
        }
        public string CalculatedDDM()
        {
            //  DONE: Validate by hand
            return $"47{ DegreesSymbol }50.02{ MinutesSymbol }N, 122{ DegreesSymbol }16.31{ MinutesSymbol }W";
        }
        public string AttainableDDM()
        {
            //  DONE: Validate by hand with ARRL charts
            return $"47{ DegreesSymbol }51.25{ MinutesSymbol }N, 122{ DegreesSymbol }17.50{ MinutesSymbol }W";
        }
        /*
        Lynnwood WA
	    ARRL:	n/a
	    DMS:	
	    Model DDM:		47*49.21'N, 122*18.02'W
	    Corrected DDM to try:	47*50.00'N, 122*17.5'W
	    Corrected DD to try:	47.8334, -122.2719
	    Corrected DMS to try:	N47*50'00", W122*17'30"
        */
    }
}
