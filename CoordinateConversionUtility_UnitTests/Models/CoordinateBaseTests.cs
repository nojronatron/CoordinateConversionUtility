using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoordinateConversionUtility.Models.Tests
{
    [TestClass()]
    public class CoordinateBaseTests
    {
        [TestMethod()]
        public void ValidateLattitude()
        {
            bool expectedResult = true;
            decimal expectedOutput = -35m;

            string strLattitude = "-35";

            bool actualResult = CoordinateBase.ValidateIsLatDegrees(strLattitude, out decimal actualOutput);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod()]
        public void ValidateLongitude()
        {
            bool expectedresult = true;
            decimal expectedOutput = -123m;

            string strLongitude = "-123";

            bool actualResult = CoordinateBase.ValidateIsLonDegrees(strLongitude, out decimal actualOutput);

            Assert.AreEqual(expectedresult, actualResult);
            Assert.AreEqual(expectedOutput, actualOutput);
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

        [TestMethod()]
        public void IsValid_CannotValidateBaseInstance_Test()
        {
            bool expectedResult = false;
            var cb = new CoordinateBase();

            bool actualResult = cb.IsValid;

            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}