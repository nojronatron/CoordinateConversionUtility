using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoordinateConversionUtility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateConversionUtility.Models.Tests
{
    [TestClass()]
    public class CoordinateBaseTests
    {
        [TestMethod()]
        public void ValidateIsSecsOrMinsTest0()
        {
            string testInput = "0";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = true;

            bool actualResult = CoordinateBase.ValidateIsSecsOrMins(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsSecsOrMinsTest60()
        {
            string testInput = "60";
            decimal expectedOutResult = 60.0m;
            bool expectedResult = true;

            bool actualResult = CoordinateBase.ValidateIsSecsOrMins(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsSecsOrMinsTestNeg61()
        {
            string testInput = "-61";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = false;

            bool actualResult = CoordinateBase.ValidateIsSecsOrMins(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsSecsOrMinsTestNeg59()
        {
            string testInput = "-50";
            decimal expectedOutResult = -50.0m;
            bool expectedResult = true;

            bool actualResult = CoordinateBase.ValidateIsSecsOrMins(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsSecsOrMinsTest10000()
        {
            string testInput = "10000";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = false;

            bool actualResult = CoordinateBase.ValidateIsSecsOrMins(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsLatDegreesTest0()
        {
            string testInput = "0";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = true;

            bool actualResult = CoordinateBase.ValidateIsLatDegrees(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsLatDegreesTestNeg90()
        {
            string testInput = "-90";
            decimal expectedOutResult = -90.0m;
            bool expectedResult = true;

            bool actualResult = CoordinateBase.ValidateIsLatDegrees(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsLatDegreesTest90()
        {
            string testInput = "90";
            decimal expectedOutResult = 90.0m;
            bool expectedResult = true;

            bool actualResult = CoordinateBase.ValidateIsLatDegrees(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsLatDegreesTest91()
        {
            string testInput = "91";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = false;

            bool actualResult = CoordinateBase.ValidateIsLatDegrees(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsLatDegreesTestNeg100000()
        {
            string testInput = "-100000";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = false;

            bool actualResult = CoordinateBase.ValidateIsLatDegrees(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsLonDegreesTest0()
        {
            string testInput = "0";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = true;

            bool actualResult = CoordinateBase.ValidateIsLonDegrees(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsLonDegreesTestNeg180()
        {
            string testInput = "-180";
            decimal expectedOutResult = -180.0m;
            bool expectedResult = true;

            bool actualResult = CoordinateBase.ValidateIsLonDegrees(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsLonDegreesTest181()
        {
            string testInput = "181";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = false;

            bool actualResult = CoordinateBase.ValidateIsLonDegrees(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsLonDegreesTestNeg0()
        {
            string testInput = "-0";
            decimal expectedOutResult = 0.0m;
            bool expectedResult = true;

            bool actualResult = CoordinateBase.ValidateIsLonDegrees(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

        [TestMethod()]
        public void ValidateIsLonDegreesTest90()
        {
            string testInput = "90";
            decimal expectedOutResult = 90.0m;
            bool expectedResult = true;

            bool actualResult = CoordinateBase.ValidateIsLonDegrees(testInput, out decimal actualOutResult);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutResult, actualOutResult);
        }

    }
}