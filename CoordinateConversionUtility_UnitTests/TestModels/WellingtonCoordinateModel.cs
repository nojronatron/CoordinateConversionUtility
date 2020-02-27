
namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class WellingtonCoordinateModel : SymbolHelper
    {
        public string ArrlDDM()
        {
            return $"41{ DegreesSymbol }17.00{ MinutesSymbol }S, 174{ DegreesSymbol }44.70{ MinutesSymbol}E";
        }
        public string ArrlGridsquare()
        {
            return "RE78ir";
        }
        public decimal DegreesLat()
        {
            return -41.2833m;
        }
        public decimal DegreesLon()
        {
            return 174.7450m;
        }
        public string GoogleMapsDD()
        {
            return $"-41.2833{ DegreesSymbol }, 174.7450{ DegreesSymbol }";
        }
        public string GoogleMapsDMS()
        {
            return $" S 41{ DegreesSymbol }16{ MinutesSymbol }59.9{ SecondsSymbol }, " +
                   $"E 174{ DegreesSymbol }44{ MinutesSymbol }42.0{ SecondsSymbol }";
        }
        public string CalculatedDDM()
        {
            return $"41{ DegreesSymbol }17.00{ MinutesSymbol }S, 174{ DegreesSymbol }44.70{ MinutesSymbol }E";
        }
        public string AttainableDDM()
        {
            return $"41{ DegreesSymbol }16.25{ MinutesSymbol }S, 174{ DegreesSymbol }42.50{ MinutesSymbol }E";
            //  <41°17.50'S, 174°45.00'E>. Actual:<41°16.25'S, 174°42.50'E>
        }
        /*  24-Feb-2020
        ARRL DDM:	41*17.0'S, 174*44.7'E
        ARRL Gridsquare:	RE78ir
        GoogleMapped DD:	-41.2833, 174.7450
        GoogleMapped DMS:	S 41*16'59.9", E 174*44'42.0"
        Calculated DDM:		41*17.00'S, 174*44.70'E
        Attainable DDM:		41°17.50'S, 174°45.00'E
        */
    }
}
