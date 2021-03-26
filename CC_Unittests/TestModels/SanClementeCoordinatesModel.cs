namespace CoordinateConversionUtility_UnitTests.TestModels
{
    internal class SanClementeCoordinatesModel : RootCoordinateModel
    {
        internal static string NS => "N";
        internal static string EW => "W";
        public SanClementeCoordinatesModel()
        {
            DegreesLat = 33.43749m; //33.4375m;
            DegreesLon = -117.62495m; //-117.6250m;
            DdmMinsLat = 26.25m;
            DdmMinsLon = 37.49m;
            DmsSecondsLat = 14.9m;
            DmsSecondsLon = 29.8m;
        }
        public static string StrGridsquare()
        {
            return $"DM13ek";
        }
        public static string StrDD()
        {
            return $"{ 33.43749m:f5}{ DegreesSymbol }, { -117.62495m:f5}{ DegreesSymbol }";
        }
        public static string StrDDM()
        {
            return $"33{ DegreesSymbol }26.25{ MinutesSymbol }{ NS }, " +
                   $"117{ DegreesSymbol }37.49{ MinutesSymbol }{ EW }";
        }

        /// <summary>
        /// Coordinates that point to middle of gridsquare RE78ir
        /// </summary>
        /// <returns></returns>
        public static string StrArrlDDM()
        {
            return $"33{ DegreesSymbol }26.25{ MinutesSymbol }{ NS }, " +
                   $"117{ DegreesSymbol }37.50{ MinutesSymbol }{ EW }";
        }
        public static string StrDMS()
        {
            return $"{ NS } 33{ DegreesSymbol }26{ MinutesSymbol }15.0{ SecondsSymbol}, " +
                   $"{ EW } 117{ DegreesSymbol }37{ MinutesSymbol }29.8{ SecondsSymbol }";
        }
        /*
        Validated: 7-Mar-2021
	    ARRL DDM:		    33*26.25'N, 117*37.50'W
	    ARRL Gridsquare:    DM13ek
	    GoogleMapped DD:    33.43749, -117.62495
	    GoogleMapped DMS:	N 33*26'15.0", W 117*37'29.8"
	    Calculated DDM:	    33*26.25'N, 117*37.50'W
        Attainable DDM:     33*27.50'N, 117*40.00'W
        Calculated Gridsquare: DM13gk
        */
    }
}
