using CoordinateConversionLibrary.Helpers;
using NUnit.Framework;

namespace CC_Unittests.Helpers
{
    [TestFixture()]
    public class GridSquareHelperTests
    {
        [Test()]
        public void Test_SixthGridSquareCharacter()
        {
            short latDirection = 1;
            decimal nearestEvenMultiple = 5.0m;
            var gsh = new GridSquareHelper();
            var expectedResult = "B";

            string actualResult = gsh.GetSixthGridsquareCharacter(latDirection, nearestEvenMultiple);

            Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper());
        }
        [Test()]
        public void Test_SixthGridSquareCharacter_Negative()
        {
            short latDirection = -1;
            decimal nearestEvenMultiple = -35.0m;
            var gsh = new GridSquareHelper();
            var expectedResult = "J";

            string actualResult = gsh.GetSixthGridsquareCharacter(latDirection, nearestEvenMultiple);

            Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper());
        }
        [Test()]
        public void Test_FifthGridSquareCharacter()
        {
            short lonDirection = 1;
            decimal nearestEvenMultiple = 5.0m;
            var gsh = new GridSquareHelper();
            var expectedResult = "A";

            string actualResult = gsh.GetFifthGridsquareCharacter(lonDirection, nearestEvenMultiple);

            Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper());
        }
        [Test()]
        public void Test_FifthGridSquareCharacter_Negative()
        {
            short lonDirection = -1;
            decimal nearestEvenMultiple = -35.0m;
            var gsh = new GridSquareHelper();
            var expectedResult = "Q";

            string actualResult = gsh.GetFifthGridsquareCharacter(lonDirection, nearestEvenMultiple);

            Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper());
        }
        [Test()]
        public void Test_FourthGridSquareCharacter()
        {
            decimal latRemainder = 1.0m;
            short latDirection = 1;
            var expectedResult = "1.0";

            string actualResult = GridSquareHelper.GetFourthGridsquareCharacter(latRemainder, latDirection);

            Assert.AreEqual(expectedResult, actualResult);
        }
        [Test()]
        public void Test_FourthGridSquareCharacter_Negative()
        {
            decimal latRemainder = -1.0m;
            short latDirection = 1;
            var expectedResult = "-1.0";

            string actualResult = GridSquareHelper.GetFourthGridsquareCharacter(latRemainder, latDirection);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void Test_ThirdGridSquareCharacter()
        {
            decimal minsRemLon;
            string actualResult = GridSquareHelper.GetThirdGridsquareCharacter(1.0m, -1, out minsRemLon);

            Assert.Fail();
        }
        [Test()]
        public void Test_SecondGridSquareCharacter()
        {
            decimal remainderLat;
            var gsh = new GridSquareHelper();
            string actualResult = gsh.GetSecondGridsquareCharacter(47.8m, 1, out remainderLat);

            Assert.Fail();
        }
        [Test()]
        public void TestFirstGridSquareCharacter()
        {
            decimal remainderLon;
            var gsh = new GridSquareHelper();
            string actualResult = gsh.GetFirstGridsquareCharacter(122.0m, -1, out remainderLon);

            Assert.Fail();
        }



        [Test()]
        public void Test_GridSquareHelper()
        {
            var gsh = new GridSquareHelper();

            Assert.IsTrue(gsh.GetType().Name == "GridSquareHelper");
        }

        [Test()]
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

        [Test()]
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