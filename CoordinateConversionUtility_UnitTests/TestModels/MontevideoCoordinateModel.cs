using System;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    class MontevideoCoordinateModel : RootCoordinateModel
    {
        public MontevideoCoordinateModel()
        {
            DegreesLat = -34.91000m;
            DegreesLon = -56.21169m;
            DdmMinsLat = 54.60m;
            DdmMinsLon = 12.70m;
            DmsSecondsLat = 36.0m;
            DmsSecondsLon = 42.1m;
        }
        public static string strGridsquare()
        {
            return $"GF15vc";
        }
        public static string strDD()
        {
            return $"{ -34.9100m }{ DegreesSymbol }, { -56.2117m }{ DegreesSymbol }";
        }
        //public static string strDDM()
        //{
        //    return $"34{ DegreesSymbol }55.00{ MinutesSymbol }S, " +
        //           $"56{ DegreesSymbol }15.00{ MinutesSymbol }W";
        //}
        public static string strDDM()
        {
            return $"34{ DegreesSymbol }54.60{ MinutesSymbol }S, " +
                   $"56{ DegreesSymbol }12.70{ MinutesSymbol }W";
        }
        public static string strDDM_ARRL()
        {
            //  these are the ARRL coordinates that point to middle of gridsquare GF15vc
            //  Submit this method to test accurate gridsquare result e.g.: ConvertGridsquareToDDM()
            return $"34{ DegreesSymbol }55.00{ MinutesSymbol }S, " +
                   $"56{ DegreesSymbol }15.00{ MinutesSymbol }W";
        }
        public static string strDMS()
        {
            return $"S 34{ DegreesSymbol }54{ MinutesSymbol }36.0{ SecondsSymbol}, " +
                   $"W 56{ DegreesSymbol }12{ MinutesSymbol }42.1{ SecondsSymbol }";
        }
        /*  23-Jan-2021 calculations
	    ARRL DDM: 34*54.6'S, 56*12.7'W
	    ARRL Gridsquare: GF15vc
	    GoogleMapped DD:	-34.91000,-56.21169
    	GoogleMapped DMS:	S 34°54'36.0", W 56°12'42.1"
	    Calculated DDM:		34*54.6'S, 56*12.70'W
    	Attainable DDM:		34*55.0'S, 56*15.0'W
        */
    }
}
