using System;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class MunichCoordinatesModel : RootCoordinateModel
    {
        internal static string NS => "N";
        internal static string EW => "E";
        public MunichCoordinatesModel()
        {
            DegreesLat = 48.1467m;
            DegreesLon = 11.6083m;
        }
        public static string strGridSquare()
        {
            return $"JN58td";
        }
        public static string strDD()
        {
            return $"{ 48.1467 }{ DegreesSymbol }, { 11.6083 }{ DegreesSymbol }";
        }
        public static string strDDM()
        {
            //  This is a RESULT DDM to test program output against
            return $"48{ DegreesSymbol }10.00{ MinutesSymbol }N, " +
                   $"11{ DegreesSymbol }40.00{ MinutesSymbol }E";
        }
        public static string strArrlDDM()
        {
            //  these are the ARRL coordinates that point to middle of gridsquare RE78ir
            return $"48{ DegreesSymbol }08.80{ MinutesSymbol }{ NS }, " +
                   $"11{ DegreesSymbol }36.50{ MinutesSymbol }{ EW }";
        }
        public static string strDMS()
        {
            return $"N 48{ DegreesSymbol }08{ MinutesSymbol }48.1{ SecondsSymbol}, " +
                   $"E 11{ DegreesSymbol }36{ MinutesSymbol }29.9{ SecondsSymbol }";
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
