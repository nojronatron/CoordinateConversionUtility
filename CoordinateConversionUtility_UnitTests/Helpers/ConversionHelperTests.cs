using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoordinateConversionUtility;
using CoordinateConversionUtility_UnitTests.TestModels;

namespace CoordinateConversionUtility.Tests
{
    [TestClass()]
    public class ConversionHelperTests
    {
        public WellingtonCoordinateModel wcm = new WellingtonCoordinateModel();
        public WashingtondcCoordinateModel wdccm = new WashingtondcCoordinateModel();
        public SanClementeCoordinatesModel sccm = new SanClementeCoordinatesModel();
        public MunichCoordinatesModel mcm = new MunichCoordinatesModel();
        public MontevideoCoordinateModel mvcm = new MontevideoCoordinateModel();
        public LynnwoodCoordinatesModel lcm = new LynnwoodCoordinatesModel();

        [TestMethod()]
        public void Test_RollupDdmMinsToDecimalDegrees()
        {
            decimal decDegrees = 45.0000m;
            decimal decMinutes = 30m;
            decimal expectedResult = 45.5000m;
            decimal actualResult = ConversionHelper.RollupDdmMinsToDecimalDegrees(decDegrees, decMinutes);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_RollupDmsSecsToDdmMinutes()
        {
            decimal decMinutes = 24.00m;
            decimal decSeconds = 30m;
            decimal expectedResult = 24.50m;
            decimal actualResult = ConversionHelper.RollupDmsSecsToDdmMinutes(decMinutes, decSeconds);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_DDMtoDD()
        {   //  Expected:<47.8334°, -122.2719°>. Actual:<47.833666666666666666666666667°, -122.27183333333333333333333333°>.
            DDMCoordinateHelper Lynnwood = new DDMCoordinateHelper(lcm.CalculatedDDM());
            string expectedResult = lcm.GoogleMapsDD();
            string actualResult = ConversionHelper.ToDD(Lynnwood);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_DMStoDD()
        {   //  Expected:<47.8334°, -122.2719°>. Actual:<47.8337°, -122.2670°>.
            DMSCoordinateHelper Lynnwood = new DMSCoordinateHelper(lcm.GoogleMapsDMS());
            string expectedResult = lcm.GoogleMapsDD();
            string actualResult = ConversionHelper.ToDD(Lynnwood);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_DDToDDM()
        {   //  Expected:<47°50.02'N, 122°16.31'W>. Actual:<47°50.00' N, 122°16.31' W>.
            DDCoordindateHelper Lynnwood = new DDCoordindateHelper(lcm.DegreesLat(), lcm.DegreesLon());
            string expectedResult = lcm.CalculatedDDM();
            string actualResult = ConversionHelper.ToDDM(Lynnwood);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_DMSToDDM()
        {   //  Expected:<47°50.02'N, 122°16.31'W>. Actual:<47°50.00' N, 122°16.00' W>.
            DMSCoordinateHelper Lynnwood = new DMSCoordinateHelper(lcm.GoogleMapsDMS());
            string expectedResult = lcm.CalculatedDDM();
            string actualResult = ConversionHelper.ToDDM(Lynnwood);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_DDToDMS()
        {   //  Expected:<N 47°50'1.20", W 122°16'18.60">. Actual:<N 47°50'0", W 122°16'18">.
            DDCoordindateHelper Lynnwood = new DDCoordindateHelper(lcm.DegreesLat(), lcm.DegreesLon());
            string expectedResult = lcm.GoogleMapsDMS();
            string actualResult = ConversionHelper.ToDMS(Lynnwood);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_DDMToDMS()
        {   //  Expected:<47°50.02'N, 122°16.31'W>. Actual:<N 47°50'1", S 122°16'18">
            DDMCoordinateHelper Lynnwood = new DDMCoordinateHelper(lcm.CalculatedDDM());
            string expectedResult = lcm.CalculatedDDM();
            string actualResult = ConversionHelper.ToDMS(Lynnwood);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetNSEW_North()
        {
            string expectedResult = "N";
            string actualResult = ConversionHelper.GetNSEW(1, 1);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetNSEW_East()
        {
            string expectedResult = "E";
            string actualResult = ConversionHelper.GetNSEW(1, 2);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetNSEW_South()
        {
            string expectedResult = "S";
            string actualResult = ConversionHelper.GetNSEW(-1, 1);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetNSEW_West()
        {
            string expectedResult = "W";
            string actualResult = ConversionHelper.GetNSEW(-1, 2);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetNSEW_InvalidLatLonID()
        {
            string expectedResult = string.Empty;
            string actualResult = ConversionHelper.GetNSEW(1, 5);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetNSEW_InvalidDegrees()
        {
            string expectedResult = string.Empty;
            string actualResult = ConversionHelper.GetNSEW(-190, 1);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetMinutesLatAlpha()
        {   //  dd.1225 = 7.35'
            decimal expectedResult = 7.35m;
            decimal actualResult = ConversionHelper.GetMinutesLat(12.1225m);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetMinutesLonAlpha()
        {   //  dd.1350 = 8.10'
            decimal expectedResult = 8.10m;
            decimal actualResult = ConversionHelper.GetMinutesLon(123.1350m);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetSecondsLatAlpha()
        {   //  m.35' = 21.0"
            decimal expectedResult = 21.0m;
            decimal actualResult = ConversionHelper.GetSecondsLat(0.35m);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetSecondsLonAlpha()
        {   //  m.10' = 6.0"
            decimal expectedResult = 6.0m;
            decimal actualResult = ConversionHelper.GetSecondsLon(0.1m);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod()]
        public void Test_GetMinutesRoundsToThreeDecimalPoints()
        {   //  dd.1225 = 7.35'
            decimal expectedResult = 27.996m;
            decimal actualResult = ConversionHelper.GetMinutesLat(12.4666m);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetMinutesLonRoundsToThreeDecimalPoints()
        {   //  dd.9999 = 59.994'
            //  Expected:<59.995>. Actual:<59.994960>.
            decimal expectedResult = 59.995m;
            decimal actualResult = ConversionHelper.GetMinutesLon(123.999916m);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetSecondsLatNearSixtyMinutes()
        {   //  m.99' = 59.4"
            decimal expectedResult = 59.4m;
            decimal actualResult = ConversionHelper.GetSecondsLat(0.99m);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetSecondsLonNearZeroMinutes()
        {   //  m.01' = 0.6"
            decimal expectedResult = 0.6m;
            decimal actualResult = ConversionHelper.GetSecondsLon(0.01m);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetMinutesNegativeNumber()
        {   //  Expected:<0>. Actual:<7.3500>.
            decimal expectedResult = 0m;
            decimal actualResult = ConversionHelper.GetMinutesLat(-0.1225m);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetMinutesLonSixtyMinutes()
        {   //  returns minutes and throws away degrees
            decimal expectedResult = 0m;
            decimal actualResult = ConversionHelper.GetMinutesLat(60.0m);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod()]
        public void Test_GetMinutesLonSixtyOneMinutes()
        {   //  Expected:<1>. Actual:<0.0>. 
            decimal expectedResult = 1m;
            decimal actualResult = ConversionHelper.GetMinutesLon(61.0m);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetSecondsLatZeroMinutes()
        {   //  zero in zero out
            decimal expectedResult = 0m;
            decimal actualResult = ConversionHelper.GetSecondsLat(0.00m);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Test_GetSecondsLonNegativeMinutes()
        {   //  Expected:<0>. Actual:<0.60>. 
            decimal expectedResult = 0m;
            decimal actualResult = ConversionHelper.GetSecondsLon(-0.01m);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}