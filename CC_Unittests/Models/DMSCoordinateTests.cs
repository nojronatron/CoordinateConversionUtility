using CoordinateConversionUtility_UnitTests.TestModels;
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CoordinateConversionUtility.Models.Tests
{
    [TestFixture()]
    public class DMSCoordinateTests : UnitTestsBase
    {
        [Test()]
        public void DefaultCtorCreatesInvalidDMSCoordinate_Test()
        {
            var dms = new DMSCoordinate();
            var expectedResult = false;
            var expectedLatDegrees = 0m;
            var expectedLatMins = 0m;
            var expectedLatSecs = 0m;
            var expectedLonDegrees = 0m;
            var expectedLonMins = 0m;
            var expectedLonSecs = 0m;

            var actualResult = dms.IsValid;
            var actualLatDegrees = dms.GetShortDegreesLat();
            var actualLatMins = dms.GetMinsLat();
            var actualLatSecs = dms.GetSecondsLattitude();
            var actualLonDegrees = dms.GetShortDegreesLon();
            var actualLonMins = dms.GetMinsLon();
            var actualLonSecs = dms.GetSecondsLongitude();

            Assert.AreEqual(expectedLatDegrees, actualLatDegrees);
            Assert.AreEqual(expectedLatMins, actualLatMins);
            Assert.AreEqual(expectedLatSecs, actualLatSecs);
            Assert.AreEqual(expectedLonDegrees, actualLonDegrees);
            Assert.AreEqual(expectedLonMins, actualLonMins);
            Assert.AreEqual(expectedLonSecs, actualLonSecs);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void ValidateIsSecsOrMinsTest0()
        {
            string testInput = "0";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = true;

            bool actualResult = DMSCoordinate.ValidateIsMinutes(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [Test()]
        public void ValidateIsSecsOrMinsTest60()
        {
            string testInput = "60";
            decimal expectedOutResult = 60.0m;
            bool expectedResult = true;

            bool actualResult = DMSCoordinate.ValidateIsSeconds(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [Test()]
        public void ValidateIsSecsOrMinsTestNeg61()
        {
            string testInput = "-61";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = false;

            bool actualResult = DMSCoordinate.ValidateIsMinutes(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [Test()]
        public void ValidateIsSecsOrMinsTestNeg59()
        {
            string testInput = "-50";
            decimal expectedOutResult = -50.0m;
            bool expectedResult = true;

            bool actualResult = DMSCoordinate.ValidateIsSeconds(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [Test()]
        public void ValidateIsSecsOrMinsTest10000()
        {
            string testInput = "10000";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = false;

            bool actualResult = DMSCoordinate.ValidateIsMinutes(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }


        [Test()]
        public void ValidateSeconds()
        {
            bool expectedResultSecs = true;
            decimal expectedOutputSecs = 34.21m;

            string strSeconds = "34.21";

            bool actualResultSecs = DMSCoordinate.ValidateIsSeconds(strSeconds, out decimal actualOutputSecs);

            Assert.AreEqual(expectedResultSecs, actualResultSecs);
            Assert.AreEqual(expectedOutputSecs, actualOutputSecs);
        }

        [Test()]
        public void CTOR_DDtoDMS_NW_Test()
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

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff },
                { "latSecsDiff", latSecsDiff },
                { "lonSecsDiff", lonSecsDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);
            Assert.IsTrue(latSecsDiff >= 0 && latSecsDiff <= 1.0m);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
            Assert.IsTrue(lonSecsDiff >= 0 && lonSecsDiff <= 1.0m);
        }

        [Test()]
        public void CTOR_DDtoDMS_NE_Test()
        {
            var mcm = new MunichCoordinatesModel();
            decimal ddLat = mcm.DegreesLat;
            decimal ddLon = mcm.DegreesLon;

            var dms = new DMSCoordinate(ddLat, ddLon);

            string expectedResult = LynnwoodCoordinatesModel.StrDMS();
            string actualResult = dms.ToString();

            decimal latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(mcm.DegreesLat));
            decimal lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(mcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(mcm.DdmMinsLat));
            decimal lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(mcm.DdmMinsLon));

            decimal latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - mcm.DmsSecondsLat);
            decimal lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - mcm.DmsSecondsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff },
                { "latSecsDiff", latSecsDiff },
                { "lonSecsDiff", lonSecsDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);
            Assert.IsTrue(latSecsDiff >= 0 && latSecsDiff <= 1.0m);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
            Assert.IsTrue(lonSecsDiff >= 0 && lonSecsDiff <= 1.0m);
        }

        [Test()]
        public void CTOR_DDtoDMS_SE_Test()
        {
            var wcm = new WellingtonCoordinateModel();
            decimal ddLat = wcm.DegreesLat;
            decimal ddLon = wcm.DegreesLon;

            var dms = new DMSCoordinate(ddLat, ddLon);

            string expectedResult = LynnwoodCoordinatesModel.StrDMS();
            string actualResult = dms.ToString();

            decimal latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(wcm.DegreesLat));
            decimal lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(wcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(wcm.DdmMinsLat));
            decimal lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(wcm.DdmMinsLon));

            decimal latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - wcm.DmsSecondsLat);
            decimal lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - wcm.DmsSecondsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff },
                { "latSecsDiff", latSecsDiff },
                { "lonSecsDiff", lonSecsDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);
            Assert.IsTrue(latSecsDiff >= 0 && latSecsDiff <= 1.0m);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
            Assert.IsTrue(lonSecsDiff >= 0 && lonSecsDiff <= 1.0m);
        }

        [Test()]
        public void CTOR_DDtoDMS_SW_Test()
        {
            var mvcm = new MontevideoCoordinateModel();
            decimal ddLat = mvcm.DegreesLat;
            decimal ddLon = mvcm.DegreesLon;

            var dms = new DMSCoordinate(ddLat, ddLon);

            string expectedResult = LynnwoodCoordinatesModel.StrDMS();
            string actualResult = dms.ToString();

            decimal latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(mvcm.DegreesLat));
            decimal lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(mvcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(mvcm.DdmMinsLat));
            decimal lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(mvcm.DdmMinsLon));

            decimal latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - mvcm.DmsSecondsLat);
            decimal lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - mvcm.DmsSecondsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff },
                { "latSecsDiff", latSecsDiff },
                { "lonSecsDiff", lonSecsDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);
            Assert.IsTrue(latSecsDiff >= 0 && latSecsDiff <= 1.0m);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
            Assert.IsTrue(lonSecsDiff >= 0 && lonSecsDiff <= 1.0m);
        }

        [Test()]
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

                var dict = new Dictionary<string, decimal>
                {
                    { "latDiff", latDiff },
                    { "lonDiff", lonDiff },
                    { "latMinsDiff", latMinsDiff },
                    { "lonMinsDiff", lonMinsDiff },
                    { "latSecsDiff", latSecsDiff },
                    { "lonSecsDiff", lonSecsDiff }
                };
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

        [Test()]
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

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff },
                { "latSecsDiff", latSecsDiff },
                { "lonSecsDiff", lonSecsDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);
            Assert.IsTrue(latSecsDiff >= 0 && latSecsDiff <= 1.0m);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
            Assert.IsTrue(lonSecsDiff >= 0 && lonSecsDiff <= 1.0m);
        }

        [Test()]
        public void CTOR_DmsStringToDMS_Test()
        {
            string expectedResult = SanClementeCoordinatesModel.StrDMS();

            var dms = new DMSCoordinate(expectedResult);
            string actualResult = dms.ToString();

            DisplayOutput(expectedResult, actualResult, new Dictionary<string, decimal>());

            int expectedCount = expectedResult.Length;
            int actualCount = actualResult.Length;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test()]
        public void GetShortMinutesLattitudeTest()
        {
            var mcm = new MunichCoordinatesModel();
            var dms = new DMSCoordinate(mcm.DegreesLat, mcm.DegreesLon);

            decimal shortMinutesLat = dms.GetShortMinutesLattitude();

            decimal minutesLatDiff = Math.Abs(Math.Truncate(mcm.DdmMinsLat) - shortMinutesLat);

            var dict = new Dictionary<string, decimal>
            {
                { "minutesLatDiff", minutesLatDiff }
            };

            DisplayOutput("49", shortMinutesLat.ToString(), dict);

            Assert.IsTrue(minutesLatDiff <= 0.1m);
        }

        [Test()]
        public void GetShortMinutesLongitudeTest()
        {
            var wcm = new WellingtonCoordinateModel();
            var dms = new DMSCoordinate(wcm.DegreesLat, wcm.DegreesLon);

            decimal shortMinutesLon = dms.GetShortMinutesLongitude();
            decimal minutesLonDiff = Math.Abs(Math.Truncate(wcm.DdmMinsLon) - shortMinutesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "minutesLonDiff", minutesLonDiff }
            };

            DisplayOutput("17", shortMinutesLon.ToString(), dict);

            Assert.IsTrue(minutesLonDiff <= 0.1m);
        }

        [Test()]
        public void GetSecondsLattitude()
        {
            var mvcm = new MontevideoCoordinateModel();
            var dms = new DMSCoordinate(mvcm.DegreesLat, mvcm.DegreesLon);

            decimal expectedSecLats = dms.GetSecondsLattitude();
            decimal secondsLat = dms.GetSecondsLattitude();
            decimal secondsLatDiff = Math.Abs(Math.Truncate(mvcm.DmsSecondsLat) - secondsLat);

            var dict = new Dictionary<string, decimal>
            {
                { "secondsLatDiff", 0m }
            };
            DisplayOutput(expectedSecLats.ToString(), secondsLat.ToString(), dict);

            Assert.IsTrue(secondsLatDiff <= 1m);
        }

        [Test()]
        public void GetSecondsLongitude()
        {
            var sccm = new SanClementeCoordinatesModel();
            var dms = new DMSCoordinate(sccm.DegreesLat, sccm.DegreesLon);

            decimal expectedSecLons = dms.GetSecondsLongitude();
            decimal secondsLon = dms.GetSecondsLongitude();
            decimal secondsLonDiff = Math.Abs(Math.Truncate(sccm.DmsSecondsLon) - secondsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "secondsLonDiff", 0m }
            };
            DisplayOutput(expectedSecLons.ToString(), secondsLon.ToString(), dict);

            Assert.IsTrue(secondsLonDiff <= 1m);
        }

        [Test()]
        public void IsValid_90_180_Passes_Test()
        {
            decimal lattitude = 90.0m;
            decimal longitude = 180.0m;
            bool expectedResult = true;

            var dms = new DMSCoordinate(lattitude, longitude);
            bool actualResult = dms.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void IsValid_InvalidLat_Test()
        {
            decimal lattitude = 91.0m;
            decimal longitude = 180.0m;
            bool expectedResult = false;

            var dms = new DMSCoordinate(lattitude, longitude);
            bool actualResult = dms.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
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