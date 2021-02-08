using System;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    class MontevideoCoordinateModel : RootCoordinateModel
    {
        private static string NS => "S";
        private static string EW => "W";
        public MontevideoCoordinateModel()
        {
            DegreesLat = -34.91000m;
            DegreesLon = -56.21169m;
            DdmMinsLat = 54.60m;
            DdmMinsLon = 12.70m;
            DmsSecondsLat = 36.00m;
            DmsSecondsLon = 42.08m;
        }
        public static string strGridsquare()
        {
            return $"GF15vc";
        }
        public static string strDD()
        {
            return $"{ -34.9100m:f5}{ DegreesSymbol }, { -56.2117m:f5}{ DegreesSymbol }";
        }

        public static string strDDM()
        {
            return $"34{ DegreesSymbol }54.60{ MinutesSymbol }{ NS }, " +
                   $"56{ DegreesSymbol }12.70{ MinutesSymbol }{ EW }";
        }

        public static string strDDM_ARRL()
        {
            return $"34{ DegreesSymbol }53.75{ MinutesSymbol }{ NS }, " +
                   $"56{ DegreesSymbol }12.50{ MinutesSymbol }{ EW }";
        }

        public static string strDMS()
        {
            return $"S 34{ DegreesSymbol }54{ MinutesSymbol }36.0{ SecondsSymbol}, " +
                   $"W 56{ DegreesSymbol }12{ MinutesSymbol }42.1{ SecondsSymbol }";
        }
        /*  23-Jan-2021 calculations
	    ARRL input DDM:     34*54.6'S, 56*12.7'W
	    ARRL input Grid:    GF15vc
	    GoogleMapped DD:	-34.91000,-56.21169
    	GoogleMapped DMS:	S 34°54'36.0", W 56°12'42.1"
	    Calculated DDM:		34*54.60'S, 56*12.70'W
    	Attainable DDM:		34*53.75'S, 56*12.50'W
        */
    }
}
