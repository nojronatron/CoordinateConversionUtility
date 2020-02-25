using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateConversionUtility_UnitTests.TestModels
{
    public class LynnwoodCoordinatesModel : RootCoordinateModel
    {
        public LynnwoodCoordinatesModel()
        {
            DegreesLat = 47.8203m;
            DegreesLon = -122.3003m;
        }
        public static string strGridSquare()
        {
            return $"CN87??";
        }
        public static string strDD()
        {
            return $"{ 47.8203m }{ DegreesSymbol }, { -122.3003m }{ DegreesSymbol }";
        }
        public static string strDDM()
        {
            return $"47{ DegreesSymbol }49.21{ MinutesSymbol }N, " +
                   $"122{ DegreesSymbol }18.02{ MinutesSymbol }W";
        }
        public static string strDMS()
        {
            return $"N 47{ DegreesSymbol }49{ MinutesSymbol }13{ SecondsSymbol}, " +
                   $"W 122{ DegreesSymbol }18{ MinutesSymbol }01{ SecondsSymbol }";
        }
    }
}
