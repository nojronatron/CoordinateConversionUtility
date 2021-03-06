using CoordinateConversionUtility_UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoordinateConversionUtility.Helpers.Tests
{
    [TestClass()]
    public class InputHelperTests
    {
        [TestMethod()]
        public void Test_GetCommand_Grid_Succeeds()
        {
            var expectedResult = "-grid";
            var ihCommand = "-grid";

            var actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod()]
        public void Test_GetCommand_dd_Succeeds()
        {
            var expectedResult = "-dd";
            var ihCommand = "-dd";

            var actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod()]
        public void Test_GetCommand_ddm_Succeeds()
        {
            var expectedResult = "-ddm";
            var ihCommand = "-ddm";

            var actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod()]
        public void Test_GetCommand_dms_Succeeds()
        {
            var expectedResult = "-dms";
            var ihCommand = "-dms";

            var actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod()]
        public void Test_GetCommand_direwolf_Succeeds()
        {
            var expectedResult = "-direwolf";
            var ihCommand = "-direwolf";

            var actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod()]
        public void Test_GetCommand_dash_h_Succeeds()
        {
            var expectedResult = "-h";
            var ihCommand = "-h";

            var actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod()]
        public void Test_GetCommand_dashdash_help_Succeeds()
        {
            var expectedResult = "-h";
            var ihCommand = "--help";

            var actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod()]
        public void Test_IsGridsquare_ValidIn_True()
        {
            var expectedResult = true;
            var expectedValidatedGrid = "CN87UT";
            var inputGridsquare = "CN87ut";

            var actualResult = InputHelper.IsGridsquare(inputGridsquare, out string actualValidatedGridsquare);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedGrid, actualValidatedGridsquare);
        }

        [TestMethod()]
        public void Test_IsDD_Pass()
        {
            var expectedResult = true;
            var expectedValidatedDD = MontevideoCoordinateModel.strDD();

            var actualResult = InputHelper.IsDD(expectedValidatedDD, out string actualValidatedDD);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDD, actualValidatedDD);
        }

        [TestMethod()]
        public void Test_IsDD_Fail()
        {
            var testDDinput = "- 34 . 91000, - 56 . 21169";
            var expectedResult = true;
            var expectedValidatedDD = MontevideoCoordinateModel.strDD();

            var actualResult = InputHelper.IsDD(testDDinput, out string actualValidatedDD);

            Assert.AreNotEqual(actualResult, expectedResult);
            Assert.AreNotEqual(actualValidatedDD, expectedValidatedDD);
        }

        [TestMethod()]
        public void Test_IsDDM_DDM()
        {
            var testDDMinput = "34 54.60S, 56 12.70W";
            var expectedResult = true;
            var expectedValidatedDDM = MontevideoCoordinateModel.strDDM();

            var actualResult = InputHelper.IsDDM(testDDMinput, false, out string actualValidatedDDM);

            Assert.AreEqual(actualResult, expectedResult);
            Assert.AreEqual(actualValidatedDDM, expectedValidatedDDM);
        }

        [TestMethod()]
        public void Test_IsDDM_DDMdefaultNorth()
        {
            var testDDMinput = "48 8.81, 11 36.49E";
            var expectedResult = false;
            var expectedValidatedDDM = string.Empty;

            var actualResult = InputHelper.IsDDM(testDDMinput, false, out string actualValidatedDDM);

            Assert.AreEqual(actualResult, expectedResult);
            Assert.AreEqual(actualValidatedDDM, expectedValidatedDDM);
        }


        [TestMethod()]
        public void Test_IsDDM_DDMdefaultEast()
        {
            var testDDMinput = "48 8.81N, 11 36.94";
            var expectedResult = false;
            var expectedValidatedDDM = string.Empty;

            var actualResult = InputHelper.IsDDM(testDDMinput, false, out string actualValidatedDDM);

            Assert.AreEqual(actualResult, expectedResult);
            Assert.AreEqual(actualValidatedDDM, expectedValidatedDDM);
        }


        [TestMethod()]
        public void Test_IsDDM_Direwolf()
        {
            var testDirewolfInput = "S34 54.6000, W56  12.7014";
            var expectedResult = true;
            var expectedValidatedDDM = MontevideoCoordinateModel.strDDM();

            var actualResult = InputHelper.IsDDM(testDirewolfInput, true, out string actualValidatedDDM);

            Assert.AreEqual(actualResult, expectedResult);
            Assert.AreEqual(actualValidatedDDM, expectedValidatedDDM);
        }

        [TestMethod()]
        public void Test_IsDMS()
        {
            var testDMSinput = "S  34 54 36.0, W 56 12 42.08";
            var expectedResult = true;
            var expectedValidatedDMS = MontevideoCoordinateModel.strDMS();

            var actualResult = InputHelper.IsDMS(testDMSinput, out string actualValidatedDMS);

            Assert.AreEqual(actualResult, expectedResult);
            Assert.AreEqual(actualValidatedDMS, expectedValidatedDMS);
        }
    }
}