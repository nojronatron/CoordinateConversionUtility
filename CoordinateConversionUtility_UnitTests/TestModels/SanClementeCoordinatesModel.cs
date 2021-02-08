using System;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    class SanClementeCoordinatesModel : RootCoordinateModel
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
        public static string strGridsquare()
        {
            return $"DM13ek";
        }
        public static string strDD()
        {
            return $"{ 33.4375m:f5}{ DegreesSymbol }, { -117.6250m:f5}{ DegreesSymbol }";
        }
        public static string strDDM()
        {
            return $"33{ DegreesSymbol }26.25{ MinutesSymbol }{ NS }, " +
                   $"117{ DegreesSymbol }37.50{ MinutesSymbol }{ EW }";
        }

        /// <summary>
        /// Coordinates that point to middle of gridsquare RE78ir
        /// </summary>
        /// <returns></returns>
        public static string strArrlDDM()
        {

            return $"33{ DegreesSymbol }26.25{ MinutesSymbol }{ NS }, " +
                   $"117{ DegreesSymbol }37.50{ MinutesSymbol }{ EW }";
        }
        public static string strDMS()
        {
            return $"{ NS } 41{ DegreesSymbol }15{ MinutesSymbol }00{ SecondsSymbol}, " +
                   $"{ EW } 174{ DegreesSymbol }45{ MinutesSymbol }00{ SecondsSymbol }";
        }
        /*
	    ARRL DDM:		    33*26.25'N, 117*37.5'W
	    ARRL Gridsquare:    DM13ek
	    GoogleMapped DD:    33.43749, -117.62495
	    GoogleMapped DMS:	N 33*26'15.0", W 117*37'29.8"
	    Calculated DDM:	    33*26.25'N, 117*37.50'W
        Attainable DDM:     33*27.50'N, 117*40.00'W
        Calculated Gridsquare: DM13gk
        */
    }
}
