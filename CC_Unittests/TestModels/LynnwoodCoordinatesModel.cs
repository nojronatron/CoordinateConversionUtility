﻿namespace CC_Unittests.TestModels
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
        public static string StrGridSquare()
        {
            return $"CN87UT";
        }
        public static string StrDD()
        {
            return $"{ 47.82533m:f5}{ DegreesSymbol }, { -122.29333m:f5}{ DegreesSymbol }";
        }
        public static string StrDDM()
        {
            return $"47{ DegreesSymbol }49.52{ MinutesSymbol }N, " +
                   $"122{ DegreesSymbol }17.60{ MinutesSymbol }W";
        }

        public static string StrDMS()
        {
            return $"N 47{ DegreesSymbol }49{ MinutesSymbol }31.1{ SecondsSymbol}, " +
                   $"W 122{ DegreesSymbol }17{ MinutesSymbol }36.2{ SecondsSymbol }";
        }

    }
}
