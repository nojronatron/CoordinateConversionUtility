using CoordinateConversionLibrary.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CC_Unittests.Helpers
{
    [TestClass]
    public class GridSquareHelperTests
    {
        [TestMethod]
        public void Test_SixthGridSquareCharacter()
        {
            short latDirection = 1;
            decimal nearestEvenMultiple = 5.0m;
            var gsh = new GridSquareHelper();
            var expectedResult = "B";

            string actualResult = gsh.GetSixthGridsquareCharacter(latDirection, nearestEvenMultiple);

            Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper());
        }
        [TestMethod]
        public void Test_SixthGridSquareCharacter_Negative()
        {
            short latDirection = -1;
            decimal nearestEvenMultiple = -35.0m;
            var gsh = new GridSquareHelper();
            var expectedResult = "J";

            string actualResult = gsh.GetSixthGridsquareCharacter(latDirection, nearestEvenMultiple);

            Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper());
        }
        [TestMethod]
        public void Test_FifthGridSquareCharacter()
        {
            short lonDirection = 1;
            decimal nearestEvenMultiple = 5.0m;
            var gsh = new GridSquareHelper();
            var expectedResult = "A";

            string actualResult = gsh.GetFifthGridsquareCharacter(lonDirection, nearestEvenMultiple);

            Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper());
        }
        [TestMethod]
        public void Test_FifthGridSquareCharacter_Negative()
        {
            short lonDirection = -1;
            decimal nearestEvenMultiple = -35.0m;
            var gsh = new GridSquareHelper();
            var expectedResult = "Q";

            string actualResult = gsh.GetFifthGridsquareCharacter(lonDirection, nearestEvenMultiple);

            Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper());
        }
        [TestMethod]
        public void Test_FourthGridSquareCharacter()
        {
            decimal latRemainder = 7.0m;
            short latDirection = 1;
            var expectedResult = "7";

            string actualResult = GridSquareHelper.GetFourthGridsquareCharacter(latRemainder, latDirection);

            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Test_FourthGridSquareCharacter_Negative()
        {
            decimal latRemainder = -2.0m;
            short latDirection = -1;
            string expectedResult = "7";

            string actualResult = GridSquareHelper.GetFourthGridsquareCharacter(latRemainder, latDirection);

            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Test_ThirdGridSquareCharacter()
        {
            decimal remainingLonDegrees = 3m;
            short lonDirection = 1;

            decimal expectedOut = 60m;
            string expectedResult = "1";

            string actualResult = GridSquareHelper.GetThirdGridsquareCharacter(remainingLonDegrees, lonDirection, out decimal actualOut);

            Assert.AreEqual(expectedOut, actualOut);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Test_ThirdGridSquareCharacter_Negative()
        {
            decimal remainingLonDegrees = -5m;
            short lonDirection = -1;

            decimal expectedOut = 60m;
            string expectedResult = "7";

            string actualResult = GridSquareHelper.GetThirdGridsquareCharacter(remainingLonDegrees, lonDirection, out decimal actualOut);

            Assert.AreEqual(expectedOut, actualOut);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Test_SecondGridSquareCharacter()
        {
            decimal latDegrees = 47.0m;
            short latDirection = 1;

            string expectedResult = "N";
            decimal expectedOut = 7.0m;

            var gsh = new GridSquareHelper();
            string actualResult = gsh.GetSecondGridsquareCharacter(latDegrees, latDirection, out decimal actualOut);

            Assert.AreEqual(expectedOut, actualOut);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Test_SecondGridSquareCharacter_Negative()
        {
            decimal latDegrees = -47.0m;
            short latDirection = -1;

            string expectedResult = "E";
            decimal expectedOut = -7.0m;

            var gsh = new GridSquareHelper();
            string actualResult = gsh.GetSecondGridsquareCharacter(latDegrees, latDirection, out decimal actualOut);

            Assert.AreEqual(expectedOut, actualOut);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestFirstGridSquareCharacter()
        {
            decimal lonDegrees = 122.0m;
            short lonDirection = 1;
            string expectedResult = "P";
            decimal expectedOutRemainder = 2m;

            var gsh = new GridSquareHelper();
            string actualResult = gsh.GetFirstGridsquareCharacter(lonDegrees, lonDirection, out decimal actualOutRemainder);

            Assert.AreEqual(expectedOutRemainder, actualOutRemainder);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestFirstGridSquareCharacter_Negative()
        {
            decimal lonDegrees = -122.0m;
            short lonDirection = -1;
            string expectedResult = "C";
            decimal expectedOutRemainder = -2m;

            var gsh = new GridSquareHelper();
            string actualResult = gsh.GetFirstGridsquareCharacter(lonDegrees, lonDirection, out decimal actualOutRemainder);

            Assert.AreEqual(expectedOutRemainder, actualOutRemainder);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Test_GridSquareHelper()
        {
            var gsh = new GridSquareHelper();

            Assert.IsTrue(gsh.GetType().Name == "GridSquareHelper");
        }

        [TestMethod]
        public void Test_ValidateGridsquareInput_Pass()
        {
            var gsh = new GridSquareHelper();
            string gridsquare = "CN87ut";
            bool expectedResult = true;
            string expectedValidatedGrid = "CN87UT";

            bool actualResult = gsh.ValidateGridsquareInput(gridsquare, out string actualValidatedGrid);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedGrid, actualValidatedGrid);
        }

        [TestMethod]
        public void Test_ValidateGridsquareInput_Spaces_Pass()
        {
            var gsh = new GridSquareHelper();
            string gridsquare = "  CN87ut  ";
            bool expectedResult = true;
            string expectedValidatedGrid = "CN87UT";

            bool actualResult = gsh.ValidateGridsquareInput(gridsquare, out string actualValidatedGrid);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedGrid, actualValidatedGrid);
        }

    }
}