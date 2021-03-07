using CoordinateConversionUtility_UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CoordinateConversionUtility.Models.Tests
{
    [TestClass()]
    public class DDMCoordinateTests : UnitTestsBase
    {

        [TestMethod()]
        public void ValidateMinutes()
        {
            bool expectedResultMins = true;
            decimal expectedOutputMins = 45.55m;

            string strMinutes = "45.55";

            bool actualresultMins = DDMCoordinate.ValidateIsMinutes(strMinutes, out decimal actualMinsOutput);

            Assert.AreEqual(expectedResultMins, actualresultMins);
            Assert.AreEqual(expectedOutputMins, actualMinsOutput);
        }
        
        [TestMethod()]
        public void DefaultCTOR_Test()
        {
            var ddmCoord = new DDMCoordinate();

            bool expectedResult = true;
            bool actualResult = ddmCoord.GetType().FullName == "CoordinateConversionUtility.Models.DDMCoordinate";

            Assert.IsTrue(expectedResult == actualResult);
        }

        [TestMethod()]
        public void CTOR_DD_Test()
        {
            var mvcm = new MontevideoCoordinateModel();
            decimal ddLat = mvcm.DegreesLat;
            decimal ddLon = mvcm.DegreesLon;

            var ddm = new DDMCoordinate(ddLat, ddLon);

            string expectedResult = MontevideoCoordinateModel.strDDM();
            string actualResult = ddm.ToString();

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(mvcm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(mvcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - mvcm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - mvcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void CTOR_DDM_Test()
        {
            var mvcm = new MontevideoCoordinateModel();
            decimal latDegrees = mvcm.DegreesLat;
            decimal latMins = mvcm.DdmMinsLat;
            decimal lonDegrees = mvcm.DegreesLon;
            decimal lonMins = mvcm.DdmMinsLon;

            var ddm = new DDMCoordinate(latDegrees, latMins, lonDegrees, lonMins);

            string expectedResult = MontevideoCoordinateModel.strDDM();
            string actualResult = ddm.ToString();

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(mvcm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(mvcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - mvcm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - mvcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void CTOR_DMS_Test()
        {
            var mvcm = new MontevideoCoordinateModel();
            decimal latDegrees = mvcm.DegreesLat;
            decimal latMinutes = Math.Truncate(mvcm.DdmMinsLat);
            decimal latSeconds = mvcm.DmsSecondsLat;
            decimal lonDegrees = mvcm.DegreesLon;
            decimal lonMinutes = Math.Truncate(mvcm.DdmMinsLon);
            decimal lonSeconds = mvcm.DmsSecondsLon;

            var ddm = new DDMCoordinate(latDegrees, latMinutes, latSeconds, lonDegrees, lonMinutes, lonSeconds);

            string expectedResult = MontevideoCoordinateModel.strDDM();
            string actualResult = ddm.ToString();

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(mvcm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(mvcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - mvcm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - mvcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void DdmToDDM_NW_Test()
        {
            var wdccm = new WashingtondcCoordinateModel();
            var ddm = new DDMCoordinate(
                wdccm.ShortDegreesLattitude(), wdccm.DdmMinsLat,
                wdccm.ShortDegreesLongitude(), wdccm.DdmMinsLon
                );
            string expectedResult = WashingtondcCoordinateModel.strDDM();
            int expectedLength = expectedResult.Length;

            string actualResult = ddm.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(wdccm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(wdccm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - wdccm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - wdccm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void DdmStringToDDM_NE_Test()
        {
            var mcm = new MunichCoordinatesModel();
            var ddm = new DDMCoordinate(
                mcm.ShortDegreesLattitude(), mcm.DdmMinsLat,
                mcm.ShortDegreesLongitude(), mcm.DdmMinsLon
                );
            string expectedResult = MunichCoordinatesModel.strDDM();
            int expectedLength = expectedResult.Length;

            string actualResult = ddm.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(mcm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(mcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - mcm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - mcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void DdmToDDM_SE_Test()
        {
            var wcm = new WellingtonCoordinateModel();
            var ddm = new DDMCoordinate(
                wcm.ShortDegreesLattitude(), wcm.DdmMinsLat,
                wcm.ShortDegreesLongitude(), wcm.DdmMinsLon
                );
            string expectedResult = WellingtonCoordinateModel.strDDM();
            int expectedLength = expectedResult.Length;

            string actualResult = ddm.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(wcm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(wcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - wcm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - wcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void DdmToDDM_SW_Test()
        {
            var mvcm = new MontevideoCoordinateModel();
            var ddm = new DDMCoordinate(
                mvcm.ShortDegreesLattitude(), mvcm.DdmMinsLat,
                mvcm.ShortDegreesLongitude(), mvcm.DdmMinsLon
                );
            string expectedResult = MontevideoCoordinateModel.strDDM();
            int expectedLength = expectedResult.Length;

            string actualResult = ddm.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(mvcm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(mvcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - mvcm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - mvcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void IsValid_90_180_Passes_Test()
        {
            decimal lattitude = 90.0m;
            decimal longitude = 180.0m;
            bool expectedResult = true;

            var ddm = new DDMCoordinate(lattitude, longitude);
            bool actualResult = ddm.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void IsValid_InvalidLat_Test()
        {
            decimal lattitude = 91.0m;
            decimal longitude = 180.0m;
            bool expectedResult = false;

            var ddm = new DDMCoordinate(lattitude, longitude);
            bool actualResult = ddm.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void IsValid_InvalidLon_Test()
        {
            decimal lattitude = 90.0m;
            decimal longitude = -181.0m;
            bool expectedResult = false;

            var ddm = new DDMCoordinate(lattitude, longitude);
            bool actualResult = ddm.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }


        [TestMethod()]
        public void DdToDDM_NE_Test()
        {
            var mcm = new MunichCoordinatesModel();
            var ddm = new DDMCoordinate(
                mcm.DegreesLat, mcm.DegreesLon
                );
            string expectedResult = MunichCoordinatesModel.strDDM();
            int expectedLength = expectedResult.Length;

            string actualResult = ddm.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(mcm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(mcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - mcm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - mcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void DdToDDM_NW_Test()
        {
            var sccm = new SanClementeCoordinatesModel();
            var ddm = new DDMCoordinate(
                sccm.DegreesLat, sccm.DegreesLon
                );
            string expectedResult = SanClementeCoordinatesModel.strDDM();
            int expectedLength = expectedResult.Length;

            string actualResult = ddm.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(sccm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(sccm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - sccm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - sccm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void DdToDDM_SE_Test()
        {
            var wcm = new WellingtonCoordinateModel();
            var ddm = new DDMCoordinate(
                wcm.DegreesLat, wcm.DegreesLon
                );
            string expectedResult = WellingtonCoordinateModel.strDDM();
            int expectedLength = expectedResult.Length;

            string actualResult = ddm.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(wcm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(wcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - wcm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - wcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void DdToDDM_SW_Test()
        {
            var mvcm = new MontevideoCoordinateModel();
            var ddm = new DDMCoordinate(
                mvcm.DegreesLat, mvcm.DegreesLon
                );
            string expectedResult = MontevideoCoordinateModel.strDDM();
            int expectedLength = expectedResult.Length;

            string actualResult = ddm.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(mvcm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(mvcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - mvcm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - mvcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void DmsToDDM_NW_Test()
        {
            var wdccm = new WashingtondcCoordinateModel();
            var ddm = new DDMCoordinate(
                wdccm.ShortDegreesLattitude(), wdccm.DdmMinsLat, wdccm.DmsSecondsLat,
                wdccm.ShortDegreesLongitude(), wdccm.DdmMinsLon, wdccm.DmsSecondsLon
                );
            string expectedResult = WashingtondcCoordinateModel.strDDM();
            int expectedLength = expectedResult.Length;

            string actualResult = ddm.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(wdccm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(wdccm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - wdccm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - wdccm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void DmsToDDM_NE_Test()
        {
            var mcm = new MunichCoordinatesModel();
            var ddm = new DDMCoordinate(
                mcm.ShortDegreesLattitude(), mcm.DdmMinsLat, mcm.DmsSecondsLat,
                mcm.ShortDegreesLongitude(), mcm.DdmMinsLon, mcm.DmsSecondsLon
                );
            string expectedResult = MunichCoordinatesModel.strDDM();
            int expectedLength = expectedResult.Length;

            string actualResult = ddm.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(mcm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(mcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - mcm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - mcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void DmsToDDM_SE_Test()
        {
            var wcm = new WellingtonCoordinateModel();
            var ddm = new DDMCoordinate(
                wcm.ShortDegreesLattitude(), wcm.DdmMinsLat, wcm.DmsSecondsLat,
                wcm.ShortDegreesLongitude(), wcm.DdmMinsLon, wcm.DmsSecondsLon
                );
            string expectedResult = WellingtonCoordinateModel.strDDM();
            int expectedLength = expectedResult.Length;

            string actualResult = ddm.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(wcm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(wcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - wcm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - wcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void DmsToDDM_SW_Test()
        {
            var mvcm = new MontevideoCoordinateModel();
            var ddm = new DDMCoordinate(
                mvcm.ShortDegreesLattitude(), mvcm.DdmMinsLat, mvcm.DmsSecondsLat,
                mvcm.ShortDegreesLongitude(), mvcm.DdmMinsLon, mvcm.DmsSecondsLon
                );
            string expectedResult = MontevideoCoordinateModel.strDDM();
            int expectedLength = expectedResult.Length;

            string actualResult = ddm.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(mvcm.DegreesLat));
            decimal lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(mvcm.DegreesLon));

            decimal latMinsDiff = Math.Abs(ddm.GetMinsLat() - mvcm.DdmMinsLat);
            decimal lonMinsDiff = Math.Abs(ddm.GetMinsLon() - mvcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

    }
}