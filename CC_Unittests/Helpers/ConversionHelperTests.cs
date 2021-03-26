using CoordinateConversionUtility.Models.Tests;
using CoordinateConversionUtility_UnitTests.TestModels;
using NUnit.Framework;

namespace CoordinateConversionUtility.Helpers.Tests
{
    [TestFixture()]
    public class ConversionHelperTests : UnitTestsBase
    {
        [Test()]
        public void ExtractPolarityTest()
        {
            int expectedNorthResult = 1;
            int expectedSouthResult = -1;
            int expectedZeroResult = 0;

            short actualNorthResult = ConversionHelper.ExtractPolarity(10.0987m);
            short actualSouthResult = ConversionHelper.ExtractPolarity(-1.1234m);
            short actualZeroResult = ConversionHelper.ExtractPolarity(-0m);

            Assert.AreEqual(expectedNorthResult, actualNorthResult);
            Assert.AreEqual(expectedSouthResult, actualSouthResult);
            Assert.AreEqual(expectedZeroResult, actualZeroResult);
        }

        [Test()]
        public void ExtractPolarityNSTest()
        {
            int expectedPositiveResult = 1;
            int expectedNegativeResult = -1;
            int expectedZeroResult = 0;

            short actualPositiveResult = ConversionHelper.ExtractPolarityNS(SanClementeCoordinatesModel.StrDDM());
            short actualNegativeResult = ConversionHelper.ExtractPolarityNS(WellingtonCoordinateModel.strDDM());
            short actualZeroResult = ConversionHelper.ExtractPolarityNS(string.Empty);

            Assert.AreEqual(expectedPositiveResult, actualPositiveResult);
            Assert.AreEqual(expectedNegativeResult, actualNegativeResult);
            Assert.AreEqual(expectedZeroResult, actualZeroResult);
        }

        [Test()]
        public void ExtractPolarityEWTest()
        {
            int expectedPositiveResult = 1;
            int expectedNegativeResult = -1;
            int expectedZeroResult = 0;

            short actualPositiveResult = ConversionHelper.ExtractPolarityEW(MunichCoordinatesModel.strDDM());
            short actualNegativeResult = ConversionHelper.ExtractPolarityEW(SanClementeCoordinatesModel.StrDDM());
            short actualZeroResult = ConversionHelper.ExtractPolarityEW(string.Empty);

            Assert.AreEqual(expectedPositiveResult, actualPositiveResult);
            Assert.AreEqual(expectedNegativeResult, actualNegativeResult);
            Assert.AreEqual(expectedZeroResult, actualZeroResult);
        }

        [Test()]
        public void GetNSEW_Test()
        {
            string expectedLattitudeResult = "N";
            string expectedLongitudeResult = "W";
            string expectedCombinedResult = "NW";

            string lcm = LynnwoodCoordinatesModel.StrDDM();
            string[] splitLatAndLon = lcm.ToString().Split(CommaSymbol);
            string lcmLattitude = splitLatAndLon[0];
            string lcmLongitude = splitLatAndLon[1];

            string actualLattitudeResult = ConversionHelper.GetNSEW(lcmLattitude);
            string actualLongitudeResult = ConversionHelper.GetNSEW(lcmLongitude);
            string actualCombinedResult = ConversionHelper.GetNSEW(lcm);

            Assert.AreEqual(expectedLattitudeResult, actualLattitudeResult);
            Assert.AreEqual(expectedLongitudeResult, actualLongitudeResult);
            Assert.AreEqual(expectedCombinedResult, actualCombinedResult);
        }

        [Test()]
        public void GetNSEW2_Test1()
        {
            string expectedNorthResult = "N";
            string expectedEastResult = "E";
            string expectedNonResult = "N";

            var munich = new MunichCoordinatesModel();

            string actualNorthResult = ConversionHelper.GetNSEW(munich.DegreesLat, 1);
            string actualEastResult = ConversionHelper.GetNSEW(munich.DegreesLon, 2);
            string actualLattitudeNonResult = ConversionHelper.GetNSEW(0, 1);

            Assert.AreEqual(expectedNorthResult, actualNorthResult);
            Assert.AreEqual(expectedEastResult, actualEastResult);
            Assert.AreEqual(expectedNonResult, actualLattitudeNonResult);
        }

        [Test()]
        public void GetNSEW2_Test2()
        {
            string expectedSouthResult = "S";
            string expectedWestResult = "W";
            string expectedNonResult = "E";

            var montevideo = new MontevideoCoordinateModel();

            string actualSouthResult = ConversionHelper.GetNSEW(montevideo.DegreesLat, 1);
            string actualWestResult = ConversionHelper.GetNSEW(montevideo.DegreesLon, 2);
            string actualLongitudeNonResult = ConversionHelper.GetNSEW(0, 2);

            Assert.AreEqual(expectedSouthResult, actualSouthResult);
            Assert.AreEqual(expectedWestResult, actualWestResult);
            Assert.AreEqual(expectedNonResult, actualLongitudeNonResult);
        }

        [Test()]
        public void IsValidTest()
        {
            decimal lattitude = 48.1467m;
            decimal longitude = 11.6083m;

            bool expectedResult = true;
            bool actualResult = ConversionHelper.IsValidLatDegreesAndLonDegrees(lattitude, longitude);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void IsNotValidTest_Lat()
        {
            decimal lattitude = 93.1467m;
            decimal longitude = 11.6083m;

            bool expectedResult = false;
            bool actualResult = ConversionHelper.IsValidLatDegreesAndLonDegrees(lattitude, longitude);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void IsNotValidTest_Lon()
        {
            decimal lattitude = 48.1467m;
            decimal longitude = 191.6083m;

            bool expectedResult = false;
            bool actualResult = ConversionHelper.IsValidLatDegreesAndLonDegrees(lattitude, longitude);

            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test()]
        public void IsNotValidTest_NegativeLat()
        {
            decimal lattitude = -93.1467m;
            decimal longitude = 11.6083m;

            bool expectedResult = false;
            bool actualResult = ConversionHelper.IsValidLatDegreesAndLonDegrees(lattitude, longitude);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void IsNotValidTest_NegativeLon()
        {
            decimal lattitude = 48.1467m;
            decimal longitude = -180.6083m;

            bool expectedResult = false;
            bool actualResult = ConversionHelper.IsValidLatDegreesAndLonDegrees(lattitude, longitude);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void LatDecimalIsValid_PassingPosAndNeg()
        {
            decimal positiveLat = 48.1467m;
            decimal negativeLat = -48.1467m;
            decimal zeroLat = 0.0m;

            bool expectedResult = true;
            bool positiveLatActualResult = ConversionHelper.LatDecimalIsValid(positiveLat);
            bool negativeLatACtualResult = ConversionHelper.LatDecimalIsValid(negativeLat);
            bool zeroLatActualResult = ConversionHelper.LatDecimalIsValid(zeroLat);

            Assert.AreEqual(expectedResult, positiveLatActualResult);
            Assert.AreEqual(expectedResult, negativeLatACtualResult);
            Assert.AreEqual(expectedResult, zeroLatActualResult);
        }

        [Test()]
        public void LonDecimalIsValid_PassingPosAndNeg()
        {
            decimal positiveLon = 179.1467m;
            decimal negativeLon = -179.1467m;
            decimal zeroLon = 0.0m;

            bool expectedResult = true;
            bool positiveLonActualResult = ConversionHelper.LonDecimalIsValid(positiveLon);
            bool negativeLonActualResult = ConversionHelper.LonDecimalIsValid(negativeLon);
            bool zeroLonActualResult = ConversionHelper.LonDecimalIsValid(zeroLon);

            Assert.AreEqual(expectedResult, positiveLonActualResult);
            Assert.AreEqual(expectedResult, negativeLonActualResult);
            Assert.AreEqual(expectedResult, zeroLonActualResult);
        }

        [Test()]
        public void LatDecimalIsValid_FalseScenarios()
        {
            decimal test1 = 90.000001m;
            decimal test2 = 90.1m;
            decimal test3 = 91.0m;
            decimal test4 = -90.000001m;
            decimal test5 = -90.1m;
            decimal test6 = -91.0m;

            bool expectedResult = false;

            bool test1Actual = ConversionHelper.LatDecimalIsValid(test1);
            bool test2Actual = ConversionHelper.LatDecimalIsValid(test2);
            bool test3Actual = ConversionHelper.LatDecimalIsValid(test3);
            bool test4Actual = ConversionHelper.LatDecimalIsValid(test4);
            bool test5Actual = ConversionHelper.LatDecimalIsValid(test5);
            bool test6Actual = ConversionHelper.LatDecimalIsValid(test6);

            Assert.AreEqual(expectedResult, test1Actual);
            Assert.AreEqual(expectedResult, test2Actual);
            Assert.AreEqual(expectedResult, test3Actual);
            Assert.AreEqual(expectedResult, test4Actual);
            Assert.AreEqual(expectedResult, test5Actual);
            Assert.AreEqual(expectedResult, test6Actual);
        }

        [Test()]
        public void LonDecimalIsValid_FalseScenarios()
        {
            decimal test1 = 180.000001m;
            decimal test2 = 180.1m;
            decimal test3 = 181.0m;
            decimal test4 = -180.000001m;
            decimal test5 = -180.1m;
            decimal test6 = -181.0m;

            bool expectedResult = false;

            bool test1Actual = ConversionHelper.LonDecimalIsValid(test1);
            bool test2Actual = ConversionHelper.LonDecimalIsValid(test2);
            bool test3Actual = ConversionHelper.LonDecimalIsValid(test3);
            bool test4Actual = ConversionHelper.LonDecimalIsValid(test4);
            bool test5Actual = ConversionHelper.LonDecimalIsValid(test5);
            bool test6Actual = ConversionHelper.LonDecimalIsValid(test6);

            Assert.AreEqual(expectedResult, test1Actual);
            Assert.AreEqual(expectedResult, test2Actual);
            Assert.AreEqual(expectedResult, test3Actual);
            Assert.AreEqual(expectedResult, test4Actual);
            Assert.AreEqual(expectedResult, test5Actual);
            Assert.AreEqual(expectedResult, test6Actual);
        }

        [Test()]
        public void GetLatDegrees_North_Pass()
        {
            var LTH = new LookupTablesHelper();
            decimal actualResult;
            short actualLatDirection;
            decimal expectedResult = 40;
            short expectedLatDirection = 1;

            if (LTH.GenerateTableLookups())
            {
                string grid = "CN87ut";
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

        [Test()]
        public void GetLatDegrees_South_Pass()
        {
            var LTH = new LookupTablesHelper();
            decimal actualResult;
            short actualLatDirection;
            decimal expectedResult = -120;
            short expectedLatDirection = -1;

            if (LTH.GenerateTableLookups())
            {
                string grid = "CN87ut";
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

        [Test()]
        public void AddLatDegrees_North_Pass()
        {
            decimal startingLat = 40.0m;
            int latDir = 1;
            string grid = "CN87ut";

            decimal expectedResult = 47m;

            decimal actualResult = ConversionHelper.AddLatDegreesRemainder(startingLat, latDir, grid);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void AddLonDegrees_West_Pass()
        {
            decimal startingLat = -120.0m;
            int latDir = -1;
            string grid = "CN87ut";

            decimal expectedResult = -122.0m;

            decimal actualResult = ConversionHelper.AddLonDegreesRemainder(startingLat, latDir, grid);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void GetLatMinutes_North_Pass()
        {
            var LTH = new LookupTablesHelper();
            decimal startingLat = 47.0m;
            int latDir = 1;
            string grid = "CN87ut";

            decimal actualAdjLatDegrees;
            decimal actualLatMinutesResult;

            decimal expectedAdjLatDegrees = 47.0m;
            decimal expectedResult = 48.75m;

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

        [Test()]
        public void GetLonMinutes_West_Pass()
        {
            var LTH = new LookupTablesHelper();
            decimal startingLon = -122.0m;
            int lonDir = -1;
            string grid = "CN87ut";

            decimal actualAdjLonDegrees;
            decimal actualResult;

            decimal expectedAdjLatDegrees = -122.0m;
            decimal expectedLonMinutes = 17.5m;

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

        [Test()]
        public void GetNearestEvenMultiple_Lat_Pass()
        {
            decimal minutesInput = 49.52m;

            decimal expectedResult = 50.0m;

            decimal actualResult = ConversionHelper.GetNearestEvenMultiple(minutesInput, 1);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void GetNearestEvenMultiple_NegativeLat_Pass()
        {
            decimal minutesInput = -54.60m;

            decimal expectedResult = -52.5m;

            decimal actualResult = ConversionHelper.GetNearestEvenMultiple(minutesInput, 1);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void GetNearestEvenMultiple_Lat_ZeroIsZero()
        {
            decimal minutesInput = 0.0m;

            decimal expectedResult = 0.0m;

            decimal actualResult = ConversionHelper.GetNearestEvenMultiple(minutesInput, 1);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void GetNearestEvenMultiple_Lon_Pass()
        {
            decimal minutesInput = 36.50m;

            decimal expectedResult = 40.0m;

            decimal actualResult = ConversionHelper.GetNearestEvenMultiple(minutesInput, 2);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void GetNearestEvenMultiple_NegativeLon_Pass()
        {
            decimal minutesInput = -17.60m;

            decimal expectedResult = -15.0m;

            decimal actualResult = ConversionHelper.GetNearestEvenMultiple(minutesInput, 2);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void GetNearestEvenMultiple_ZeroIsZero()
        {
            decimal minutesInput = 0.0m;

            decimal expectedResult = 0.0m;

            decimal actualResult = ConversionHelper.GetNearestEvenMultiple(minutesInput, 2);

            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}
