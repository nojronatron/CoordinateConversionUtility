using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class WashingtondcCoordinateModel : SymbolHelper
    {
        public string ArrlDDM()
        {
            return $"38{ DegreesSymbol }55.20{ MinutesSymbol }N, 77{ DegreesSymbol }3.90{ MinutesSymbol}W";
        }//38*55.2'N, 77*3.9'W
        public string ArrlGridsquare()
        {
            return "FM18lw";
        }
        public decimal DegreesLat()
        {
            return 38.9200m;
        }
        public decimal DegreesLon()
        {
            return -77.0650m;
        }
        public string GoogleMapsDD()
        {
            return $"38.9200{ DegreesSymbol }, -77.0650{ DegreesSymbol }";
        }   //  38.9200,-77.0650
        public string GoogleMapsDMS()
        {
            return $"N 38{ DegreesSymbol }55{ MinutesSymbol }12.0{ SecondsSymbol }, " +
                   $"W 77{ DegreesSymbol }3{ MinutesSymbol }54.0{ SecondsSymbol }";
        }   //  38°55'12.0"N 77°03'54.0"W
        public string CalculatedDDM()
        {
            return $"38{ DegreesSymbol }55.20{ MinutesSymbol }N, 77{ DegreesSymbol }3.90{ MinutesSymbol }W";
        }
        public string AttainableDDM()
        {
            return $"38{ DegreesSymbol }56.25{ MinutesSymbol }N, 77{ DegreesSymbol }02.50{ MinutesSymbol }W";
            //  38°57.50'N, 77°5.00'W>. Actual:<38°56.25'N, 77°02.50'W
        }
        /*
        ARRL DDM:	 		38*55.2'N, 77*3.9'W
        ARRL Gridsquare:	FM18lw
        GoogleMapped DD:	38.9200,-77.0650
        GoogleMapped DMS:	38°55'12.0"N 77°03'54.0"W
        Calculated DDM:		38*55.2'N, 77*03.90'W
        Attainable DDM:		38*57.50'N, 77*05.00'W
        */
    }
}
