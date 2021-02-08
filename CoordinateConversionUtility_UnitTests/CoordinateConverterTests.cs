using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateConversionUtility.Models;
using CoordinateConversionUtility_UnitTests.TestModels;
using CoordinateConversionUtility.Models.Tests;
using System.Collections.Generic;

namespace CoordinateConversionUtility.Tests
{
    [TestClass()]
    public class CoordinateConverterTests : UnitTestsBase
    {
        [TestMethod()]
        public void CoordinateConverterCTORTest()
        {
            var cc = new CoordinateConverter();
            
            var expectedType = "CoordinateConversionUtility.CoordinateConverter";
            var actualType = cc.GetType().FullName;

            Assert.AreEqual(expectedType, actualType);
        }

        [TestMethod()]
        public void ConvertGridsquareToDDMTestNW()
        {
            var sccm = new SanClementeCoordinatesModel();
            var cc = new CoordinateConverter();
            var gridsquare = SanClementeCoordinatesModel.strGridsquare();

            var expectedResult = new DDMCoordinate(sccm.DegreesLat, sccm.DdmMinsLat, sccm.DegreesLon, sccm.DdmMinsLon);
            var actualResult = cc.ConvertGridsquareToDDM(gridsquare);

            var latDiff = Math.Abs(expectedResult.GetShortDegreesLat() - actualResult.GetShortDegreesLat());
            var lonDiff = Math.Abs(expectedResult.GetShortDegreesLon() - actualResult.GetShortDegreesLon());

            var latMinsDiff = Math.Abs(expectedResult.GetMinsLat() - actualResult.GetMinsLat());
            var lonMinsDiff = Math.Abs(expectedResult.GetMinsLon() - actualResult.GetMinsLon());

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            dict.Add("latMinsDiff", latMinsDiff);
            dict.Add("lonMinsDiff", lonMinsDiff);
            DisplayOutput(expectedResult.ToString(), actualResult.ToString(), dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);

            //var areEqual = expectedResult.Equals(actualResult);
            //Assert.IsTrue(areEqual);
        }

        [TestMethod()]
        public void ConvertGridsquareToDDMTestNE()
        {
            var mcm = new MunichCoordinatesModel();
            var cc = new CoordinateConverter();
            var gridsquare = MunichCoordinatesModel.strGridSquare();

            var expectedResult = new DDMCoordinate(mcm.DegreesLat, mcm.DdmMinsLat, mcm.DegreesLon, mcm.DdmMinsLon);
            var actualResult = cc.ConvertGridsquareToDDM(gridsquare);

            var latDiff = Math.Abs(expectedResult.GetShortDegreesLat() - actualResult.GetShortDegreesLat());
            var lonDiff = Math.Abs(expectedResult.GetShortDegreesLon() - actualResult.GetShortDegreesLon());

            var latMinsDiff = Math.Abs(expectedResult.GetMinsLat() - actualResult.GetMinsLat());
            var lonMinsDiff = Math.Abs(expectedResult.GetMinsLon() - actualResult.GetMinsLon());

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            dict.Add("latMinsDiff", latMinsDiff);
            dict.Add("lonMinsDiff", lonMinsDiff);
            DisplayOutput(expectedResult.ToString(), actualResult.ToString(), dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold); //  Longitude Minutes calculations are off by 1 full degree
        }

        [TestMethod()]
        public void ConvertGridsquareToDDMTestSW()
        {
            var wcm = new WellingtonCoordinateModel();
            var cc = new CoordinateConverter();
            var gridsquare = WellingtonCoordinateModel.strGridsquare();

            var expectedResult = new DDMCoordinate(WellingtonCoordinateModel.strAttainableDDM());
            var actualResult = cc.ConvertGridsquareToDDM(gridsquare);

            var latDiff = Math.Abs(expectedResult.GetShortDegreesLat() - actualResult.GetShortDegreesLat());
            var lonDiff = Math.Abs(expectedResult.GetShortDegreesLon() - actualResult.GetShortDegreesLon());

            var latMinsDiff = Math.Abs(expectedResult.GetMinsLat() - actualResult.GetMinsLat());
            var lonMinsDiff = Math.Abs(expectedResult.GetMinsLon() - actualResult.GetMinsLon());

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            dict.Add("latMinsDiff", latMinsDiff);
            dict.Add("lonMinsDiff", lonMinsDiff);
            DisplayOutput(expectedResult.ToString(), actualResult.ToString(), dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void ConvertGridsquareToDDMTestSE()
        {
            var mvcm = new MontevideoCoordinateModel();
            var cc = new CoordinateConverter();
            var gridsquare = MontevideoCoordinateModel.strGridsquare();

            var expectedResult = new DDMCoordinate(mvcm.ShortDegreesLattitude(), mvcm.DdmMinsLat, mvcm.ShortDegreesLongitude(), mvcm.DdmMinsLon);
            var actualResult = cc.ConvertGridsquareToDDM(gridsquare);

            var latDiff = Math.Abs(expectedResult.GetShortDegreesLat() - actualResult.GetShortDegreesLat());
            var lonDiff = Math.Abs(expectedResult.GetShortDegreesLon() - actualResult.GetShortDegreesLon());

            var latMinsDiff = Math.Abs(expectedResult.GetMinsLat() - actualResult.GetMinsLat());
            var lonMinsDiff = Math.Abs(expectedResult.GetMinsLon() - actualResult.GetMinsLon());

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            dict.Add("latMinsDiff", latMinsDiff);
            dict.Add("lonMinsDiff", lonMinsDiff);
            DisplayOutput(expectedResult.ToString(), actualResult.ToString(), dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);

            //var areEqual = expectedResult.Equals(actualResult);
            //Assert.IsTrue(areEqual);
        }

        [TestMethod()]
        public void ConvertDDMtoGridsquareTest_NW()
        {
            var sccm = new SanClementeCoordinatesModel();
            var degreesLat = Math.Truncate(sccm.DegreesLat);
            var degreesLon = Math.Truncate(sccm.DegreesLon);
            var minutesLat = sccm.DdmMinsLat;
            var minutesLon = sccm.DdmMinsLon;
            var ddm = new DDMCoordinate(degreesLat, minutesLat, degreesLon, minutesLon);
            var cc = new CoordinateConverter();

            var expectedResult = SanClementeCoordinatesModel.strGridsquare();
            var actualResult = cc.ConvertDDMtoGridsquare(ddm);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void ConvertDDMtoGridsquareTest_NE()
        {
            var mcm = new MunichCoordinatesModel();
            var degreesLat = Math.Truncate(mcm.DegreesLat);
            var degreesLon = Math.Truncate(mcm.DegreesLon);
            var minutesLat = mcm.DdmMinsLat;
            var minutesLon = mcm.DdmMinsLon;
            var ddm = new DDMCoordinate(degreesLat, minutesLat, degreesLon, minutesLon);
            var cc = new CoordinateConverter();

            var expectedResult = MunichCoordinatesModel.strGridSquare();
            var actualResult = cc.ConvertDDMtoGridsquare(ddm);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void ConvertDDMtoGridsquareTest_SE()
        {
            var wcm = new WellingtonCoordinateModel();
            var degreesLat = Math.Truncate(wcm.DegreesLat);
            var degreesLon = Math.Truncate(wcm.DegreesLon);
            var minutesLat = wcm.DdmMinsLat;
            var minutesLon = wcm.DdmMinsLon;
            var ddm = new DDMCoordinate(degreesLat, minutesLat, degreesLon, minutesLon);
            var cc = new CoordinateConverter();

            var expectedResult = WellingtonCoordinateModel.strGridsquare();
            var actualResult = cc.ConvertDDMtoGridsquare(ddm);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void ConvertDDMtoGridsquareTest_SW()
        {
            var mcm = new MontevideoCoordinateModel();
            var degreesLat = Math.Truncate(mcm.DegreesLat);
            var degreesLon = Math.Truncate(mcm.DegreesLon);
            var minutesLat = mcm.DdmMinsLat;
            var minutesLon = mcm.DdmMinsLon;
            var ddm = new DDMCoordinate(degreesLat, minutesLat, degreesLon, minutesLon);
            var cc = new CoordinateConverter();

            var expectedResult = MontevideoCoordinateModel.strGridsquare();
            var actualResult = cc.ConvertDDMtoGridsquare(ddm);

            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}