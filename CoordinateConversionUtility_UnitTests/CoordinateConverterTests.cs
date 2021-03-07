using CoordinateConversionUtility.Models;
using CoordinateConversionUtility.Models.Tests;
using CoordinateConversionUtility_UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

            string expectedType = "CoordinateConversionUtility.CoordinateConverter";
            string actualType = cc.GetType().FullName;

            Assert.AreEqual(expectedType, actualType);
        }

        [TestMethod()]
        public void ConvertGridsquareToDDMTestNW()
        {
            var sccm = new SanClementeCoordinatesModel();
            var cc = new CoordinateConverter();
            string gridsquare = SanClementeCoordinatesModel.strGridsquare();

            var expectedResult = new DDMCoordinate(sccm.DegreesLat, sccm.DdmMinsLat, sccm.DegreesLon, sccm.DdmMinsLon);
            DDMCoordinate actualResult = cc.ConvertGridsquareToDDM(gridsquare);

            decimal latDiff = Math.Abs(expectedResult.GetShortDegreesLat() - actualResult.GetShortDegreesLat());
            decimal lonDiff = Math.Abs(expectedResult.GetShortDegreesLon() - actualResult.GetShortDegreesLon());

            decimal latMinsDiff = Math.Abs(expectedResult.GetMinsLat() - actualResult.GetMinsLat());
            decimal lonMinsDiff = Math.Abs(expectedResult.GetMinsLon() - actualResult.GetMinsLon());

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };
            DisplayOutput(expectedResult.ToString(), actualResult.ToString(), dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void ConvertGridsquareToDDMTestNE()
        {
            var mcm = new MunichCoordinatesModel();
            var cc = new CoordinateConverter();
            string gridsquare = MunichCoordinatesModel.strGridSquare();

            var expectedResult = new DDMCoordinate(mcm.DegreesLat, mcm.DdmMinsLat, mcm.DegreesLon, mcm.DdmMinsLon);
            DDMCoordinate actualResult = cc.ConvertGridsquareToDDM(gridsquare);

            decimal latDiff = Math.Abs(expectedResult.GetShortDegreesLat() - actualResult.GetShortDegreesLat());
            decimal lonDiff = Math.Abs(expectedResult.GetShortDegreesLon() - actualResult.GetShortDegreesLon());

            decimal latMinsDiff = Math.Abs(expectedResult.GetMinsLat() - actualResult.GetMinsLat());
            decimal lonMinsDiff = Math.Abs(expectedResult.GetMinsLon() - actualResult.GetMinsLon());

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };
            DisplayOutput(expectedResult.ToString(), actualResult.ToString(), dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void ConvertGridsquareToDDMTestSW()
        {
            var cc = new CoordinateConverter();
            string gridsquare = WellingtonCoordinateModel.strGridsquare();

            var expectedResult = new DDMCoordinate(WellingtonCoordinateModel.strAttainableDDM());
            DDMCoordinate actualResult = cc.ConvertGridsquareToDDM(gridsquare);

            decimal latDiff = Math.Abs(expectedResult.GetShortDegreesLat() - actualResult.GetShortDegreesLat());
            decimal lonDiff = Math.Abs(expectedResult.GetShortDegreesLon() - actualResult.GetShortDegreesLon());

            decimal latMinsDiff = Math.Abs(expectedResult.GetMinsLat() - actualResult.GetMinsLat());
            decimal lonMinsDiff = Math.Abs(expectedResult.GetMinsLon() - actualResult.GetMinsLon());

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };
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
            string gridsquare = MontevideoCoordinateModel.strGridsquare();

            var expectedResult = new DDMCoordinate(mvcm.ShortDegreesLattitude(), mvcm.DdmMinsLat, mvcm.ShortDegreesLongitude(), mvcm.DdmMinsLon);
            DDMCoordinate actualResult = cc.ConvertGridsquareToDDM(gridsquare);

            decimal latDiff = Math.Abs(expectedResult.GetShortDegreesLat() - actualResult.GetShortDegreesLat());
            decimal lonDiff = Math.Abs(expectedResult.GetShortDegreesLon() - actualResult.GetShortDegreesLon());

            decimal latMinsDiff = Math.Abs(expectedResult.GetMinsLat() - actualResult.GetMinsLat());
            decimal lonMinsDiff = Math.Abs(expectedResult.GetMinsLon() - actualResult.GetMinsLon());

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff },
                { "latMinsDiff", latMinsDiff },
                { "lonMinsDiff", lonMinsDiff }
            };
            DisplayOutput(expectedResult.ToString(), actualResult.ToString(), dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= LatMinsAccuracyThreshold);

            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= LonMinsAccuracyThreshold);
        }

        [TestMethod()]
        public void ConvertDDMtoGridsquareTest_NW()
        {
            var sccm = new SanClementeCoordinatesModel();
            decimal degreesLat = Math.Truncate(sccm.DegreesLat);
            decimal degreesLon = Math.Truncate(sccm.DegreesLon);
            decimal minutesLat = sccm.DdmMinsLat;
            decimal minutesLon = sccm.DdmMinsLon;
            var ddm = new DDMCoordinate(degreesLat, minutesLat, degreesLon, minutesLon);
            var cc = new CoordinateConverter();

            string expectedResult = SanClementeCoordinatesModel.strGridsquare();
            string actualResult = cc.ConvertDDMtoGridsquare(ddm);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void ConvertDDMtoGridsquareTest_NE()
        {
            var mcm = new MunichCoordinatesModel();
            decimal degreesLat = Math.Truncate(mcm.DegreesLat);
            decimal degreesLon = Math.Truncate(mcm.DegreesLon);
            decimal minutesLat = mcm.DdmMinsLat;
            decimal minutesLon = mcm.DdmMinsLon;
            var ddm = new DDMCoordinate(degreesLat, minutesLat, degreesLon, minutesLon);
            var cc = new CoordinateConverter();

            string expectedResult = MunichCoordinatesModel.strGridSquare();
            string actualResult = cc.ConvertDDMtoGridsquare(ddm);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void ConvertDDMtoGridsquareTest_SE()
        {
            var wcm = new WellingtonCoordinateModel();
            decimal degreesLat = Math.Truncate(wcm.DegreesLat);
            decimal degreesLon = Math.Truncate(wcm.DegreesLon);
            decimal minutesLat = wcm.DdmMinsLat;
            decimal minutesLon = wcm.DdmMinsLon;
            var ddm = new DDMCoordinate(degreesLat, minutesLat, degreesLon, minutesLon);
            var cc = new CoordinateConverter();

            string expectedResult = WellingtonCoordinateModel.strGridsquare();
            string actualResult = cc.ConvertDDMtoGridsquare(ddm);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void ConvertDDMtoGridsquareTest_SW()
        {
            var mcm = new MontevideoCoordinateModel();
            decimal degreesLat = Math.Truncate(mcm.DegreesLat);
            decimal degreesLon = Math.Truncate(mcm.DegreesLon);
            decimal minutesLat = mcm.DdmMinsLat;
            decimal minutesLon = mcm.DdmMinsLon;
            var ddm = new DDMCoordinate(degreesLat, minutesLat, degreesLon, minutesLon);
            var cc = new CoordinateConverter();

            string expectedResult = MontevideoCoordinateModel.strGridsquare();
            string actualResult = cc.ConvertDDMtoGridsquare(ddm);

            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}