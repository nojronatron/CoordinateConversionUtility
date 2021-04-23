using CoordinateConversionLibrary.Helpers;
using NUnit.Framework;

namespace CC_Unittests.Helpers
{
    [TestFixture()]
    public class LookupTablesHelperTests
    {
        [Test()]
        public void Test_LookupTablesHelper()
        {
            string expectedResult = "CoordinateConversionLibrary.Helpers.LookupTablesHelper";

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