using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoordinateConversionUtility.Helpers.Tests
{
    [TestClass()]
    public class GridSquareHelperTests
    {
        [TestMethod()]
        public void Test_GridSquareHelper()
        {
            var gsh = new GridSquareHelper();

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void Test_ValidateGridsquareInput_Pass()
        {
            var gsh = new GridSquareHelper();
            var gridsquare = "CN87ut";
            var expectedResult = true;
            var expectedValidatedGrid = "CN87UT";

            var actualResult = gsh.ValidateGridsquareInput(gridsquare, out string actualValidatedGrid);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedGrid, actualValidatedGrid);
        }

        [TestMethod()]
        public void Test_ValidateGridsquareInput_Spaces_Pass()
        {
            var gsh = new GridSquareHelper();
            var gridsquare = "  CN87ut  ";
            var expectedResult = true;
            var expectedValidatedGrid = "CN87UT";

            var actualResult = gsh.ValidateGridsquareInput(gridsquare, out string actualValidatedGrid);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedGrid, actualValidatedGrid);
        }

    }
}