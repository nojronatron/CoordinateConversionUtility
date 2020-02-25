using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    class MontevideoCoordinateModel : RootCoordinateModel
    {
        public MontevideoCoordinateModel()
        {
            DegreesLat = -34.9100m;
            DegreesLon = -56.2117m;
        }
        public static string strGridsquare()
        {
            return $"GF15vc";
        }
        public static string strDD()
        {
            return $"{ -34.9100m }{ DegreesSymbol }, { -56.2117m }{ DegreesSymbol }";
        }
        public static string strDDM()
        {
            return $"34{ DegreesSymbol }55.00{ MinutesSymbol }S, " +
                   $"56{ DegreesSymbol }15.00{ MinutesSymbol }W";
        }
        public static string strMidGridDDM()
        {
            //  these are the ARRL coordinates that point to middle of gridsquare GF15vc
            //  Submit this method to test accurate gridsquare result e.g.: ConvertGridsquareToDDM()
            return $"34{ DegreesSymbol }54.60{ MinutesSymbol }S, " +
                   $"56{ DegreesSymbol }12.70{ MinutesSymbol }W";
        }
        public static string strDMS()
        {
            return $"S 34{ DegreesSymbol }53{ MinutesSymbol }45{ SecondsSymbol}, " +
                   $"W 56{ DegreesSymbol }12{ MinutesSymbol }30{ SecondsSymbol }";
        }
        /*  24-Feb-2020 calculations
	    ARRL DDM: 34*54.6'S, 56*12.7'W
	    ARRL Gridsquare: GF15vc
	    GoogleMapped DD:	-34.9100,-56.2117
    	GoogleMapped DMS:	S 34°54'36.0", W 56°12'42.1"
	    Calculated DDM:		34*54.6'S, 56*12.70'W
    	Attainable DDM:		34*55.0'S, 56*15.0'W
        */
    }
}
