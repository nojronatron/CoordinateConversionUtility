using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using CoordinateConversionUtility_UnitTests.TestModels;

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

            var latDiff = dd.GetLattitudeDD() - mvcm.DegreesLat;
            var lonDiff = dd.GetLongitudeDD() - mvcm.DegreesLon;

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
        }

        [TestMethod()]
        public void CTOR_DDM_Test()
        {
            var mvcm = new MunichCoordinatesModel();
            decimal latDegrees = mvcm.DegreesLat;
            decimal latMins = mvcm.DdmMinsLat;
            decimal lonDegrees = mvcm.DegreesLon;
            decimal lonMins = mvcm.DdmMinsLon;

            var dd = new DDCoordinate(latDegrees, latMins, lonDegrees, lonMins);

            string expectedResult = MunichCoordinatesModel.strDDM();
            string actualResult = dd.ToString();

            var latDiff = Math.Abs(dd.GetShortDegreesLat() - Math.Truncate(mvcm.DegreesLat));
            var lonDiff = Math.Abs(dd.GetShortDegreesLon() - Math.Truncate(mvcm.DegreesLon));

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
        }

        [TestMethod()]
        public void CTOR_DMS_Test()
        {
            var mvcm = new MunichCoordinatesModel();
            decimal latDegrees = mvcm.DegreesLat;
            decimal latMinutes = Math.Truncate(mvcm.DdmMinsLat);
            decimal latSeconds = mvcm.DmsSecondsLat;
            decimal lonDegrees = mvcm.DegreesLon;
            decimal lonMinutes = Math.Truncate(mvcm.DdmMinsLon);
            decimal lonSeconds = mvcm.DmsSecondsLon;

            var dd = new DDCoordinate(latDegrees, latMinutes, latSeconds, lonDegrees, lonMinutes, lonSeconds);

            string expectedResult = MunichCoordinatesModel.strDDM();
            string actualResult = dd.ToString();

            var latDiff = Math.Abs(dd.GetShortDegreesLat() - Math.Truncate(mvcm.DegreesLat));
            var lonDiff = Math.Abs(dd.GetShortDegreesLon() - Math.Truncate(mvcm.DegreesLon));

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
        }

        [TestMethod()]
        public void DdStringToDD_Test()
        {
            var dd = new DDCoordinate(MunichCoordinatesModel.strDD());
            var expectedResult = MunichCoordinatesModel.strDD();
            var expectedLength = expectedResult.Length;

            var actualResult = dd.ToString();
            var actualLength = actualResult.Length;

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("stringToDDM", 0.0m);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(expectedLength == actualLength);
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