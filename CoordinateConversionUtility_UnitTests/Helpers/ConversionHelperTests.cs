using CoordinateConversionUtility.Models.Tests;
using CoordinateConversionUtility_UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoordinateConversionUtility.Helpers.Tests
{
    [TestClass()]
    public class ConversionHelperTests : UnitTestsBase
    {
        [TestMethod()]
        public void ValidateLattitude()
        {
            var expectedResult = true;
            var expectedOutput = -35m;

            var strLattitude = "-35";

            decimal actualOutput = 0.0m;
            bool actualResult = ConversionHelper.ValidateIsLattitude(strLattitude, out actualOutput);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod()]
        public void ValidateLongitude()
        {
            var expectedresult = true;
            var expectedOutput = -123m;

            var strLongitude = "-123";

            decimal actualOutput = 0.0m;
            bool actualResult = ConversionHelper.ValidateIsLongitude(strLongitude, out actualOutput);

            Assert.AreEqual(expectedresult, actualResult);
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod()]
        public void ValidateMinutes()
        {
            var expectedResultMins = true;
            var expectedOutputMins = 45.55m;

            var strMinutes = "45.55";

            decimal actualMinsOutput = 0.0m;
            bool actualresultMins = ConversionHelper.ValidateIsSecsOrMins(strMinutes, out actualMinsOutput);

            Assert.AreEqual(expectedResultMins, actualresultMins);
            Assert.AreEqual(expectedOutputMins, actualMinsOutput);
        }

        [TestMethod()]
        public void ValidateSeconds()
        {
            var expectedResultSecs = true;
            var expectedOutputSecs = 34.21m;

            var strSeconds = "34.21";

            decimal actualOutputSecs = 0.0m;
            bool actualResultSecs = ConversionHelper.ValidateIsSecsOrMins(strSeconds, out actualOutputSecs);

            Assert.AreEqual(expectedResultSecs, actualResultSecs);
            Assert.AreEqual(expectedOutputSecs, actualOutputSecs);
        }

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

            var lcm = LynnwoodCoordinatesModel.StrDDM();
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
            var expectedNonResult = "N";

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
            var expectedNonResult = "E";

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

        [TestMethod()]
        public void LatDecimalIsValid_PassingPosAndNeg()
        {
            decimal positiveLat = 48.1467m;
            decimal negativeLat = -48.1467m;
            decimal zeroLat = 0.0m;

            bool expectedResult = true;
            var positiveLatActualResult = ConversionHelper.LatDecimalIsValid(positiveLat);
            var negativeLatACtualResult = ConversionHelper.LatDecimalIsValid(negativeLat);
            var zeroLatActualResult = ConversionHelper.LatDecimalIsValid(zeroLat);

            Assert.AreEqual(expectedResult, positiveLatActualResult);
            Assert.AreEqual(expectedResult, negativeLatACtualResult);
            Assert.AreEqual(expectedResult, zeroLatActualResult);
        }

        [TestMethod()]
        public void LonDecimalIsValid_PassingPosAndNeg()
        {
            decimal positiveLon = 179.1467m;
            decimal negativeLon = -179.1467m;
            decimal zeroLon = 0.0m;

            bool expectedResult = true;
            var positiveLonActualResult = ConversionHelper.LonDecimalIsValid(positiveLon);
            var negativeLonActualResult = ConversionHelper.LonDecimalIsValid(negativeLon);
            var zeroLonActualResult = ConversionHelper.LonDecimalIsValid(zeroLon);

            Assert.AreEqual(expectedResult, positiveLonActualResult);
            Assert.AreEqual(expectedResult, negativeLonActualResult);
            Assert.AreEqual(expectedResult, zeroLonActualResult);
        }

        [TestMethod()]
        public void LatDecimalIsValid_FalseScenarios()
        {
            decimal test1 = 90.000001m;
            decimal test2 = 90.1m;
            decimal test3 = 91.0m;
            decimal test4 = -90.000001m;
            decimal test5 = -90.1m;
            decimal test6 = -91.0m;

            bool expectedResult = false;

            var test1Actual = ConversionHelper.LatDecimalIsValid(test1);
            var test2Actual = ConversionHelper.LatDecimalIsValid(test2);
            var test3Actual = ConversionHelper.LatDecimalIsValid(test3);
            var test4Actual = ConversionHelper.LatDecimalIsValid(test4);
            var test5Actual = ConversionHelper.LatDecimalIsValid(test5);
            var test6Actual = ConversionHelper.LatDecimalIsValid(test6);

            Assert.AreEqual(expectedResult, test1Actual);
            Assert.AreEqual(expectedResult, test2Actual);
            Assert.AreEqual(expectedResult, test3Actual);
            Assert.AreEqual(expectedResult, test4Actual);
            Assert.AreEqual(expectedResult, test5Actual);
            Assert.AreEqual(expectedResult, test6Actual);
        }

        [TestMethod()]
        public void LonDecimalIsValid_FalseScenarios()
        {
            decimal test1 = 180.000001m;
            decimal test2 = 180.1m;
            decimal test3 = 181.0m;
            decimal test4 = -180.000001m;
            decimal test5 = -180.1m;
            decimal test6 = -181.0m;

            bool expectedResult = false;

            var test1Actual = ConversionHelper.LonDecimalIsValid(test1);
            var test2Actual = ConversionHelper.LonDecimalIsValid(test2);
            var test3Actual = ConversionHelper.LonDecimalIsValid(test3);
            var test4Actual = ConversionHelper.LonDecimalIsValid(test4);
            var test5Actual = ConversionHelper.LonDecimalIsValid(test5);
            var test6Actual = ConversionHelper.LonDecimalIsValid(test6);

            Assert.AreEqual(expectedResult, test1Actual);
            Assert.AreEqual(expectedResult, test2Actual);
            Assert.AreEqual(expectedResult, test3Actual);
            Assert.AreEqual(expectedResult, test4Actual);
            Assert.AreEqual(expectedResult, test5Actual);
            Assert.AreEqual(expectedResult, test6Actual);
        }

        [TestMethod()]
        public void GetLatDegrees_North_Pass()
        {
            var LTH = new LookupTablesHelper();
            decimal actualResult;
            short actualLatDirection;
            decimal expectedResult = 40;
            short expectedLatDirection = 1;

            if (LTH.GenerateTableLookups())
            {
                var grid = "CN87ut";
                actualResult = ConversionHelper.GetLatDegrees(LTH, grid, out actualLatDirection);
            }
            else
            {
                actualResult = 0;
                actualLatDirection = 0;
            }

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedLatDirection, actualLatDirection);
        }

        [TestMethod()]
        public void GetLatDegrees_South_Pass()
        {
            var LTH = new LookupTablesHelper();
            decimal actualResult;
            short actualLatDirection;
            decimal expectedResult = -120;
            short expectedLatDirection = -1;

            if (LTH.GenerateTableLookups())
            {
                var grid = "CN87ut";
                actualResult = ConversionHelper.GetLonDegrees(LTH, grid, out actualLatDirection);
            }
            else
            {
                actualResult = 0;
                actualLatDirection = 0;
            }

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedLatDirection, actualLatDirection);
        }

        [TestMethod()]
        public void AddLatDegrees_North_Pass()
        {
            var startingLat = 40.0m;
            var latDir = 1;
            var grid = "CN87ut";

            var expectedResult = 47m;

            var actualResult = ConversionHelper.AddLatDegreesRemainder(startingLat, latDir, grid);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void AddLonDegrees_West_Pass()
        {
            var startingLat = -120.0m;
            var latDir = -1;
            var grid = "CN87ut";

            var expectedResult = -122.0m;

            var actualResult = ConversionHelper.AddLonDegreesRemainder(startingLat, latDir, grid);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetLatMinutes_North_Pass()
        {
            var LTH = new LookupTablesHelper();
            var startingLat = 47.0m;
            var latDir = 1;
            var grid = "CN87ut";

            decimal actualAdjLatDegrees;
            decimal actualLatMinutesResult;

            var expectedAdjLatDegrees = 47.0m;
            var expectedResult = 48.75m;

            if (LTH.GenerateTableLookups())
            {
                actualLatMinutesResult = ConversionHelper.GetLatMinutes(LTH, startingLat, latDir, grid, out actualAdjLatDegrees);
            }
            else
            {
                actualAdjLatDegrees = 0m;
                actualLatMinutesResult = 0m;
            }

            Assert.AreEqual(expectedResult, actualLatMinutesResult);
            Assert.AreEqual(expectedAdjLatDegrees, actualAdjLatDegrees);
        }

        [TestMethod()]
        public void GetLonMinutes_West_Pass()
        {
            var LTH = new LookupTablesHelper();
            var startingLon = -122.0m;
            var lonDir = -1;
            var grid = "CN87ut";

            decimal actualAdjLonDegrees;
            decimal actualResult;

            var expectedAdjLatDegrees = -122.0m;
            var expectedLonMinutes = 17.5m;

            if (LTH.GenerateTableLookups())
            {
                actualResult = ConversionHelper.GetLonMinutes(LTH, startingLon, lonDir, grid, out actualAdjLonDegrees);
            }
            else
            {
                actualAdjLonDegrees = 0m;
                actualResult = 0m;
            }

            Assert.AreEqual(expectedLonMinutes, actualResult);
            Assert.AreEqual(expectedAdjLatDegrees, actualAdjLonDegrees);
        }

        [TestMethod()]
        public void GetNearestEvenMultiple_Lat_Pass()
        {
            var minutesInput = 49.52m;

            var expectedResult = 50.0m;

            var actualResult = ConversionHelper.GetNearestEvenMultiple(minutesInput, 1);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetNearestEvenMultiple_NegativeLat_Pass()
        {
            var minutesInput = -54.60m;

            var expectedResult = -52.5m;

            var actualResult = ConversionHelper.GetNearestEvenMultiple(minutesInput, 1);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetNearestEvenMultiple_Lat_ZeroIsZero()
        {
            var minutesInput = 0.0m;

            var expectedResult = 0.0m;

            var actualResult = ConversionHelper.GetNearestEvenMultiple(minutesInput, 1);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetNearestEvenMultiple_Lon_Pass()
        {
            var minutesInput = 36.50m;

            var expectedResult = 40.0m;

            var actualResult = ConversionHelper.GetNearestEvenMultiple(minutesInput, 2);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetNearestEvenMultiple_NegativeLon_Pass()
        {
            var minutesInput = -17.60m;

            var expectedResult = -15.0m;

            var actualResult = ConversionHelper.GetNearestEvenMultiple(minutesInput, 2);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetNearestEvenMultiple_ZeroIsZero()
        {
            var minutesInput = 0.0m;

            var expectedResult = 0.0m;

            var actualResult = ConversionHelper.GetNearestEvenMultiple(minutesInput, 2);

            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}
