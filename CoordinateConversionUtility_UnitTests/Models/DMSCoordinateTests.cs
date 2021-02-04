using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using CoordinateConversionUtility_UnitTests.TestModels;

namespace CoordinateConversionUtility.Models.Tests
{
    [TestClass()]
    public class DMSCoordinateTests : UnitTestsBase
    {
        [TestMethod()]
        public void CTOR_DDtoDMS_Test()
        {
            var lcm = new LynnwoodCoordinatesModel();
            decimal ddLat = lcm.DegreesLat;
            decimal ddLon = lcm.DegreesLon;

            DMSCoordinate dms = new DMSCoordinate(ddLat, ddLon);

            string expectedResult = LynnwoodCoordinatesModel.strDMS();
            string actualResult = dms.ToString();

            var latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(lcm.DegreesLat));
            var lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(lcm.DegreesLon));

            var latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(lcm.DdmMinsLat));
            var lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(lcm.DdmMinsLon));

            var latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - lcm.DmsSecondsLat);
            var lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - lcm.DmsSecondsLon);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            dict.Add("latMinsDiff", latMinsDiff);
            dict.Add("lonMinsDiff", lonMinsDiff);
            dict.Add("latSecsDiff", latSecsDiff);
            dict.Add("lonSecsDiff", lonSecsDiff);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);
            Assert.IsTrue(latSecsDiff >= 0 && latSecsDiff <= 1.0m);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
            Assert.IsTrue(lonSecsDiff >= 0 && lonSecsDiff <= 1.0m);
        }

        [TestMethod()]
        public void CTOR_DDMtoDMS_Test()
        {
            var lcm = new LynnwoodCoordinatesModel();
            DMSCoordinate dms = new DMSCoordinate(lcm.ShortDegreesLattitude(), lcm.DdmMinsLat, lcm.ShortDegreesLongitude(), lcm.DdmMinsLon);

            string expectedResult = LynnwoodCoordinatesModel.strDMS();
            string actualResult = dms.ToString();

            if (expectedResult != actualResult)
            {
                var latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(lcm.DegreesLat));
                var lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(lcm.DegreesLon));

                var latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(lcm.DdmMinsLat));
                var lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(lcm.DdmMinsLon));

                var latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - lcm.DmsSecondsLat);
                var lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - lcm.DmsSecondsLon);

                Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
                dict.Add("latDiff", latDiff);
                dict.Add("lonDiff", lonDiff);
                dict.Add("latMinsDiff", latMinsDiff);
                dict.Add("lonMinsDiff", lonMinsDiff);
                dict.Add("latSecsDiff", latSecsDiff);
                dict.Add("lonSecsDiff", lonSecsDiff);
                DisplayOutput(expectedResult, actualResult, dict);

                Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
                Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);
                Assert.IsTrue(latSecsDiff >= 0 && latSecsDiff <= 1.0m);

                Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
                Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
                Assert.IsTrue(lonSecsDiff >= 0 && lonSecsDiff <= 1.0m);
            }
            else
            {
                Assert.AreEqual(expectedResult, actualResult);
            }

        }

        [TestMethod()]
        public void CTOR_DmsToDMS_Test()
        {
            var lcm = new LynnwoodCoordinatesModel();
            DMSCoordinate dms = new DMSCoordinate(
                lcm.DegreesLat, Math.Truncate(lcm.DdmMinsLat), lcm.DmsSecondsLat, 
                lcm.DegreesLon, Math.Truncate(lcm.DdmMinsLon), lcm.DmsSecondsLon);

            var expectedResult = LynnwoodCoordinatesModel.strDMS();
            var actualResult = dms.ToString();

            var latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(lcm.DegreesLat));
            var lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(lcm.DegreesLon));

            var latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(lcm.DdmMinsLat));
            var lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(lcm.DdmMinsLon));

            var latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - lcm.DmsSecondsLat);
            var lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - lcm.DmsSecondsLon);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            dict.Add("latMinsDiff", latMinsDiff);
            dict.Add("lonMinsDiff", lonMinsDiff);
            dict.Add("latSecsDiff", latSecsDiff);
            dict.Add("lonSecsDiff", lonSecsDiff);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);
            Assert.IsTrue(latSecsDiff >= 0 && latSecsDiff <= 1.0m);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
            Assert.IsTrue(lonSecsDiff >= 0 && lonSecsDiff <= 1.0m);
        }

        [TestMethod()]
        public void CTOR_DmsStringToDMS_Test()
        {
            var lcm = new LynnwoodCoordinatesModel();
            string expectedResult = LynnwoodCoordinatesModel.strDMS();

            DMSCoordinate dms = new DMSCoordinate(expectedResult);
            string actualResult = dms.ToString();

            DisplayOutput(expectedResult, actualResult, new Dictionary<string, decimal>());

            int expectedCount = expectedResult.Length;
            int actualCount = actualResult.Length;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod()]
        public void GetShortMinutesLattitudeTest()
        {
            var lcm = new LynnwoodCoordinatesModel();
            DMSCoordinate dms = new DMSCoordinate(lcm.DegreesLat, lcm.DegreesLon);

            decimal shortMinutesLat = dms.GetShortMinutesLattitude();

            decimal minutesLatDiff = Math.Abs(Math.Truncate(lcm.DdmMinsLat) - shortMinutesLat);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("minutesLatDiff", minutesLatDiff);

            DisplayOutput("49", shortMinutesLat.ToString(), dict);

            Assert.IsTrue(minutesLatDiff <= 0.1m);
        }

        [TestMethod()]
        public void GetShortMinutesLongitudeTest()
        {
            var lcm = new LynnwoodCoordinatesModel();
            DMSCoordinate dms = new DMSCoordinate(lcm.DegreesLat, lcm.DegreesLon);

            decimal shortMinutesLon = dms.GetShortMinutesLongitude();
            decimal minutesLonDiff = Math.Abs(Math.Truncate(lcm.DdmMinsLon) - shortMinutesLon);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("minutesLonDiff", minutesLonDiff);

            DisplayOutput("17", shortMinutesLon.ToString(), dict);

            Assert.IsTrue(minutesLonDiff <= 0.1m);
        }

        [TestMethod()]
        public void GetSecondsLattitude()
        {
            var lcm = new LynnwoodCoordinatesModel();
            DMSCoordinate dms = new DMSCoordinate(lcm.DegreesLat, lcm.DegreesLon);

            var expectedSecLats = dms.GetSecondsLattitude();
            decimal secondsLat = dms.GetSecondsLattitude();
            decimal secondsLatDiff = Math.Abs(Math.Truncate(lcm.DmsSecondsLat) - secondsLat);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("secondsLatDiff", 0m);
            DisplayOutput(expectedSecLats.ToString(), secondsLat.ToString(), dict);

            Assert.IsTrue(secondsLatDiff <= 1m);
        }

        [TestMethod()]
        public void GetSecondsLongitude()
        {
            var lcm = new LynnwoodCoordinatesModel();
            DMSCoordinate dms = new DMSCoordinate(lcm.DegreesLat, lcm.DegreesLon);

            var expectedSecLons = dms.GetSecondsLongitude();
            decimal secondsLon = dms.GetSecondsLongitude();
            decimal secondsLonDiff = Math.Abs(Math.Truncate(lcm.DmsSecondsLon) - secondsLon);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("secondsLonDiff", 0m);
            DisplayOutput(expectedSecLons.ToString(), secondsLon.ToString(), dict);

            Assert.IsTrue(secondsLonDiff <= 1m);
        }

    }
}