using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class WellingtonCoordinateModel : RootCoordinateModel
    {
        internal static string NS => "S";
        internal static string EW => "E";
        public WellingtonCoordinateModel()
        {
            DegreesLat = -41.2833m;
            DegreesLon = 174.7450m;
        }
        public static string strGridsquare()
        {
            return $"RE78ir";
        }
        public static string strDD()
        {
            return $"{ -41.2833m }{ DegreesSymbol }, { 174.7450m }{ DegreesSymbol }";
        }
        public static string strDDM()
        {
            //  Confirmed correct: 41°17.50'S, 174°45.00'E
            //  This is a RESULT DDM to test program output against
            return $"41{ DegreesSymbol }17.50{ MinutesSymbol }S, " +
            $"174{ DegreesSymbol }45.00{ MinutesSymbol }E";
            
        }
        public static string strArrlDDM()
        {
            //  these are the ARRL coordinates that point to middle of gridsquare
            //  Submit this method to test accurrate gridsquare result e.g.: ConvertGridsquareToDDM()
            return $"41{ DegreesSymbol }17.0{ MinutesSymbol }S, " +
                   $"174{ DegreesSymbol }44.7{ MinutesSymbol }E";
        }
        public static string strDMS()
        {
            return $"S 41{ DegreesSymbol }16{ MinutesSymbol }59.9{ SecondsSymbol}, " +
                   $"W 174{ DegreesSymbol }44{ MinutesSymbol }42.0{ SecondsSymbol }";
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
