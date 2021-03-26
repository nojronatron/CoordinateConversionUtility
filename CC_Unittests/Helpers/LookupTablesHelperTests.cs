using NUnit.Framework;

namespace CoordinateConversionUtility.Helpers.Tests
{
    [TestFixture()]
    public class LookupTablesHelperTests
    {
        [Test()]
        public void Test_LookupTablesHelper()
        {
            string expectedResult = "CoordinateConversionUtility.Helpers.LookupTablesHelper";

            var actualResult = new LookupTablesHelper();

            Assert.IsTrue(actualResult.GetType().FullName == expectedResult);
        }

        [Test()]
        public void Test_GenerateTableLookups()
        {
            bool expectedResult = true;

            var lth = new LookupTablesHelper();
            bool actualResult = lth.GenerateTableLookups();

            Assert.IsTrue(expectedResult == actualResult);
        }
    }
}