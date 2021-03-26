namespace CC_Unittests.TestModels
{
    public class WellingtonCoordinateModel : RootCoordinateModel
    {
        internal static string NS => "S";
        internal static string EW => "E";
        public WellingtonCoordinateModel()
        {
            DegreesLat = -41.28330m;
            DegreesLon = 174.74500m;
            DdmMinsLat = 16.99m;
            DdmMinsLon = 44.70m;
            DmsSecondsLat = 59.90m;
            DmsSecondsLon = 42.00m;
        }
        public static string StrGridsquare()
        {
            return $"RE78ir";
        }
        public static string StrDD()
        {
            return $"{ -41.28330m:f5}{ DegreesSymbol }, { 174.74500m:f5}{ DegreesSymbol }";
        }

        /// <summary>
        /// Test program output against this calculated DDM.
        /// </summary>
        /// <returns></returns>
        public static string StrDDM()
        {
            //  Confirmed correct: 41°17.50'S, 174°45.00'E
            return $"41{ DegreesSymbol }16.99{ MinutesSymbol }S, " +
            $"174{ DegreesSymbol }44.70{ MinutesSymbol }E";

        }

        /// <summary>
        /// DDM Coordinates at the middle of the current gridsquare.
        /// </summary>
        /// <returns></returns>
        public static string StrArrlDDM()
        {
            return $"41{ DegreesSymbol }17.0{ MinutesSymbol }S, " +
                   $"174{ DegreesSymbol }44.7{ MinutesSymbol }E";
        }

        public static string StrAttainableDDM()
        {
            return $"41{ DegreesSymbol }17.50{ MinutesSymbol }S, " +
                   $"174{ DegreesSymbol }45.00{ MinutesSymbol }E";
        }

        public static string StrDMS()
        {
            return $"S 41{ DegreesSymbol }16{ MinutesSymbol }59.9{ SecondsSymbol}, " +
                   $"W 174{ DegreesSymbol }44{ MinutesSymbol }42.0{ SecondsSymbol }";
        }
        /*  7-Feb-2021
	    ARRL DDM:	41*17.0'S, 174*44.7'E
	    ARRL Gridsquare:	RE78ir
	    GoogleMapped DD:	-41.2833, 174.7450
	    GoogleMapped DMS:	S 41*16'59.9", E 174*44'42.0"
	    Calculated DDM:		41*16.99'S, 174*44.70'E
    	Attainable DDM:		41°17.50'S, 174°45.00'E
        */
    }
}
