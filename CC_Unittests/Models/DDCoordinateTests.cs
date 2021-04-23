using System;
using System.Collections.Generic;
using CC_Unittests.TestModels;
using CoordinateConversionLibrary.Models;
using NUnit.Framework;

namespace CC_Unittests.Models
{
    [TestFixture()]
    public class DDCoordinateTests : UnitTestsBase
    {
        [Test()]
        public void DefaultCtorCreatesInvalidDMSCoordinate_Test()
        {
            var ddm = new DDMCoordinate();
            var expectedResult = false;
            var expectedLatDegrees = 0m;
            var expectedLonDegrees = 0m;

            var actualResult = ddm.IsValid;
            var actualLatDegrees = ddm.GetShortDegreesLat();
            var actualLonDegrees = ddm.GetShortDegreesLon();

            Assert.AreEqual(expectedLatDegrees, actualLatDegrees);
            Assert.AreEqual(expectedLonDegrees, actualLonDegrees);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void DefaultCTOR_Test()
        {
            var ddCoord = new DDCoordinate();

            bool expectedResult = true;
            bool actualResult = ddCoord.GetType().FullName == "CoordinateConversionLibrary.Models.DDCoordinate";

            Assert.IsTrue(expectedResult == actualResult);
        }

        [Test()]
        public void CTOR_DD_Test()
        {
            var mvcm = new MunichCoordinatesModel();
            decimal ddLat = mvcm.DegreesLat;
            decimal ddLon = mvcm.DegreesLon;

            var dd = new DDCoordinate(ddLat, ddLon);

            string expectedResult = MunichCoordinatesModel.StrDDM();
            string actualResult = dd.ToString();

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mvcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mvcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void CTOR_DDM_Test()
        {
            var mcm = new MunichCoordinatesModel();
            decimal latDegrees = mcm.ShortDegreesLattitude();
            decimal latMins = mcm.DdmMinsLat;
            decimal lonDegrees = mcm.ShortDegreesLongitude();
            decimal lonMins = mcm.DdmMinsLon;

            var dd = new DDCoordinate(latDegrees, latMins, lonDegrees, lonMins);

            string expectedResult = MunichCoordinatesModel.StrDD();
            string actualResult = dd.ToString();

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
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

            string expectedResult = MunichCoordinatesModel.StrDD();
            string actualResult = dd.ToString();

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DdStringToDD_NE_Test()
        {
            var dd = new DDCoordinate(MunichCoordinatesModel.StrDD());
            string expectedResult = MunichCoordinatesModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            var mcm = new MunichCoordinatesModel();
            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DdStringToDD_NW_Test()
        {
            var dd = new DDCoordinate(WashingtondcCoordinateModel.StrDD());
            string expectedResult = WashingtondcCoordinateModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            var wdccm = new WashingtondcCoordinateModel();
            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - wdccm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - wdccm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DdStringToDD_SE_Test()
        {
            var dd = new DDCoordinate(WellingtonCoordinateModel.StrDD());
            string expectedResult = WellingtonCoordinateModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            var wcm = new WellingtonCoordinateModel();
            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - wcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - wcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DdStringToDD_SW_Test()
        {
            var dd = new DDCoordinate(MontevideoCoordinateModel.StrDD());
            string expectedResult = MontevideoCoordinateModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            var mvcm = new MontevideoCoordinateModel();
            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mvcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mvcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DdToDD_NE_Test()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(mcm.DegreesLat, mcm.DegreesLon);
            string expectedResult = MunichCoordinatesModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DdToDD_NW_Test()
        {
            var wdccm = new WashingtondcCoordinateModel();
            var dd = new DDCoordinate(wdccm.DegreesLat, wdccm.DegreesLon);
            string expectedResult = WashingtondcCoordinateModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - wdccm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - wdccm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DdToDD_SE_Test()
        {
            var wcm = new WellingtonCoordinateModel();
            var dd = new DDCoordinate(wcm.DegreesLat, wcm.DegreesLon);
            string expectedResult = WellingtonCoordinateModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - wcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - wcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DdToDD_SW_Test()
        {
            var mvcm = new MontevideoCoordinateModel();
            var dd = new DDCoordinate(mvcm.DegreesLat, mvcm.DegreesLon);
            string expectedResult = MontevideoCoordinateModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mvcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mvcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DdmToDD_NW_Test()
        {
            var wdccm = new WashingtondcCoordinateModel();
            var dd = new DDCoordinate(
                wdccm.ShortDegreesLattitude(), wdccm.DdmMinsLat, wdccm.ShortDegreesLongitude(), wdccm.DdmMinsLon
                );
            string expectedResult = WashingtondcCoordinateModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - wdccm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - wdccm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DdmToDD_NE_Test()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(
                mcm.ShortDegreesLattitude(), mcm.DdmMinsLat, mcm.ShortDegreesLongitude(), mcm.DdmMinsLon
                );
            string expectedResult = MunichCoordinatesModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DdmToDD_SE_Test()
        {
            var wcm = new WellingtonCoordinateModel();
            var dd = new DDCoordinate(
                wcm.ShortDegreesLattitude(), wcm.DdmMinsLat, wcm.ShortDegreesLongitude(), wcm.DdmMinsLon
                );
            string expectedResult = WellingtonCoordinateModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - wcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - wcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DdmToDD_SW_Test()
        {
            var mvcm = new MontevideoCoordinateModel();
            var dd = new DDCoordinate(
                mvcm.ShortDegreesLattitude(), mvcm.DdmMinsLat, mvcm.ShortDegreesLongitude(), mvcm.DdmMinsLon
                );
            string expectedResult = MontevideoCoordinateModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mvcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mvcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DmsToDD_NW_Test()
        {
            var wdccm = new WashingtondcCoordinateModel();
            var dd = new DDCoordinate(
                wdccm.ShortDegreesLattitude(), wdccm.DdmMinsLat, wdccm.DmsSecondsLat,
                wdccm.ShortDegreesLongitude(), wdccm.DdmMinsLon, wdccm.DmsSecondsLon
                );
            string expectedResult = WashingtondcCoordinateModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - wdccm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - wdccm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DmsToDD_NE_Test()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(
                mcm.ShortDegreesLattitude(), mcm.DdmMinsLat, mcm.DmsSecondsLat,
                mcm.ShortDegreesLongitude(), mcm.DdmMinsLon, mcm.DmsSecondsLon
                );
            string expectedResult = MunichCoordinatesModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DmsToDD_SE_Test()
        {
            var wcm = new WellingtonCoordinateModel();
            var dd = new DDCoordinate(
                wcm.ShortDegreesLattitude(), wcm.DdmMinsLat, wcm.DmsSecondsLat,
                wcm.ShortDegreesLongitude(), wcm.DdmMinsLon, wcm.DmsSecondsLon
                );
            string expectedResult = WellingtonCoordinateModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - wcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - wcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void DmsToDD_SW_Test()
        {
            var mvcm = new MontevideoCoordinateModel();
            var dd = new DDCoordinate(
                mvcm.ShortDegreesLattitude(), mvcm.DdmMinsLat, mvcm.DmsSecondsLat,
                mvcm.ShortDegreesLongitude(), mvcm.DdmMinsLon, mvcm.DmsSecondsLon
                );
            string expectedResult = MontevideoCoordinateModel.StrDD();
            int expectedLength = expectedResult.Length;

            string actualResult = dd.ToString();
            int actualLength = actualResult.Length;

            decimal latDiff = Math.Abs(dd.GetLattitudeDD() - mvcm.DegreesLat);
            decimal lonDiff = Math.Abs(dd.GetLongitudeDD() - mvcm.DegreesLon);

            var dict = new Dictionary<string, decimal>
            {
                { "latDiff", latDiff },
                { "lonDiff", lonDiff }
            };
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.AreEqual(expectedLength, actualLength);
            Assert.IsTrue(latDiff >= 0 && latDiff <= DegreeAccuracyThreshold);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= DegreeAccuracyThreshold);
        }

        [Test()]
        public void GetShortDegreesLonTest()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(mcm.DegreesLat, mcm.DegreesLon);

            decimal expectedResult = mcm.ShortDegreesLongitude();
            decimal actualResult = dd.GetShortDegreesLon();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void GetShortDegreesLatTest()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(mcm.DegreesLat, mcm.DegreesLon);

            decimal expectedResult = mcm.ShortDegreesLattitude();
            decimal actualResult = dd.GetShortDegreesLat();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void GetFractionalLattitudeTest()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(mcm.DegreesLat, mcm.DegreesLon);

            decimal expectedResult = mcm.DegreesLat - Math.Truncate(mcm.DegreesLat);
            decimal actualResult = dd.GetFractionalLattitude();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void GetFractionalLongitudeTest()
        {
            var mcm = new MunichCoordinatesModel();
            var dd = new DDCoordinate(mcm.DegreesLat, mcm.DegreesLon);

            decimal expectedResult = mcm.DegreesLon - Math.Truncate(mcm.DegreesLon);
            decimal actualResult = dd.GetFractionalLongitude();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void IsValid_90_180_Passes_Test()
        {
            decimal lattitude = 90.0m;
            decimal longitude = 180.0m;
            bool expectedResult = true;

            var dd = new DDCoordinate(lattitude, longitude);
            bool actualResult = dd.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void IsValid_InvalidLat_Test()
        {
            decimal lattitude = 91.0m;
            decimal longitude = 180.0m;
            bool expectedResult = false;

            var dd = new DDCoordinate(lattitude, longitude);
            bool actualResult = dd.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
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