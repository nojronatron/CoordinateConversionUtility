using System;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class LynnwoodCoordinatesModel : RootCoordinateModel
    {
        public LynnwoodCoordinatesModel()
        {
            DegreesLat = 47.82533m;
            DegreesLon = -122.29333m;    //  Google Maps rounds up to 2934; calculated is 293333...
            DdmMinsLat = 49.52m;
            DdmMinsLon = 17.60m;
            DmsSecondsLat = 31.2m;
            DmsSecondsLon = 36.0m;
        }
        public static string strGridSquare()
        {
            return $"CN87UT";
        }
        public static string strDD()
        {
            return $"{ 47.82533m }{ DegreesSymbol }, { -122.29333m }{ DegreesSymbol }";
        }
        public static string strDDM()
        {
            return $"47{ DegreesSymbol }49.52{ MinutesSymbol }N, " +
                   $"122{ DegreesSymbol }17.60{ MinutesSymbol }W";
        }

        public static string strDMS()
        {
            return $"N 47{ DegreesSymbol }49{ MinutesSymbol }31.1{ SecondsSymbol}, " +
                   $"W 122{ DegreesSymbol }17{ MinutesSymbol }36.2{ SecondsSymbol }";
        }

    }
}
