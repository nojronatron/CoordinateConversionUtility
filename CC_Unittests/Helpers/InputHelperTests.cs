using CoordinateConversionUtility_UnitTests.TestModels;
using NUnit.Framework;

namespace CoordinateConversionUtility.Helpers.Tests
{
    [TestFixture()]
    public class InputHelperTests
    {
        [Test()]
        public void Test_GetCommand_Grid_Succeeds()
        {
            string expectedResult = "-grid";
            string ihCommand = "-grid";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [Test()]
        public void Test_GetCommand_dd_Succeeds()
        {
            string expectedResult = "-dd";
            string ihCommand = "-dd";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [Test()]
        public void Test_GetCommand_ddm_Succeeds()
        {
            string expectedResult = "-ddm";
            string ihCommand = "-ddm";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [Test()]
        public void Test_GetCommand_dms_Succeeds()
        {
            string expectedResult = "-dms";
            string ihCommand = "-dms";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [Test()]
        public void Test_GetCommand_direwolf_Succeeds()
        {
            string expectedResult = "-direwolf";
            string ihCommand = "-direwolf";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [Test()]
        public void Test_GetCommand_dash_h_Succeeds()
        {
            string expectedResult = "-h";
            string ihCommand = "-h";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [Test()]
        public void Test_GetCommand_dashdash_help_Succeeds()
        {
            string expectedResult = "-h";
            string ihCommand = "--help";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [Test()]
        public void Test_IsGridsquare_ValidIn_True()
        {
            bool expectedResult = true;
            string expectedValidatedGrid = "CN87UT";
            string inputGridsquare = "CN87ut";

            bool actualResult = InputHelper.IsGridsquare(inputGridsquare, out string actualValidatedGridsquare);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedGrid, actualValidatedGridsquare);
        }

        [Test()]
        public void Test_IsDD_Pass()
        {
            bool expectedResult = true;
            string expectedValidatedDD = MontevideoCoordinateModel.StrDD();

            bool actualResult = InputHelper.ParseAsDDCoordinate(expectedValidatedDD, out string actualValidatedDD);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDD, actualValidatedDD);
        }

        [Test()]
        public void Test_IsDD_Fail()
        {
            string testDDinput = "- 34 . 91000, - 56 . 21169";
            bool expectedResult = true;
            string expectedValidatedDD = MontevideoCoordinateModel.StrDD();

            bool actualResult = InputHelper.ParseAsDDCoordinate(testDDinput, out string actualValidatedDD);

            Assert.AreNotEqual(actualResult, expectedResult);
            Assert.AreNotEqual(actualValidatedDD, expectedValidatedDD);
        }

        [Test()]
        public void Test_IsDDM_DDM()
        {
            string testDDMinput = "34 54.60S, 56 12.70W";
            bool expectedResult = true;
            string expectedValidatedDDM = MontevideoCoordinateModel.StrDDM();

            bool actualResult = InputHelper.ParseAsDDMCoordinate(testDDMinput, false, out string actualValidatedDDM);

            Assert.AreEqual(actualResult, expectedResult);
            Assert.AreEqual(actualValidatedDDM, expectedValidatedDDM);
        }

        [Test()]
        public void Test_IsDDM_DDMdefaultNorth()
        {
            string testDDMinput = "48 8.81, 11 36.49E";
            bool expectedResult = false;
            string expectedValidatedDDM = string.Empty;

            bool actualResult = InputHelper.ParseAsDDMCoordinate(testDDMinput, false, out string actualValidatedDDM);

            Assert.AreEqual(actualResult, expectedResult);
            Assert.AreEqual(actualValidatedDDM, expectedValidatedDDM);
        }


        [Test()]
        public void Test_IsDDM_DDMdefaultEast()
        {
            string testDDMinput = "48 8.81N, 11 36.94";
            bool expectedResult = false;
            string expectedValidatedDDM = string.Empty;

            bool actualResult = InputHelper.ParseAsDDMCoordinate(testDDMinput, false, out string actualValidatedDDM);

            Assert.AreEqual(actualResult, expectedResult);
            Assert.AreEqual(actualValidatedDDM, expectedValidatedDDM);
        }


        [Test()]
        public void Test_IsDDM_Direwolf()
        {
            string testDirewolfInput = "S34 54.6000, W56  12.7014";
            bool expectedResult = true;
            string expectedValidatedDDM = MontevideoCoordinateModel.StrDDM();

            bool actualResult = InputHelper.ParseAsDDMCoordinate(testDirewolfInput, true, out string actualValidatedDDM);

            Assert.AreEqual(actualResult, expectedResult);
            Assert.AreEqual(actualValidatedDDM, expectedValidatedDDM);
        }

        [Test()]
        public void Test_IsDMS()
        {
            string testDMSinput = "S  34 54 36.0, W 56 12 42.08";
            bool expectedResult = true;
            string expectedValidatedDMS = MontevideoCoordinateModel.StrDMS();

            bool actualResult = InputHelper.ParseAsDMSCoordinate(testDMSinput, out string actualValidatedDMS);

            Assert.AreEqual(actualResult, expectedResult);
            Assert.AreEqual(actualValidatedDMS, expectedValidatedDMS);
        }
    }
}