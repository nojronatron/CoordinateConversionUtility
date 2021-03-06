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
        public void DefaultCTOR_Test()
        {
            var ddmCoord = new DDMCoordinate();

            var expectedResult = true;
            var actualResult = ddmCoord.GetType().FullName == "CoordinateConversionUtility.Models.DDMCoordinate";

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

            var latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(mvcm.DegreesLat));
            var lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(mvcm.DegreesLon));

            var latMinsDiff = Math.Abs(ddm.GetMinsLat() - mvcm.DdmMinsLat);
            var lonMinsDiff = Math.Abs(ddm.GetMinsLon() - mvcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
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

            var latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(mvcm.DegreesLat));
            var lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(mvcm.DegreesLon));

            var latMinsDiff = Math.Abs(ddm.GetMinsLat() - mvcm.DdmMinsLat);
            var lonMinsDiff = Math.Abs(ddm.GetMinsLon() - mvcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
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

            var latDiff = Math.Abs(ddm.GetShortDegreesLat() - Math.Truncate(mvcm.DegreesLat));
            var lonDiff = Math.Abs(ddm.GetShortDegreesLon() - Math.Truncate(mvcm.DegreesLon));

            var latMinsDiff = Math.Abs(ddm.GetMinsLat() - mvcm.DdmMinsLat);
            var lonMinsDiff = Math.Abs(ddm.GetMinsLon() - mvcm.DdmMinsLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };

            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
        }

        [TestMethod()]
        public void DdmStringToDDM_Test()
        {
            var ddm = new DDMCoordinate(MontevideoCoordinateModel.strDDM());
            var expectedResult = MontevideoCoordinateModel.strDDM();
            var expectedLength = expectedResult.Length;

            var actualResult = ddm.ToString();
            var actualLength = actualResult.Length;

            var dict = new Dictionary<string, decimal>
            {
                { "stringToDDM", 0.0m }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(expectedLength == actualLength);
        }

        [TestMethod()]
        public void IsValid_90_180_Passes_Test()
        {
            var lattitude = 90.0m;
            var longitude = 180.0m;
            var expectedResult = true;

            var ddm = new DDMCoordinate(lattitude, longitude);
            var actualResult = ddm.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void IsValid_InvalidLat_Test()
        {
            var lattitude = 91.0m;
            var longitude = 180.0m;
            var expectedResult = false;

            var ddm = new DDMCoordinate(lattitude, longitude);
            var actualResult = ddm.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void IsValid_InvalidLon_Test()
        {
            var lattitude = 90.0m;
            var longitude = -181.0m;
            var expectedResult = false;

            var ddm = new DDMCoordinate(lattitude, longitude);
            var actualResult = ddm.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}