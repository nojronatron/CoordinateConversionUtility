﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoordinateConversionUtility.Helpers.Tests
{
    [TestClass()]
    public class LookupTablesHelperTests
    {
        [TestMethod()]
        public void Test_LookupTablesHelper()
        {
            string expectedResult = "CoordinateConversionUtility.Helpers.LookupTablesHelper";

            var actualResult = new LookupTablesHelper();

            Assert.IsTrue(actualResult.GetType().FullName == expectedResult);
        }

        [TestMethod()]
        public void Test_GenerateTableLookups()
        {
            bool expectedResult = true;

            var lth = new LookupTablesHelper();
            bool actualResult = lth.GenerateTableLookups();

            Assert.IsTrue(expectedResult == actualResult);
        }
    }
}