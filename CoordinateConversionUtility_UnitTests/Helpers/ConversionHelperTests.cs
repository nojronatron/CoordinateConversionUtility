using CoordinateConversionUtility_UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoordinateConversionUtility.Helpers.Tests
{
    [TestClass()]
    public class ConversionHelperTests
    {
        internal static char CommaSymbol => (char)44;    //  comma symbol
        internal static char DegreesSymbol => (char)176; //  degree symbol
        internal static char MinutesSymbol => (char)39;      //  single quote
        internal static char SecondsSymbol => (char)34;      //  double quote
        internal static char SpaceCharacter => (char)32;    //  spacebar
        internal char[] trimChars = { CommaSymbol, DegreesSymbol, MinutesSymbol, SecondsSymbol, SpaceCharacter };

        [TestMethod()]
        public void ExtractPolarityTest()
        {
            var expectedNorthResult = 1;
            var expectedSouthResult = -1;
            var expectedZeroResult = 0;

            var actualNorthResult = ConversionHelper.ExtractPolarity(10.0987m);
            var actualSouthResult = ConversionHelper.ExtractPolarity(-1.1234m);
            var actualZeroResult = ConversionHelper.ExtractPolarity(-0m);

            Assert.AreEqual(expectedNorthResult, actualNorthResult);
            Assert.AreEqual(expectedSouthResult, actualSouthResult);
            Assert.AreEqual(expectedZeroResult, actualZeroResult);
        }

        [TestMethod()]
        public void ExtractPolarityNSTest()
        {
            var expectedPositiveResult = 1;
            var expectedNegativeResult = -1;
            var expectedZeroResult = 0;

            var actualPositiveResult = ConversionHelper.ExtractPolarityNS(SanClementeCoordinatesModel.strDDM());
            var actualNegativeResult = ConversionHelper.ExtractPolarityNS(WellingtonCoordinateModel.strDDM());
            var actualZeroResult = ConversionHelper.ExtractPolarityNS(string.Empty);

            Assert.AreEqual(expectedPositiveResult, actualPositiveResult);
            Assert.AreEqual(expectedNegativeResult, actualNegativeResult);
            Assert.AreEqual(expectedZeroResult, actualZeroResult);
        }

        [TestMethod()]
        public void ExtractPolarityEWTest()
        {
            var expectedPositiveResult = 1;
            var expectedNegativeResult = -1;
            var expectedZeroResult = 0;

            var actualPositiveResult = ConversionHelper.ExtractPolarityEW(MunichCoordinatesModel.strDDM());
            var actualNegativeResult = ConversionHelper.ExtractPolarityEW(SanClementeCoordinatesModel.strDDM());
            var actualZeroResult = ConversionHelper.ExtractPolarityEW(string.Empty);

            Assert.AreEqual(expectedPositiveResult, actualPositiveResult);
            Assert.AreEqual(expectedNegativeResult, actualNegativeResult);
            Assert.AreEqual(expectedZeroResult, actualZeroResult);
        }

        [TestMethod()]
        public void GetNSEW_Test()
        {
            var expectedLattitudeResult = "N";
            var expectedLongitudeResult = "W";
            var expectedCombinedResult = "NW";

            var lcm = LynnwoodCoordinatesModel.strDDM();
            var splitLatAndLon = lcm.ToString().Split(CommaSymbol);
            var lcmLattitude = splitLatAndLon[0];
            var lcmLongitude = splitLatAndLon[1];

            var actualLattitudeResult = ConversionHelper.GetNSEW(lcmLattitude);
            var actualLongitudeResult = ConversionHelper.GetNSEW(lcmLongitude);
            var actualCombinedResult = ConversionHelper.GetNSEW(lcm);

            Assert.AreEqual(expectedLattitudeResult, actualLattitudeResult);
            Assert.AreEqual(expectedLongitudeResult, actualLongitudeResult);
            Assert.AreEqual(expectedCombinedResult, actualCombinedResult);
        }

        [TestMethod()]
        public void GetNSEW2_Test1()
        {
            var expectedNorthResult = "N";
            var expectedEastResult = "E";
            var expectedNonResult = "N";    //  default lattitude result is "N"

            var munich = new MunichCoordinatesModel();
            
            var actualNorthResult = ConversionHelper.GetNSEW(munich.DegreesLat, 1);
            var actualEastResult = ConversionHelper.GetNSEW(munich.DegreesLon, 2);
            var actualLattitudeNonResult = ConversionHelper.GetNSEW(0, 1);

            Assert.AreEqual(expectedNorthResult, actualNorthResult);
            Assert.AreEqual(expectedEastResult, actualEastResult);
            Assert.AreEqual(expectedNonResult, actualLattitudeNonResult);
        }

        [TestMethod()]
        public void GetNSEW2_Test2()
        {
            var expectedSouthResult = "S";
            var expectedWestResult = "W";
            var expectedNonResult = "E";    //  default longitude result is "E"

            var montevideo = new MontevideoCoordinateModel();

            var actualSouthResult = ConversionHelper.GetNSEW(montevideo.DegreesLat, 1);
            var actualWestResult = ConversionHelper.GetNSEW(montevideo.DegreesLon, 2);
            var actualLongitudeNonResult = ConversionHelper.GetNSEW(0, 2);

            Assert.AreEqual(expectedSouthResult, actualSouthResult);
            Assert.AreEqual(expectedWestResult, actualWestResult);
            Assert.AreEqual(expectedNonResult, actualLongitudeNonResult);
        }

        [TestMethod()]
        public void IsValidTest()
        {
            decimal lattitude = 48.1467m;
            decimal longitude = 11.6083m;

            var expectedResult = true;
            var actualResult = ConversionHelper.IsValid(lattitude, longitude);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void IsNotValidTest_Lat()
        {
            //  DegreesLat = 48.1467m;
            //  DegreesLon = 11.6083m;
            decimal lattitude = 93.1467m;
            decimal longitude = 11.6083m;

            var expectedResult = false;
            var actualResult = ConversionHelper.IsValid(lattitude, longitude);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void IsNotValidTest_Lon()
        {
            decimal lattitude = 48.1467m;
            decimal longitude = 191.6083m;

            var expectedResult = false;
            var actualResult = ConversionHelper.IsValid(lattitude, longitude);

            Assert.AreEqual(expectedResult, actualResult);
        }


        [TestMethod()]
        public void IsNotValidTest_NegativeLat()
        {
            decimal lattitude = -93.1467m;
            decimal longitude = 11.6083m;

            var expectedResult = false;
            var actualResult = ConversionHelper.IsValid(lattitude, longitude);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void IsNotValidTest_NegativeLon()
        {
            decimal lattitude = 48.1467m;
            decimal longitude = -180.6083m;

            var expectedResult = false;
            var actualResult = ConversionHelper.IsValid(lattitude, longitude);

            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}