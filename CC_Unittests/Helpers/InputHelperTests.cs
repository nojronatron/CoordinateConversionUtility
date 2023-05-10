using CC_Unittests.Models;
using CC_Unittests.TestModels;
using CoordinateConversionLibrary.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CC_Unittests.Helpers
{
    [TestClass]
    public class InputHelperTests : UnitTestsBase
    {
        [TestMethod]
        public void Test_GetCommand_Grid_Succeeds()
        {
            string expectedResult = "-grid";
            string ihCommand = "-grid";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod]
        public void Test_GetCommand_dd_Succeeds()
        {
            string expectedResult = "-dd";
            string ihCommand = "-dd";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod]
        public void Test_GetCommand_ddm_Succeeds()
        {
            string expectedResult = "-ddm";
            string ihCommand = "-ddm";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod]
        public void Test_GetCommand_dms_Succeeds()
        {
            string expectedResult = "-dms";
            string ihCommand = "-dms";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod]
        public void Test_GetCommand_direwolf_Succeeds()
        {
            string expectedResult = "-direwolf";
            string ihCommand = "-direwolf";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod]
        public void Test_GetCommand_dash_h_Succeeds()
        {
            string expectedResult = "-h";
            string ihCommand = "-h";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod]
        public void Test_GetCommand_dashdash_help_Succeeds()
        {
            string expectedResult = "-h";
            string ihCommand = "--help";

            string actualresult = InputHelper.GetCommand(ihCommand);

            Assert.AreEqual(expectedResult, actualresult);
        }

        [TestMethod]
        public void Test_IsGridsquare_ValidIn_True()
        {
            bool expectedResult = true;
            string expectedValidatedGrid = "CN87UT";
            string inputGridsquare = "CN87ut";

            bool actualResult = InputHelper.IsGridsquare(inputGridsquare, out string actualValidatedGridsquare);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedGrid, actualValidatedGridsquare);
        }

        [TestMethod]
        public void Test_IsDD_Pass()
        {
            bool expectedResult = true;
            string expectedValidatedDD = MontevideoCoordinateModel.StrDD();

            bool actualResult = InputHelper.ParseAsDDCoordinate(expectedValidatedDD, out string actualValidatedDD);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDD, actualValidatedDD);
        }

        [TestMethod]
        public void Test_IsDD_Fail()
        {
            string testDDinput = "- 34 . 91000, - 56 . 21169";
            bool expectedResult = true;
            string expectedValidatedDD = MontevideoCoordinateModel.StrDD();

            bool actualResult = InputHelper.ParseAsDDCoordinate(testDDinput, out string actualValidatedDD);

            Assert.AreNotEqual(expectedResult, actualResult);
            Assert.AreNotEqual(expectedValidatedDD, actualValidatedDD);
        }

        [TestMethod]
        public void Test_IsDD_NoDecimal()
        {
            string ddInput = "47,-122";

            bool expectedResult = true;
            string expectedValidatedDD = $"47.00000{DegreesSymbol}, -122.00000{DegreesSymbol}";

            bool actualResult = InputHelper.ParseAsDDCoordinate(ddInput, out string actualValidatedDD);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDD, actualValidatedDD);
        }

        [TestMethod]
        public void Test_IsDDM_DDM()
        {
            string testDDMinput = "34 54.60S, 56 12.70W";
            bool expectedResult = true;
            string expectedValidatedDDM = MontevideoCoordinateModel.StrDDM();

            bool actualResult = InputHelper.ParseAsDDMCoordinate(testDDMinput, false, out string actualValidatedDDM);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDDM, actualValidatedDDM);
        }

        [TestMethod]
        public void Test_IsDDM_DDMdefaultNorth()
        {
            string testDDMinput = "48 8.81, 11 36.49E";
            bool expectedResult = false;
            string expectedValidatedDDM = string.Empty;

            bool actualResult = InputHelper.ParseAsDDMCoordinate(testDDMinput, false, out string actualValidatedDDM);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDDM, actualValidatedDDM);
        }


        [TestMethod]
        public void Test_IsDDM_DDMdefaultEast()
        {
            string testDDMinput = "48 8.81N, 11 36.94";
            bool expectedResult = false;
            string expectedValidatedDDM = string.Empty;

            bool actualResult = InputHelper.ParseAsDDMCoordinate(testDDMinput, false, out string actualValidatedDDM);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDDM, actualValidatedDDM);
        }

        [TestMethod]
        public void Test_IsDDM_NoDecimal()
        {
            string testDDMinput = "34 54S, 56 12W";
            bool expectedResult = true;
            string expectedValidatedDDM = $"34{DegreesSymbol}54.00{MinutesSymbol}S, 56{DegreesSymbol}12.00{MinutesSymbol}W";

            bool actualResult = InputHelper.ParseAsDDMCoordinate(testDDMinput, false, out string actualValidatedDDM);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDDM, actualValidatedDDM);
        }

        [TestMethod]
        public void Test_IsDDM_Direwolf()
        {
            string testDirewolfInput = "S34 54.6000, W56  12.7014";
            bool expectedResult = true;
            string expectedValidatedDDM = MontevideoCoordinateModel.StrDDM();

            bool actualResult = InputHelper.ParseAsDDMCoordinate(testDirewolfInput, true, out string actualValidatedDDM);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDDM, actualValidatedDDM);
        }

        [TestMethod]
        public void Test_IsDmsAlpha()
        {
            string testDMSinput = "S 34 54 36.0, W 56 12 42.08";
            bool expectedResult = true;
            string expectedValidatedDMS = MontevideoCoordinateModel.StrDMS();

            bool actualResult = InputHelper.ParseAsDMSCoordinate(testDMSinput, out string actualValidatedDMS);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDMS, actualValidatedDMS);
        }

        [TestMethod]
        public void Test_IsDmsBravo()
        {
            string testDMSinput = "  S  34   54  36.0  ,  W  56  12  42.08  ";
            bool expectedResult = true;
            string expectedValidatedDMS = MontevideoCoordinateModel.StrDMS();

            bool actualResult = InputHelper.ParseAsDMSCoordinate(testDMSinput, out string actualValidatedDMS);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDMS, actualValidatedDMS);
        }

        [TestMethod]
        public void Test_IsDmsCharlie()
        {
            string testDMSinput = "S34 54 36.0, W56 12 42.08";
            bool expectedResult = true;
            string expectedValidatedDMS = MontevideoCoordinateModel.StrDMS();

            bool actualResult = InputHelper.ParseAsDMSCoordinate(testDMSinput, out string actualValidatedDMS);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDMS, actualValidatedDMS);
        }

        [TestMethod]
        public void Test_IsDmsDelta()
        {
            string testDMSinput = "S 34 5436.0, W 56 1242.08";
            bool expectedResult = true;
            string expectedValidatedDMS = MontevideoCoordinateModel.StrDMS();

            bool actualResult = InputHelper.ParseAsDMSCoordinate(testDMSinput, out string actualValidatedDMS);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDMS, actualValidatedDMS);
        }

        [TestMethod]
        public void Test_IsDMS_NoDecimal()
        {
            string testDMSinput = "S34 54 25, W56 12 50";
            bool expectedResult = true;
            string expectedValidatedDMS = $"S 34{DegreesSymbol}54{MinutesSymbol}25.0{SecondsSymbol}" +
                                          $", W 56{DegreesSymbol}12{MinutesSymbol}50.0{SecondsSymbol}";

            bool actualResult = InputHelper.ParseAsDMSCoordinate(testDMSinput, out string actualValidatedDMS);

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedValidatedDMS, actualValidatedDMS);
        }

        [TestMethod]
        public void Test_DirewolfCoordFixer_Alpha()
        {
            string[] testDDMinputs = new string[] { "N47 48.7500", " W122 17.5000" };
            bool[] expectedResults = new bool[] { true, true };
            string[] expectedArray = new string[] { "N47", "48.7500", "W122", "17.5000" };

            bool[] actualResults = new bool[2];
            string[] fixedItems1 = new string[2];
            string[] fixedItems2 = new string[2];


            actualResults[0] = InputHelper.DDMCoordFixer(testDDMinputs[0], out fixedItems1);
            actualResults[1] = InputHelper.DDMCoordFixer(testDDMinputs[1], out fixedItems2);

            string[] actualArray = new string[4]
            {
                fixedItems1[0],
                fixedItems1[1],
                fixedItems2[0],
                fixedItems2[1]
            };

            for (int i = 0; i < expectedResults.Length; i++)
            {
                Assert.AreEqual(expectedArray[i], actualArray[i]);
            }
            for (int j = 0; j < expectedResults.Length; j++)
            {
                Assert.AreEqual(expectedResults[j], actualResults[j]);
            }
        }

        [TestMethod]
        public void Test_DdmCoordFixer_Alpha()
        {
            string[] testDDMinputs = new string[] { "4748.75 N", "122 17.50 W" };
            bool[] expectedResults = new bool[] { true, true };
            string[] expectedArray = new string[] { "47", "48.75 N", "122", "17.50 W" };

            bool[] actualResults = new bool[2];
            string[] fixedItems1 = new string[2];
            string[] fixedItems2 = new string[2];


            actualResults[0] = InputHelper.DDMCoordFixer(testDDMinputs[0], out fixedItems1);
            actualResults[1] = InputHelper.DDMCoordFixer(testDDMinputs[1], out fixedItems2);

            string[] actualArray = new string[4]
            {
                fixedItems1[0],
                fixedItems1[1],
                fixedItems2[0],
                fixedItems2[1]
            };

            for (int i = 0; i < expectedResults.Length; i++)
            {
                Assert.AreEqual(expectedArray[i], actualArray[i]);
            }
            for (int j = 0; j < expectedResults.Length; j++)
            {
                Assert.AreEqual(expectedResults[j], actualResults[j]);
            }
        }

        [TestMethod]
        public void Test_DdmCoordFixer_Bravo()
        {
            string[] testDDMinputs = new string[] { "47 48.75N", "122 17.50W" };
            bool[] expectedResults = new bool[] { true, true };
            string[] expectedArray = new string[] { "47", "48.75N", "122", "17.50W" };

            bool[] actualResults = new bool[2];
            string[] fixedItems1 = new string[2];
            string[] fixedItems2 = new string[2];


            actualResults[0] = InputHelper.DDMCoordFixer(testDDMinputs[0], out fixedItems1);
            actualResults[1] = InputHelper.DDMCoordFixer(testDDMinputs[1], out fixedItems2);

            string[] actualArray = new string[4]
            {
                fixedItems1[0],
                fixedItems1[1],
                fixedItems2[0],
                fixedItems2[1]
            };

            for (int i = 0; i < expectedResults.Length; i++)
            {
                Assert.AreEqual(expectedArray[i], actualArray[i]);
            }
            for (int j = 0; j < expectedResults.Length; j++)
            {
                Assert.AreEqual(expectedResults[j], actualResults[j]);
            }
        }
        [TestMethod]
        public void Test_DdmCoordFixer_Charlie()
        {
            string[] testDDMinputs = new string[] { "4748.75N", "122 17.50 W " };
            bool[] expectedResults = new bool[] { true, true };
            string[] expectedArray = new string[] { "47", "48.75N", "122", "17.50 W" };

            bool[] actualResults = new bool[2];
            string[] fixedItems1 = new string[2];
            string[] fixedItems2 = new string[2];


            actualResults[0] = InputHelper.DDMCoordFixer(testDDMinputs[0], out fixedItems1);
            actualResults[1] = InputHelper.DDMCoordFixer(testDDMinputs[1], out fixedItems2);

            string[] actualArray = new string[4]
            {
                fixedItems1[0],
                fixedItems1[1],
                fixedItems2[0],
                fixedItems2[1]
            };

            for (int i = 0; i < expectedResults.Length; i++)
            {
                Assert.AreEqual(expectedArray[i], actualArray[i]);
            }
            for (int j = 0; j < expectedResults.Length; j++)
            {
                Assert.AreEqual(expectedResults[j], actualResults[j]);
            }
        }
        [TestMethod]
        public void Test_DdmCoordFixer_Delta()
        {
            string[] testDDMinputs = new string[] { "  47  48.75  N  ", "  122  17.50  W  " };
            bool[] expectedResults = new bool[] { true, true };
            string[] expectedArray = new string[] { "47", "48.75  N", "122", "17.50  W" };

            bool[] actualResults = new bool[2];
            string[] fixedItems1 = new string[2];
            string[] fixedItems2 = new string[2];


            actualResults[0] = InputHelper.DDMCoordFixer(testDDMinputs[0], out fixedItems1);
            actualResults[1] = InputHelper.DDMCoordFixer(testDDMinputs[1], out fixedItems2);

            string[] actualArray = new string[4]
            {
                fixedItems1[0],
                fixedItems1[1],
                fixedItems2[0],
                fixedItems2[1]
            };

            for (int i = 0; i < expectedResults.Length; i++)
            {
                Assert.AreEqual(expectedArray[i], actualArray[i]);
            }
            for (int j = 0; j < expectedResults.Length; j++)
            {
                Assert.AreEqual(expectedResults[j], actualResults[j]);
            }
        }
    }
}