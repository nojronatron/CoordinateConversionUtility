using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateConversionUtility.Models;

namespace CoordinateConversionUtility.Tests
{
    [TestClass()]
    public class ConversionHelperTests
    {
        [TestMethod()]
        public void Test_ConvertDMSMinsToDD()
        {
            Decimal ddLat = 45.0000m;
            Decimal ddLon = 90.0000m;
            DMSCoordinate dms = new DMSCoordinate(ddLat, ddLon);
            
            Assert.Fail();
        }

        [TestMethod()]
        public void Test_ConvertSecondsToDM()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Test_ConvertDDMToDD()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Test_ConvertDMSToDD()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Test_ConvertDDToDDM()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Test_ConvertDMSToDDM()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Test_ConvertDDToDMS()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Test_ConvertDDMToDMS()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Test_GetMinutesLat()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Test_GetMinutesLon()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Test_GetSecondsLat()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Test_GetSecondsLon()
        {
            Assert.Fail();
        }
    }
}