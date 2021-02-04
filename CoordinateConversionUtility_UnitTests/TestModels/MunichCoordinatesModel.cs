using System;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class MunichCoordinatesModel : SymbolHelper
    {
        public string ArrlDDM()
        {
            return $"48{ DegreesSymbol }8.80{ MinutesSymbol }N, 11{ DegreesSymbol }36.50{ MinutesSymbol}E";
        }   //  ARRL DDM:			48*8.8'N, 11*36.5'E
        public string ArrlGridsquare()
        {
            return "JN58td";
        }
        public decimal DegreesLat()
        {
            return 48.1467m;
        }
        public decimal DegreesLon()
        {
            return 11.6083m;
        }   //  GoogelMapped DD:	48.1467, 11.6083
        public string GoogleMapsDD()
        {
            return $"48.1467{ DegreesSymbol }, 11.6083{ DegreesSymbol }";
        }
        public string GoogleMapsDMS()
        {
            return $"N 48{ DegreesSymbol }8{ MinutesSymbol }48.10{ SecondsSymbol }, " +
                   $"E 11{ DegreesSymbol }36{ MinutesSymbol }29.90{ SecondsSymbol }";
        }   //  GoogleMapped DMS:	48°08'48.1"N 11°36'29.9"E
        public string CalculatedDDM()
        {
            return $"48{ DegreesSymbol }8.80{ MinutesSymbol }N, 11{ DegreesSymbol }36.49{ MinutesSymbol }E";
        }
        public string AttainableDDM()
        {
            return $"48{ DegreesSymbol }08.75{ MinutesSymbol }N, 11{ DegreesSymbol }37.50{ MinutesSymbol }E";
        }
    }
    /*
 	ARRL DDM:			48*8.8'N, 11*36.5'E
	ARRL Gridsquare:	JN58td
	GoogelMapped DD:	48.1467, 11.6083
	GoogleMapped DMS:	48°08'48.1"N 11°36'29.9"E
	Calculated DDM:		48*08.80'N, 11*36.49'E
	Attainable DDM:		48*10.00'N, 11*35.00'E
    TODO:               The TABLES (Grid->DDM) will push 36.49 UP to 40.00 NOT DOWN to 35.00
    */
}