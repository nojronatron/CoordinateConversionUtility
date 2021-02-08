using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    class WashingtondcCoordinateModel : RootCoordinateModel
    {
        public WashingtondcCoordinateModel()
        {
            DegreesLat = 38.9200m;
            DegreesLon = -77.0650m;
        }
        public static string strGridsquare()
        {
            return $"FM18lw";
        }
        public static string strDD()
        {
            return $"{ 38.9200m:f5}{ DegreesSymbol }, { -77.0650m:f5}{ DegreesSymbol }";
        }
        public static string strArrlDDM()
        {
            //  Submit this method to test accurrate gridsquare result e.g.: ConvertGridsquareToDDM()
            return $"38{ DegreesSymbol }55.20{ MinutesSymbol }N, " +
                   $"77{ DegreesSymbol }03.90{ MinutesSymbol }W";
        }
        public static string strDDM()
        {
            //  Attainable DDM
            return $"38{ DegreesSymbol }57.50{ MinutesSymbol }N, " +
                   $"77{ DegreesSymbol }05.00{ MinutesSymbol }W";
        }
        public static string strDMS()
        {
            return $"N 38{ DegreesSymbol }55{ MinutesSymbol }12.0{ SecondsSymbol}, " +
                   $"W 77{ DegreesSymbol }03{ MinutesSymbol }54.0{ SecondsSymbol }";
        }
    /*
 	ARRL DDM:	 		38*55.2'N, 77*3.9'W
	ARRL Gridsquare:	FM18lw
	GoogleMapped DD:	38.9200,-77.0650
	GoogleMapped DMS:	38°55'12.0"N 77°03'54.0"W
	Calculated DDM:		38*55.2'N, 77*03.90'W
	Attainable DDM:		38*57.50'N, 77*05.00'W
    */
    }
}
