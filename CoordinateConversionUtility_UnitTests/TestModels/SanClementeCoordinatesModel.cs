using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    class SanClementeCoordinatesModel : RootCoordinateModel
    {
        internal static string NS => "N";
        internal static string EW => "W";
        public SanClementeCoordinatesModel()
        {
            DegreesLat = 33.4375m;
            DegreesLon = -117.6250m;
        }
        public static string strGridsquare()
        {
            return $"DM13ek";
        }
        public static string strDD()
        {
            return $"{ 33.4375m }{ DegreesSymbol }, { -117.6250m }{ DegreesSymbol }";
        }
        public static string strDDM()
        {
            return $"33{ DegreesSymbol }27.50{ MinutesSymbol }{ NS }, " +
                   $"117{ DegreesSymbol }40.00{ MinutesSymbol }{ EW }";
        }
        public static string strArrlDDM()
        {
            //  these are the ARRL coordinates that point to middle of gridsquare RE78ir
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
	    GoogleMapped DD:    33.4375, -117.6250
	    GoogleMapped DMS:	N 33*26'15", W 117*37'30"
	    Calculated DDM:	    33*26.25'N, 117*37.50'W
        Attainable DDM:     33*27.50'N, 117*40.00'W
        */
    }
}
