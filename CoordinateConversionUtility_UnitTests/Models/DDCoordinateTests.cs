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

            var expectedResult = true;
            var actualResult = ddCoord.GetType().FullName == "CoordinateConversionUtility.Models.DDCoordinate";

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

            var latDiff = Math.Abs(dd.GetLattitudeDD() - mvcm.DegreesLat);
            var lonDiff = Math.Abs(dd.GetLongitudeDD() - mvcm.DegreesLon);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
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

            var latDiff = Math.Abs(dd.GetLattitudeDD() - mcm.DegreesLat);
            var lonDiff = Math.Abs(dd.GetLongitudeDD() - mcm.DegreesLon);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
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

            var latDiff = Math.Abs(dd.GetLattitudeDD() - mcm.DegreesLat);
            var lonDiff = Math.Abs(dd.GetLongitudeDD() - mcm.DegreesLon);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
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
            var expectedResult = MunichCoordinatesModel.strDD();
            var expectedLength = expectedResult.Length;

            var actualResult = mcm.ToString();
            var actualLength = actualResult.Length;

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
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

            var expectedResult = mcm.ShortDegreesLongitude();
            var actualResult = dd.GetShortDegreesLon();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetShortDegreesLatTest()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(mcm.DegreesLat, mcm.DegreesLon);

            var expectedResult = mcm.ShortDegreesLattitude();
            var actualResult = dd.GetShortDegreesLat();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetFractionalLattitudeTest()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(mcm.DegreesLat, mcm.DegreesLon);

            var expectedResult = mcm.DegreesLat - Math.Truncate(mcm.DegreesLat);
            var actualResult = dd.GetFractionalLattitude();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetFractionalLongitudeTest()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(mcm.DegreesLat, mcm.DegreesLon);

            var expectedResult = mcm.DegreesLon - Math.Truncate(mcm.DegreesLon);
            var actualResult = dd.GetFractionalLongitude();

            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}