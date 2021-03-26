using NUnit.Framework;

namespace CoordinateConversionUtility.Helpers.Tests
{
    [TestFixture()]
    public class GridSquareHelperTests
    {
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