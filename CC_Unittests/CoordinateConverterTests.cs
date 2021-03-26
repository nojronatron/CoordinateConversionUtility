using CC_Unittests.Models;
using CC_Unittests.TestModels;
using CoordinateConversionLibrary;
using CoordinateConversionLibrary.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CC_Unittests
{
    [TestFixture()]
    public class CoordinateConverterTests : UnitTestsBase
    {
        [Test()]
        public void CoordinateConverterCTORTest()
        {
            var cc = new GridDdmExpert();

            string expectedType = "CoordinateConversionLibrary.GridDdmExpert";
            string actualType = cc.GetType().FullName;

            Assert.AreEqual(expectedType, actualType);
        }

        [Test()]
        public void ConvertGridsquareToDDMTestNW()
        {
            var sccm = new SanClementeCoordinatesModel();
            var cc = new GridDdmExpert();
            string gridsquare = SanClementeCoordinatesModel.StrGridsquare();

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

        [Test()]
        public void ConvertGridsquareToDDMTestNE()
        {
            var mcm = new MunichCoordinatesModel();
            var cc = new GridDdmExpert();
            string gridsquare = MunichCoordinatesModel.StrGridSquare();

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

        [Test()]
        public void ConvertGridsquareToDDMTestSW()
        {
            var cc = new GridDdmExpert();
            string gridsquare = WellingtonCoordinateModel.StrGridsquare();

            var expectedResult = new DDMCoordinate(WellingtonCoordinateModel.StrAttainableDDM());
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

        [Test()]
        public void ConvertGridsquareToDDMTestSE()
        {
            var mvcm = new MontevideoCoordinateModel();
            var cc = new GridDdmExpert();
            string gridsquare = MontevideoCoordinateModel.StrGridsquare();

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

        [Test()]
        public void ConvertDDMtoGridsquareTest_NW()
        {
            var sccm = new SanClementeCoordinatesModel();
            decimal degreesLat = Math.Truncate(sccm.DegreesLat);
            decimal degreesLon = Math.Truncate(sccm.DegreesLon);
            decimal minutesLat = sccm.DdmMinsLat;
            decimal minutesLon = sccm.DdmMinsLon;
            var ddm = new DDMCoordinate(degreesLat, minutesLat, degreesLon, minutesLon);
            var cc = new GridDdmExpert();

            string expectedResult = SanClementeCoordinatesModel.StrGridsquare();
            string actualResult = cc.ConvertDDMtoGridsquare(ddm);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void ConvertDDMtoGridsquareTest_NE()
        {
            var mcm = new MunichCoordinatesModel();
            decimal degreesLat = Math.Truncate(mcm.DegreesLat);
            decimal degreesLon = Math.Truncate(mcm.DegreesLon);
            decimal minutesLat = mcm.DdmMinsLat;
            decimal minutesLon = mcm.DdmMinsLon;
            var ddm = new DDMCoordinate(degreesLat, minutesLat, degreesLon, minutesLon);
            var cc = new GridDdmExpert();

            string expectedResult = MunichCoordinatesModel.StrGridSquare();
            string actualResult = cc.ConvertDDMtoGridsquare(ddm);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void ConvertDDMtoGridsquareTest_SE()
        {
            var wcm = new WellingtonCoordinateModel();
            decimal degreesLat = Math.Truncate(wcm.DegreesLat);
            decimal degreesLon = Math.Truncate(wcm.DegreesLon);
            decimal minutesLat = wcm.DdmMinsLat;
            decimal minutesLon = wcm.DdmMinsLon;
            var ddm = new DDMCoordinate(degreesLat, minutesLat, degreesLon, minutesLon);
            var cc = new GridDdmExpert();

            string expectedResult = WellingtonCoordinateModel.StrGridsquare();
            string actualResult = cc.ConvertDDMtoGridsquare(ddm);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void ConvertDDMtoGridsquareTest_SW()
        {
            var mcm = new MontevideoCoordinateModel();
            decimal degreesLat = Math.Truncate(mcm.DegreesLat);
            decimal degreesLon = Math.Truncate(mcm.DegreesLon);
            decimal minutesLat = mcm.DdmMinsLat;
            decimal minutesLon = mcm.DdmMinsLon;
            var ddm = new DDMCoordinate(degreesLat, minutesLat, degreesLon, minutesLon);
            var cc = new GridDdmExpert();

            string expectedResult = MontevideoCoordinateModel.StrGridsquare();
            string actualResult = cc.ConvertDDMtoGridsquare(ddm);

            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}