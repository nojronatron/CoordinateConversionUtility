using CoordinateConversionUtility_UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CoordinateConversionUtility.Models.Tests
{
    [TestClass()]
    public class DDCoordinateTests : UnitTestsBase
    {
        [TestMethod()]
        public void DefaultCTOR_Test()
        {
            var ddCoord = new DDCoordinate();

            bool expectedResult = true;
            bool actualResult = ddCoord.GetType().FullName == "CoordinateConversionUtility.Models.DDCoordinate";

            Assert.IsTrue(expectedResult == actualResult);
        }

        [TestMethod()]
        public void CTOR_DD_Test()
        {
            var mvcm = new MunichCoordinatesModel();
            decimal ddLat = mvcm.DegreesLat;
            decimal ddLon = mvcm.DegreesLon;

            var dd = new DDCoordinate(ddLat, ddLon);

            string expectedResult = MunichCoordinatesModel.strDDM();
            string actualResult = dd.ToString();

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mvcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mvcm.DegreesLon);

            var dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [TestMethod()]
        public void CTOR_DDM_Test()
        {
            //  DDM in DD out
            var mcm = new MunichCoordinatesModel();
            decimal latDegrees = mcm.ShortDegreesLattitude();
            decimal latMins = mcm.DdmMinsLat;
            decimal lonDegrees = mcm.ShortDegreesLongitude();
            decimal lonMins = mcm.DdmMinsLon;

            var dd = new DDCoordinate(latDegrees, latMins, lonDegrees, lonMins);

            string expectedResult = MunichCoordinatesModel.strDD();
            string actualResult = dd.ToString();

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mcm.DegreesLon);

            var dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [TestMethod()]
        public void CTOR_DMS_Test()
        {
            var mcm = new MunichCoordinatesModel();
            decimal latDegrees = mcm.ShortDegreesLattitude();
            decimal latMinutes = Math.Truncate(mcm.DdmMinsLat);
            decimal latSeconds = mcm.DmsSecondsLat;
            decimal lonDegrees = mcm.ShortDegreesLongitude();
            decimal lonMinutes = Math.Truncate(mcm.DdmMinsLon);
            decimal lonSeconds = mcm.DmsSecondsLon;

            var dd = new DDCoordinate(latDegrees, latMinutes, latSeconds, lonDegrees, lonMinutes, lonSeconds);

            string expectedResult = MunichCoordinatesModel.strDD();
            string actualResult = dd.ToString();

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mcm.DegreesLon);

            var dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [TestMethod()]
        public void DdStringToDD_Test()
        {
            var mcm = new DDCoordinate(MunichCoordinatesModel.strDD());
            string expectedResult = MunichCoordinatesModel.strDD();
            int expectedLength = expectedResult.Length;

            string actualResult = mcm.ToString();
            int actualLength = actualResult.Length;

            var dict = new Dictionary<string, decimal>();
            dict.Add("stringToDD Expected:", expectedLength);
            dict.Add("stringToDD Actual:", actualLength);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetShortDegreesLonTest()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(mcm.DegreesLat, mcm.DegreesLon);

            decimal expectedResult = mcm.ShortDegreesLongitude();
            decimal actualResult = dd.GetShortDegreesLon();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetShortDegreesLatTest()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(mcm.DegreesLat, mcm.DegreesLon);

            decimal expectedResult = mcm.ShortDegreesLattitude();
            decimal actualResult = dd.GetShortDegreesLat();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetFractionalLattitudeTest()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(mcm.DegreesLat, mcm.DegreesLon);

            decimal expectedResult = mcm.DegreesLat - Math.Truncate(mcm.DegreesLat);
            decimal actualResult = dd.GetFractionalLattitude();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetFractionalLongitudeTest()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(mcm.DegreesLat, mcm.DegreesLon);

            decimal expectedResult = mcm.DegreesLon - Math.Truncate(mcm.DegreesLon);
            decimal actualResult = dd.GetFractionalLongitude();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void IsValid_90_180_Passes_Test()
        {
            decimal lattitude = 90.0m;
            decimal longitude = 180.0m;
            bool expectedResult = true;

            var dd = new DDCoordinate(lattitude, longitude);
            bool actualResult = dd.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void IsValid_InvalidLat_Test()
        {
            decimal lattitude = 91.0m;
            decimal longitude = 180.0m;
            bool expectedResult = false;

            var dd = new DDCoordinate(lattitude, longitude);
            bool actualResult = dd.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void IsValid_InvalidLon_Test()
        {
            decimal lattitude = 90.0m;
            decimal longitude = -181.0m;
            bool expectedResult = false;

            var dd = new DDCoordinate(lattitude, longitude);
            bool actualResult = dd.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}