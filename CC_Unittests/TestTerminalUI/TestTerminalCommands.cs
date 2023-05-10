using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoordinateConverterCmd;
using System;

namespace CC_Unittests.TerminalUI
{
    [TestClass]
    public class TestTerminalCommands
    {
        [TestMethod]
        public void CanInstantiateCoordConverter()
        {
            CoordConverter sut = null;
            Assert.IsNull(sut);
            sut = new CoordinateConverterCmd.CoordConverter();
            Assert.IsNotNull(sut);
        }
        [TestMethod]
        public void NullInputDoesNotThrow()
        {
            try
            {
                CoordConverter.Main(new string[0]);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected calling main with 0 length string array to not throw but got: " + ex.Message);
            }
            // TODO: Enable this sub-test after refactoring CoordConverter.Main to delegate its work
            // try
            // {
            //     CoordConverter.Main(new string[1]);
            // }
            // catch (Exception ex)
            // {
            //     Assert.Fail("Expected calling main with 1 length string array to not throw but got: " + ex.Message);
            // }
        }
    }
}