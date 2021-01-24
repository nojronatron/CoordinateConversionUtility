using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateConversionUtility.Models;
using CoordinateConversionUtility_UnitTests.TestModels;
using System.Collections.Generic;

namespace CoordinateConversionUtility.Tests
{
    [TestClass()]
    public class ConversionHelperTests
    {
        private static void DisplayOutput(string expectedResult, string actualResult, Dictionary<string,decimal> diffs)
        {
            Console.WriteLine($"Expected: { expectedResult }");
            Console.WriteLine($"Actual: { actualResult }");

            foreach (KeyValuePair<string,decimal> diff in diffs)
            {
                Console.WriteLine($"{diff.Key}: {diff.Value.ToString()}");
            }
        }

        [TestMethod()]
        public void Test_ConvertDDtoDDM()
        {
            var lcm = new LynnwoodCoordinatesModel();
            decimal ddLat = lcm.DegreesLat;
            decimal ddLon = lcm.DegreesLon;

            DDMCoordinate ddm = new DDMCoordinate(ddLat, ddLon);

            string expectedResult = LynnwoodCoordinatesModel.strDDM();
            string actualResult = ddm.ToString();

            var latDiff = Math.Abs( ddm.GetShortDegreesLat() + ddm.GetFractionalLattitude() - lcm.DegreesLat);
            var lonDiff = Math.Abs( ddm.GetShortDegreesLon() + ddm.GetFractionalLongitude() - lcm.DegreesLon);

            //Console.WriteLine($"latDiff: { latDiff }");
            //Console.WriteLine($"lonDiff: { lonDiff }");
            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
        }

        [TestMethod()]
        public void Test_ConvertDDToDMS()
        {
            //var lcm = new LynnwoodCoordinatesModel();
            //decimal ddLat = lcm.DegreesLat;
            //decimal ddLon = lcm.DegreesLon;

            //DMSCoordinate dms = new DMSCoordinate(ddLat, ddLon);

            //string expectedResult = LynnwoodCoordinatesModel.strDMS();
            //string actualResult = dms.ToString();

            //var latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(lcm.DegreesLat));
            //var lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(lcm.DegreesLon));

            //var latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(lcm.DdmMinsLat));
            //var lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(lcm.DdmMinsLon));

            //var latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - lcm.DmsSecondsLat);
            //var lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - lcm.DmsSecondsLon);

            //Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            //dict.Add("latDiff", latDiff);
            //dict.Add("lonDiff", lonDiff);
            //dict.Add("latMinsDiff", latMinsDiff);
            //dict.Add("lonMinsDiff", lonMinsDiff);
            //dict.Add("latSecsDiff", latSecsDiff);
            //dict.Add("lonSecsDiff", lonSecsDiff);
            //DisplayOutput(expectedResult, actualResult, dict);

            //Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            //Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);
            //Assert.IsTrue(latSecsDiff >= 0 && latSecsDiff <= 1.0m);

            //Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            //Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m );
            //Assert.IsTrue(lonSecsDiff >= 0 && lonSecsDiff <= 1.0m );
        }

        [TestMethod()]
        public void Test_ConvertDDMToDD()
        {
            var lcm = new LynnwoodCoordinatesModel();
            decimal ddLat = lcm.DegreesLat;
            decimal ddLon = lcm.DegreesLon;

            DDMCoordinate ddm = new DDMCoordinate(LynnwoodCoordinatesModel.strDDM());
            DDCoordinate dd = ConversionHelper.ToDD(ddm);

            string expectedResult = LynnwoodCoordinatesModel.strDD();
            string actualResult = dd.ToString();

            var latDiff = Math.Abs(dd.GetLattitudeDD() - lcm.DegreesLat);
            var lonDiff = Math.Abs(dd.GetLongitudeDD() - lcm.DegreesLon);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m );
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m );
        }

        [TestMethod()]
        public void Test_ConvertDDMToDMS()
        {
            //var lcm = new LynnwoodCoordinatesModel();

            //DDMCoordinate ddm = new DDMCoordinate(LynnwoodCoordinatesModel.strDDM());
            //DMSCoordinate dms = ConversionHelper.ToDMS(ddm);

            //string expectedResult = LynnwoodCoordinatesModel.strDMS_Expected();
            //string actualResult = dms.ToString();

            //if (expectedResult != actualResult)
            //{
            //    var latDiff = Math.Abs(dms.GetShortDegreesLat() - Math.Truncate(lcm.DegreesLat));
            //    var lonDiff = Math.Abs(dms.GetShortDegreesLon() - Math.Truncate(lcm.DegreesLon));

            //    var latMinsDiff = Math.Abs(dms.GetShortMinutesLattitude() - Math.Truncate(lcm.DdmMinsLat));
            //    var lonMinsDiff = Math.Abs(dms.GetShortMinutesLongitude() - Math.Truncate(lcm.DdmMinsLon));

            //    var latSecsDiff = Math.Abs(dms.GetSecondsLattitude() - lcm.DmsSecondsLat);
            //    var lonSecsDiff = Math.Abs(dms.GetSecondsLongitude() - lcm.DmsSecondsLon);

            //    Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            //    dict.Add("latDiff", latDiff);
            //    dict.Add("lonDiff", lonDiff);
            //    dict.Add("latMinsDiff", latMinsDiff);
            //    dict.Add("lonMinsDiff", lonMinsDiff);
            //    dict.Add("latSecsDiff", latSecsDiff);
            //    dict.Add("lonSecsDiff", lonSecsDiff);
            //    DisplayOutput(expectedResult, actualResult, dict);

            //    Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            //    Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);
            //    Assert.IsTrue(latSecsDiff >= 0 && latSecsDiff <= 1.0m);

            //    Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            //    Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
            //    Assert.IsTrue(lonSecsDiff >= 0 && lonSecsDiff <= 1.0m);
            //}
            //else
            //{
            //    Assert.AreEqual(expectedResult, actualResult);
            //}
        }

        [TestMethod()]
        public void Test_ConvertDMSToDD()
        {
            var lcm = new LynnwoodCoordinatesModel();

            DMSCoordinate dms = new DMSCoordinate(LynnwoodCoordinatesModel.strDMS());
            DDCoordinate dd = ConversionHelper.ToDD(dms);

            string expectedResult = LynnwoodCoordinatesModel.strDD();
            string actualResult = dd.ToString();

            var latDiff = Math.Abs(dd.GetLattitudeDD() - lcm.DegreesLat);
            var lonDiff = Math.Abs(dd.GetLongitudeDD() - lcm.DegreesLon);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            DisplayOutput(expectedResult, actualResult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
        }

        [TestMethod()]
        public void Test_ConvertDMSToDDM()
        {
            var lcm = new LynnwoodCoordinatesModel();

            DMSCoordinate dms = new DMSCoordinate(LynnwoodCoordinatesModel.strDMS());
            DDMCoordinate ddm = ConversionHelper.ToDDM(dms);

            string expectedResult = LynnwoodCoordinatesModel.strDDM();
            string actualresult = ddm.ToString();

            var latDiff = Math.Abs(ddm.GetLattitudeDD() - Math.Truncate(lcm.DegreesLat));
            var lonDiff = Math.Abs(ddm.GetLongitudeDD() - Math.Truncate(lcm.DegreesLon));
            var latMinsDiff = Math.Abs(ddm.GetMinsLat() - lcm.DdmMinsLat);
            var lonMinsDiff = Math.Abs(ddm.GetMinsLon() - lcm.DdmMinsLon);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict.Add("latDiff", latDiff);
            dict.Add("lonDiff", lonDiff);
            dict.Add("latMinsDiff", latMinsDiff);
            dict.Add("lonMinsDiff", lonMinsDiff);
            DisplayOutput(expectedResult, actualresult, dict);

            Assert.IsTrue(latDiff >= 0 && latDiff <= 0.0001m);
            Assert.IsTrue(lonDiff >= 0 && lonDiff <= 0.0001m);
            Assert.IsTrue(latMinsDiff >= 0 && latMinsDiff <= 0.1m);
            Assert.IsTrue(lonMinsDiff >= 0 && lonMinsDiff <= 0.1m);
        }

    }
}