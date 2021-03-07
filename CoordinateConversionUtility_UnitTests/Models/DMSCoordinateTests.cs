using CoordinateConversionUtility_UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CoordinateConversionUtility.Models.Tests
{
    [TestClass()]
    public class DMSCoordinateTests : UnitTestsBase
    {
        [TestMethod()]
        public void ValidateIsSecsOrMinsTest0()
        {
            string testInput = "0";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = true;

            bool actualResult = DMSCoordinate.ValidateIsMinutes(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsSecsOrMinsTest60()
        {
            string testInput = "60";
            decimal expectedOutResult = 60.0m;
            bool expectedResult = true;

            bool actualResult = DMSCoordinate.ValidateIsSeconds(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsSecsOrMinsTestNeg61()
        {
            string testInput = "-61";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = false;

            bool actualResult = DMSCoordinate.ValidateIsMinutes(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsSecsOrMinsTestNeg59()
        {
            string testInput = "-50";
            decimal expectedOutResult = -50.0m;
            bool expectedResult = true;

            bool actualResult = DMSCoordinate.ValidateIsSeconds(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsSecsOrMinsTest10000()
        {
            string testInput = "10000";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = false;

            bool actualResult = DMSCoordinate.ValidateIsMinutes(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }


        [TestMethod()]
        public void ValidateSeconds()
        {
            bool expectedResultSecs = true;
            decimal expectedOutputSecs = 34.21m;

            string strSeconds = "34.21";

            bool actualResultSecs = DMSCoordinate.ValidateIsSeconds(strSeconds, out decimal actualOutputSecs);

            Assert.AreEqual(expectedResultSecs, actualResultSecs);
            Assert.AreEqual(expectedOutputSecs, actualOutputSecs);
        }

        [TestMethod()]
        public void CTOR_DDtoDMS_Test()
        {
            var sccm = new SanClementeCoordinatesModel();
            decimal ddLat = sccm.DegreesLat;
            decimal ddLon = sccm.DegreesLon;

            var dms = new DMSCoordinate(ddLat, ddLon);

            string expectedResult = LynnwoodCoordinatesModel.StrDMS();
            string actualResult = dms.ToString();

            decimal latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(sccm.DegreesLat));
            decimal lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(sccm.DegreesLon));

            decimal latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(sccm.DdmMinsLat));
            decimal lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(sccm.DdmMinsLon));

            decimal latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - sccm.DmsSecondsLat);
            decimal lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - sccm.DmsSecondsLon);

            var dict = new Dictionary<string, decimal>();
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
        public void CTOR_DDtoDMS_Test()
        {
            var sccm = new SanClementeCoordinatesModel();
            decimal ddLat = sccm.DegreesLat;
            decimal ddLon = sccm.DegreesLon;

            var dms = new DMSCoordinate(ddLat, ddLon);

            string expectedResult = LynnwoodCoordinatesModel.StrDMS();
            string actualResult = dms.ToString();

            decimal latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(sccm.DegreesLat));
            decimal lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(sccm.DegreesLon));

            decimal latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(sccm.DdmMinsLat));
            decimal lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(sccm.DdmMinsLon));

            decimal latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - sccm.DmsSecondsLat);
            decimal lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - sccm.DmsSecondsLon);

            var dict = new Dictionary<string, decimal>();
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
        public void CTOR_DDtoDMS_Test()
        {
            var sccm = new SanClementeCoordinatesModel();
            decimal ddLat = sccm.DegreesLat;
            decimal ddLon = sccm.DegreesLon;

            var dms = new DMSCoordinate(ddLat, ddLon);

            string expectedResult = LynnwoodCoordinatesModel.StrDMS();
            string actualResult = dms.ToString();

            decimal latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(sccm.DegreesLat));
            decimal lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(sccm.DegreesLon));

            decimal latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(sccm.DdmMinsLat));
            decimal lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(sccm.DdmMinsLon));

            decimal latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - sccm.DmsSecondsLat);
            decimal lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - sccm.DmsSecondsLon);

            var dict = new Dictionary<string, decimal>();
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
        public void CTOR_DDtoDMS_Test()
        {
            var sccm = new SanClementeCoordinatesModel();
            decimal ddLat = sccm.DegreesLat;
            decimal ddLon = sccm.DegreesLon;

            var dms = new DMSCoordinate(ddLat, ddLon);

            string expectedResult = LynnwoodCoordinatesModel.StrDMS();
            string actualResult = dms.ToString();

            decimal latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(sccm.DegreesLat));
            decimal lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(sccm.DegreesLon));

            decimal latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(sccm.DdmMinsLat));
            decimal lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(sccm.DdmMinsLon));

            decimal latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - sccm.DmsSecondsLat);
            decimal lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - sccm.DmsSecondsLon);

            var dict = new Dictionary<string, decimal>();
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
            var dms = new DMSCoordinate(lcm.ShortDegreesLattitude(), lcm.DdmMinsLat, lcm.ShortDegreesLongitude(), lcm.DdmMinsLon);

            string expectedResult = LynnwoodCoordinatesModel.StrDMS();
            string actualResult = dms.ToString();

            if (expectedResult != actualResult)
            {
                decimal latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(lcm.DegreesLat));
                decimal lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(lcm.DegreesLon));

                decimal latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(lcm.DdmMinsLat));
                decimal lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(lcm.DdmMinsLon));

                decimal latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - lcm.DmsSecondsLat);
                decimal lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - lcm.DmsSecondsLon);

                var dict = new Dictionary<string, decimal>();
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
            var dms = new DMSCoordinate(
                lcm.DegreesLat, Math.Truncate(lcm.DdmMinsLat), lcm.DmsSecondsLat,
                lcm.DegreesLon, Math.Truncate(lcm.DdmMinsLon), lcm.DmsSecondsLon);

            string expectedResult = LynnwoodCoordinatesModel.StrDMS();
            string actualResult = dms.ToString();

            decimal latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(lcm.DegreesLat));
            decimal lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(lcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(lcm.DdmMinsLat));
            decimal lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(lcm.DdmMinsLon));

            decimal latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - lcm.DmsSecondsLat);
            decimal lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - lcm.DmsSecondsLon);

            var dict = new Dictionary<string, decimal>();
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
            string expectedResult = LynnwoodCoordinatesModel.StrDMS();

            var dms = new DMSCoordinate(expectedResult);
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
            var dms = new DMSCoordinate(lcm.DegreesLat, lcm.DegreesLon);

            decimal shortMinutesLat = dms.GetShortMinutesLattitude();

            decimal minutesLatDiff = Math.Abs(Math.Truncate(lcm.DdmMinsLat) - shortMinutesLat);

            var dict = new Dictionary<string, decimal>();
            dict.Add("minutesLatDiff", minutesLatDiff);

            DisplayOutput("49", shortMinutesLat.ToString(), dict);

            Assert.IsTrue(minutesLatDiff <= 0.1m);
        }

        [TestMethod()]
        public void GetShortMinutesLongitudeTest()
        {
            var lcm = new LynnwoodCoordinatesModel();
            var dms = new DMSCoordinate(lcm.DegreesLat, lcm.DegreesLon);

            decimal shortMinutesLon = dms.GetShortMinutesLongitude();
            decimal minutesLonDiff = Math.Abs(Math.Truncate(lcm.DdmMinsLon) - shortMinutesLon);

            var dict = new Dictionary<string, decimal>();
            dict.Add("minutesLonDiff", minutesLonDiff);

            DisplayOutput("17", shortMinutesLon.ToString(), dict);

            Assert.IsTrue(minutesLonDiff <= 0.1m);
        }

        [TestMethod()]
        public void GetSecondsLattitude()
        {
            var lcm = new LynnwoodCoordinatesModel();
            var dms = new DMSCoordinate(lcm.DegreesLat, lcm.DegreesLon);

            decimal expectedSecLats = dms.GetSecondsLattitude();
            decimal secondsLat = dms.GetSecondsLattitude();
            decimal secondsLatDiff = Math.Abs(Math.Truncate(lcm.DmsSecondsLat) - secondsLat);

            var dict = new Dictionary<string, decimal>();
            dict.Add("secondsLatDiff", 0m);
            DisplayOutput(expectedSecLats.ToString(), secondsLat.ToString(), dict);

            Assert.IsTrue(secondsLatDiff <= 1m);
        }

        [TestMethod()]
        public void GetSecondsLongitude()
        {
            var lcm = new LynnwoodCoordinatesModel();
            var dms = new DMSCoordinate(lcm.DegreesLat, lcm.DegreesLon);

            decimal expectedSecLons = dms.GetSecondsLongitude();
            decimal secondsLon = dms.GetSecondsLongitude();
            decimal secondsLonDiff = Math.Abs(Math.Truncate(lcm.DmsSecondsLon) - secondsLon);

            var dict = new Dictionary<string, decimal>();
            dict.Add("secondsLonDiff", 0m);
            DisplayOutput(expectedSecLons.ToString(), secondsLon.ToString(), dict);

            Assert.IsTrue(secondsLonDiff <= 1m);
        }

        [TestMethod()]
        public void IsValid_90_180_Passes_Test()
        {
            decimal lattitude = 90.0m;
            decimal longitude = 180.0m;
            bool expectedResult = true;

            var dms = new DMSCoordinate(lattitude, longitude);
            bool actualResult = dms.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void IsValid_InvalidLat_Test()
        {
            decimal lattitude = 91.0m;
            decimal longitude = 180.0m;
            bool expectedResult = false;

            var dms = new DMSCoordinate(lattitude, longitude);
            bool actualResult = dms.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void IsValid_InvalidLon_Test()
        {
            decimal lattitude = 90.0m;
            decimal longitude = -181.0m;
            bool expectedResult = false;

            var dms = new DMSCoordinate(lattitude, longitude);
            bool actualResult = dms.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}