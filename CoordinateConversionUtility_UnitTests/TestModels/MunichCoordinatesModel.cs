namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class MunichCoordinatesModel : RootCoordinateModel
    {
        internal static string NS => "N";
        internal static string EW => "E";
        public MunichCoordinatesModel()
        {
            DegreesLat = 48.14690m;
            DegreesLon = 11.60833m;
            DdmMinsLat = 8.81m;
            DdmMinsLon = 36.49m;
            DmsSecondsLat = 48.80m;
            DmsSecondsLon = 30.0m;
        }

        public static string strGridSquare()
        {
            return $"JN58td";
        }
        public static string strDD()
        {
            return $"{ 48.14690:f5}{ DegreesSymbol }, { 11.60833:f5}{ DegreesSymbol }";
        }
        /// <summary>
        /// Calculated DDM test program output against.
        /// </summary>
        /// <returns></returns>
        public static string strDDM()
        {
            return $"48{ DegreesSymbol }08.81{ MinutesSymbol }{ NS }, " +
                   $"11{ DegreesSymbol }36.49{ MinutesSymbol }{ EW }";
        }

        /// <summary>
        /// Coordinates that point to the middle of the gridsquare
        /// </summary>
        /// <returns></returns>
        public static string strArrlDDM()
        {
            return $"48{ DegreesSymbol }07.75{ MinutesSymbol }{ NS }, " +
                   $"11{ DegreesSymbol }37.50{ MinutesSymbol }{ EW }";
        }

        public static string strDMS()
        {
            return $"N 48{ DegreesSymbol }08{ MinutesSymbol }48.60{ SecondsSymbol}, " +
                   $"E 11{ DegreesSymbol }36{ MinutesSymbol }29.40{ SecondsSymbol }";
        }
    }
    /*
    Validated on GoogleMaps 6-Feb-21
 	ARRL input DDM:			48*8.8'N, 11*36.5'E
	ARRL input Gridsquare:	JN58td
	GoogelMapped DD:	48.14690, 11.60833
    GoogleMapped DMS:	48°08'48.8"N 11°36'30.0"E
	Calculated DDM:		48*08.81'N, 11*36.49'E
	Attainable DDM:		48*08.75'N, 11*37.50'E
    */
}
