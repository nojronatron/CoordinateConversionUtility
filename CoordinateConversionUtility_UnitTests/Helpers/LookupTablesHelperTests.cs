using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoordinateConversionUtility.Helpers.Tests
{
    [TestClass()]
    public class LookupTablesHelperTests
    {
        [TestMethod()]
        public void Test_LookupTablesHelper()
        {
            var expectedResult = "CoordinateConversionUtility.Helpers.LookupTablesHelper";

            var actualResult = new LookupTablesHelper();

            Assert.IsTrue(actualResult.GetType().FullName == expectedResult);
        }

        [TestMethod()]
        public void Test_GenerateTableLookups()
        {
            var expectedResult = true;

            LookupTablesHelper lth = new LookupTablesHelper();
            var actualResult = lth.GenerateTableLookups();

            Assert.IsTrue(expectedResult == actualResult);
        }
    }
}