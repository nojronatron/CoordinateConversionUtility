using CoordinateConversionUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CoordinateConversionUtility_UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        // CAN do this if necessary: CoordinateConverter ccc = null;
        [TestMethod]
        public void Test_ValidateGridsquareInput_ExactMatch()
        {
            string inputGridsquare = "CN87ut";
            string expectedResult = $"CN87UT";
            CoordinateConverter cc = new CoordinateConverter();
            string actualResult = "failed";
            if (cc.ValidateGridsquareInput(inputGridsquare))
            {
                actualResult = cc.Gridsquare;
            }
            Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
        }
        [TestMethod]
        public void Test_munich()
        {
            string[] munich = { "48*8.8'N", "11*36.5'E", "JN58td" };
            string inputGridsquare = munich[2].ToString();
            string inputDDM = $"{munich[0]},{munich[1]}";
            string expected_Gridsquare = munich[2].ToString();
            string expected_DDM = $"{munich[0]},{munich[1]}";
            bool gridToDDMpass = false;
            bool ddmToGridPass = false;
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string actual_gridToDDM = cc.ConvertGridsquareToDDM(inputGridsquare);
                string actual_ddmToGrid = cc.ConvertDDMtoGridsquare(inputDDM);
                if (actual_ddmToGrid == expected_Gridsquare && actual_ddmToGrid.Equals(expected_Gridsquare))
                {
                    ddmToGridPass = true;
                }
                if (actual_gridToDDM == expected_DDM && actual_gridToDDM.Equals(expected_DDM))
                {
                    gridToDDMpass = true;
                }
                Assert.IsTrue(ddmToGridPass == gridToDDMpass);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }
        [TestMethod]
        public void Test_montevideo()
        {
            string[] montevideo = { "34*54.6'S", "56*12.7'W", "GF15vc" };
            string inputGridsquare = montevideo[2].ToString();
            string inputDDM = $"{montevideo[0]},{montevideo[1]}";
            string expected_Gridsquare = montevideo[2].ToString();
            string expected_DDM = $"{montevideo[0]},{montevideo[1]}";
            bool gridToDDMpass = false;
            bool ddmToGridPass = false;
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string actual_gridToDDM = cc.ConvertGridsquareToDDM(inputGridsquare);
                string actual_ddmToGrid = cc.ConvertDDMtoGridsquare(inputDDM);
                if (actual_ddmToGrid == expected_Gridsquare && actual_ddmToGrid.Equals(expected_Gridsquare))
                {
                    ddmToGridPass = true;
                }
                if (actual_gridToDDM == expected_DDM && actual_gridToDDM.Equals(expected_DDM))
                {
                    gridToDDMpass = true;
                }
                Assert.IsTrue(ddmToGridPass == gridToDDMpass);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }
        [TestMethod]
        public void Test_washingonDC()
        {
            string[] washingonDC = { "38*55.2'N", "77*3.9'W", "FM18lw" };
            string inputGridsquare = washingonDC[2].ToString();
            string inputDDM = $"{washingonDC[0]},{washingonDC[1]}";
            string expected_Gridsquare = washingonDC[2].ToString();
            string expected_DDM = $"{washingonDC[0]},{washingonDC[1]}";
            bool gridToDDMpass = false;
            bool ddmToGridPass = false;
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string actual_gridToDDM = cc.ConvertGridsquareToDDM(inputGridsquare);
                string actual_ddmToGrid = cc.ConvertDDMtoGridsquare(inputDDM);
                if (actual_ddmToGrid == expected_Gridsquare && actual_ddmToGrid.Equals(expected_Gridsquare))
                {
                    ddmToGridPass = true;
                }
                if (actual_gridToDDM == expected_DDM && actual_gridToDDM.Equals(expected_DDM))
                {
                    gridToDDMpass = true;
                }
                Assert.IsTrue(ddmToGridPass == gridToDDMpass);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }
        [TestMethod]
        public void Test_wellington()
        {
            string[] wellingon = { "41*17.0'S", "174*44.7'E", "RE78ir" };
            string inputGridsquare = wellingon[2].ToString();
            string inputDDM = $"{wellingon[0]},{wellingon[1]}";
            string expected_Gridsquare = wellingon[2].ToString();
            string expected_DDM = $"{wellingon[0]},{wellingon[1]}";
            bool gridToDDMpass = false;
            bool ddmToGridPass = false;
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string actual_gridToDDM = cc.ConvertGridsquareToDDM(inputGridsquare);
                string actual_ddmToGrid = cc.ConvertDDMtoGridsquare(inputDDM);
                if (actual_ddmToGrid == expected_Gridsquare && actual_ddmToGrid.Equals(expected_Gridsquare))
                {
                    ddmToGridPass = true;
                }
                if (actual_gridToDDM == expected_DDM && actual_gridToDDM.Equals(expected_DDM))
                {
                    gridToDDMpass = true;
                }
                Assert.IsTrue(ddmToGridPass == gridToDDMpass);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }
        [TestMethod]
        public void Test_ValidateGridsquareInputBad_JumbledMatch()
        {
            {
                string inputGridsquare = "783lkfCN87ut...ab";
                string expectedResult = $"CN87UT";
                CoordinateConverter cc = new CoordinateConverter();
                string actualResult = "failed";
                if (cc.ValidateGridsquareInput(inputGridsquare))
                {
                    actualResult = cc.Gridsquare;
                }
                Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
            }
        }
        [TestMethod]
        public void Test_ValidateGridsquareInput_TooFewChars()
        {
            string inputGridsquare = "CN87u";
            string expectedResult = $"failed";
            CoordinateConverter cc = new CoordinateConverter();
            string actualResult = "failed";
            if (cc.ValidateGridsquareInput(inputGridsquare))
            {
                actualResult = cc.Gridsquare;
            }
            Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
        }
        [TestMethod]
        public void Test_GetLonDegrees_FirstPortion()
        {   // Lon DDM Degrees
            string inputGridsquare = "CN87ut";
            string[] expectedResults = new string[2] { "-1", "-120" };
            string[] actualResults = new string[2];  // concatenate stringified versions of output from cc
            bool resultsMatch = false;
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                // sets LonDirection and DDM_LonDegrees
                cc.SetGridsquare(inputGridsquare);
                cc.GetLonDegrees();
                actualResults[0] = $"{cc.LonDirection}"; // cc.LonDirection.ToString();
                actualResults[1] = $"{cc.DDMlonDegrees}"; // cc.DDM_LonDegrees.ToString();
                if (expectedResults[0].ToString() == actualResults[0].ToString())
                {
                    if (expectedResults[1].ToString() == actualResults[1].ToString())
                    {
                        resultsMatch = true;
                    }
                }
                Assert.IsTrue(resultsMatch);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false!");
            }
        }
        [TestMethod]
        public void Test_GetLonDegrees_SecondPortion()
        {   // Lon DDM Degrees
            string inputGridsquare = "CN87ut";
            string[] expectedResults = new string[2] { "-1", "-122" };
            string[] actualResults = new string[2];
            bool resultsMatch = false;
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                cc.SetGridsquare(inputGridsquare);
                cc.GetLonDegrees();
                cc.AddLonDegreesRemainder();
                actualResults[0] = $"{cc.LonDirection}";
                actualResults[1] = $"{cc.DDMlonDegrees}";
                if (expectedResults[0].ToString() == actualResults[0].ToString())
                {
                    if (expectedResults[1].ToString() == actualResults[1].ToString())
                    {
                        resultsMatch = true;
                    }
                }
                Assert.IsTrue(resultsMatch);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false!");
            }
        }
        [TestMethod]
        public void Test_GetLonMinutes()
        {   // Lon DDM Minutes
            string inputGridsquare = "CN87ut";
            string expectedResult = "17.5";
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                cc.SetGridsquare(inputGridsquare);
                cc.GetLonDegrees();
                cc.AddLonDegreesRemainder();
                cc.GetLonMinutes();
                string actualResult = $"{cc.DDMlonMinutes}";
                Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false!");
            }
        }
        [TestMethod]
        public void Test_GetLatDegrees_FirstPortion()
        {   // Lon DDM Degrees
            string inputGridsquare = "CN87ut";
            string[] expectedResults = new string[2] { "1", "40" };
            string[] actualResults = new string[2];  // concatenate stringified versions of output from cc
            bool resultsMatch = false;
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                // sets LonDirection and DDM_LonDegrees
                cc.SetGridsquare(inputGridsquare);
                cc.GetLatDegrees();
                actualResults[0] = $"{cc.LatDirection}"; // cc.LonDirection.ToString();
                actualResults[1] = $"{cc.DDMlatDegrees}"; // cc.DDM_LonDegrees.ToString();
                if (expectedResults[0].ToString() == actualResults[0].ToString())
                {
                    if (expectedResults[1].ToString() == actualResults[1].ToString())
                    {
                        resultsMatch = true;
                    }
                }
                Assert.IsTrue(resultsMatch);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false!");
            }
        }
        [TestMethod]
        public void Test_GetLatDegrees_SecondPortion()
        {   // Lon DDM Degrees
            string inputGridsquare = "CN87ut";
            string[] expectedResults = new string[2] { "1", "47" };
            string[] actualResults = new string[2];
            bool resultsMatch = false;
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                cc.SetGridsquare(inputGridsquare);
                cc.GetLatDegrees();
                cc.AddLatDegreesRemainder();
                actualResults[0] = $"{cc.LatDirection}";
                actualResults[1] = $"{cc.DDMlatDegrees}";
                if (expectedResults[0].ToString() == actualResults[0].ToString())
                {
                    if (expectedResults[1].ToString() == actualResults[1].ToString())
                    {
                        resultsMatch = true;
                    }
                }
                Assert.IsTrue(resultsMatch);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false!");
            }
        }
        [TestMethod]
        public void Test_GetLatMinutes()
        {   // Lon DDM Minutes
            string inputGridsquare = "CN87ut";
            string expectedResult = "48.75";
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                cc.SetGridsquare(inputGridsquare);
                cc.GetLatDegrees();
                cc.AddLatDegreesRemainder();
                cc.GetLatMinutes();
                string actualResult = $"{cc.DDMlatMinutes}";
                Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false!");
            }
        }
        [TestMethod]
        public void Test_ConverGridsquareToDDM_alpha()
        {
            string inputGridsquare = "CN87ut";
            Dictionary<object, string> expectedResults = new Dictionary<object, string>(5);
            expectedResults.Add("CN87UT", "gridsquare");
            expectedResults.Add(47, "DDM_LatDegrees");
            expectedResults.Add(48.75, "DDM_LatDecimalMinutes");
            expectedResults.Add(-122, "DDM_LonDegrees");
            expectedResults.Add(17.5, "DDM_LonDecimalMinutes");
            string xrGridGood = "";
            int xrLatDGood = -999;
            int xrLonDGood = -999;
            double xrLatMGood = -99.99;
            double xrLonMGood = -99.99;
            if (CoordinateConverter.GenerateTableLookups())
            {
                CoordinateConverter cc = new CoordinateConverter();
                cc.ConvertGridsquareToDDM(inputGridsquare);
                if (expectedResults.TryGetValue(cc.Gridsquare, out var dictGrid))
                {
                    if (dictGrid == "gridsquare")
                    {
                        xrGridGood = cc.Gridsquare;
                    }
                }
                if (expectedResults.TryGetValue(cc.DDMlatDegrees, out var dictDDMLatD))
                {
                    if (dictDDMLatD == "DDM_LatDegrees")
                    {
                        xrLatDGood = cc.DDMlatDegrees;
                    }
                }
                if (expectedResults.TryGetValue(Math.Abs(cc.DDMlatMinutes), out var dictDDMLatM))
                {
                    if (dictDDMLatM == "DDM_LatDecimalMinutes")
                    {
                        xrLatMGood = cc.DDMlatMinutes;
                    }
                }
                if (expectedResults.TryGetValue(cc.DDMlonDegrees, out var dictDDMLonD))
                {
                    if (dictDDMLonD == "DDM_LonDegrees")
                    {
                        xrLonDGood = cc.DDMlonDegrees;
                    }
                }
                if (expectedResults.TryGetValue(Math.Abs(cc.DDMlonMinutes), out var dictDDMLonM))
                {
                    if (dictDDMLonM == "DDM_LonDecimalMinutes")
                    {
                        xrLonMGood = cc.DDMlonMinutes;
                    }
                }
                Console.WriteLine($"*** Tracked results ***");
                Console.WriteLine($"Validated Gridsquare: {xrGridGood}");
                Console.WriteLine($"Validated DDM Lat Degrees: {dictDDMLatD}");
                Console.WriteLine($"Validated DDM Lat Minutes: {dictDDMLatM}");
                Console.WriteLine($"Validated DDM Lon Degrees: {dictDDMLonD}");
                Console.WriteLine($"Validated DDM Lon Minutes: {dictDDMLonM}");
                Assert.IsTrue(xrGridGood.Length > 0 && xrLatDGood != -999 && xrLatMGood != -99.99 && xrLonDGood != -999 && xrLonMGood != -99.99);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false!");
            }
        }
        [TestMethod]
        public void Test_ConvertGridsquareToDDM_bravo()
        {
            string inputGridsquare = "CN87ut";
            string expectedResult = "47*48.75\"N,122*17.50\"W";
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string actualResult = cc.ConvertGridsquareToDDM(inputGridsquare);
                Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false!");
            }
        }
        [TestMethod]
        public void Test_GetLatDDM()
        {   // should return a string of Lat in DDM format
            string inputGridsquare = "CN87ut";
            string expectedResult = "47*48.75\"N";
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                cc.SetGridsquare(inputGridsquare);
                string actualResult = cc.GetLatDDM();
                Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false!");
            }
        }
        [TestMethod]
        public void Test_GetLonDDM()
        {   // should return a string of Lon in DDM format
            string inputGridsquare = "CN87ut";
            string expectedResult = "122*17.50\"W";
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                cc.SetGridsquare(inputGridsquare);
                string actualResult = cc.GetLonDDM();
                Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false!");
            }
        }
        [TestMethod]
        public void Test_ValidateDDMinput()
        {
            bool passed = false;
            string ddmCoordsInput = "47*48.75',-122*17.5'"; // comma index=9
            CoordinateConverter cc = new CoordinateConverter();
            //if (CoordinateConverter.GenerateTableLookups())
            if (cc.ValidateDDMinput(ddmCoordsInput))
            {
                passed = true;
            }
            else
            {
                passed = false;
            }
            Console.WriteLine($"Gridsquare: (Not calculated)\n" +
                              $"DDM Lat Degrees: {cc.DDMlatDegrees}\nDDM Lat Minutes: {cc.DDMlatMinutes}\nLat Direction: {cc.LatDirection}\n" +
                              $"DDM Lon Degrees: {cc.DDMlonDegrees}\nDDM Lon Minutes: {cc.DDMlonMinutes}\nLon Direction: {cc.LonDirection}");
            Assert.IsTrue(passed);
        }
        [TestMethod]
        public void Test_ConvertDDMtoGridsquare_PosNeg()
        {
            string ddmCoordsInput = "47*48.75',-122*17.5'";
            string expectedResult = "CN87ut";
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string actualResult = cc.ConvertDDMtoGridsquare(ddmCoordsInput);
                Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned 'false'.");
            }
        }
        [TestMethod]
        public void Test_convertDDMtoGridsquare_NESW()
        {
            string ddm_CoordsInput = "47*48.75'N,122*17.5'W";
            string expectedResult = "CN87ut";
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string actualResult = cc.ConvertDDMtoGridsquare(ddm_CoordsInput);
                Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned 'false'.");
            }
        }
        [TestMethod]
        public void Test_convertDDMtoGridsquare_multipleInputsSameObject()
        {
            string ddmCoordsInput1 = "47*48.75',-122*17.5'";     // CN78ut
            string ddmCoordsInput2 = "32*58.8',-105*44.0'";      // DM72dx
            string expectedResult1 = "CN87ut";
            string expectedResult2 = "DM72dx";
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string actualResult1 = cc.ConvertDDMtoGridsquare(ddmCoordsInput1);
                string actualResult2 = cc.ConvertDDMtoGridsquare(ddmCoordsInput2);
                if (expectedResult1.ToUpper() == actualResult1.ToUpper())
                {
                    string assertMsg = $"Expected Result 1: {expectedResult1}; Actual Result 1: {actualResult1}; "
                        + $"Expected Result 2: {expectedResult2}; Actual Result2: {actualResult2}";
                    Assert.IsFalse(expectedResult2 == actualResult2);   //, assertMsg);
                }
                else
                {
                    Assert.Fail($"Something went wrong and the correct results were not returned.");
                }
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned 'false'.");
            }
        }
        [TestMethod]
        public void Test_ConvertDDMtoGridsquare_multipleInputsAndObjects()
        {
            string ddmCoordsInput1 = "47*48.75',-122*17.5'";     // CN78ut
            string ddmCoordsInput2 = "32*58.8'N,105*44.0'W";      // DM72dx
            string expectedResult1 = "CN87ut";
            string expectedResult2 = "DM72dx";
            string actualResult1 = "";
            string actualResult2 = "";
            CoordinateConverter cc1 = new CoordinateConverter();
            CoordinateConverter cc2 = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                actualResult1 = cc1.ConvertDDMtoGridsquare(ddmCoordsInput1);
                actualResult2 = cc2.ConvertDDMtoGridsquare(ddmCoordsInput2);
                if (expectedResult1.ToUpper() == actualResult1.ToUpper())
                {
                    Assert.AreEqual(expectedResult2.ToUpper(), actualResult2.ToUpper(), "Separate objects produce non-concatenated Gridsquare output.");
                }
            }
            else
            {
                Assert.Fail($"Test failed. cc1 gridsquare: {actualResult1}; cc2 gridsquare: {actualResult2}.");
            }
        }
        [TestMethod]
        public void Test_ConvertDDtoDDM()
        {
            string dd_CoordsInput = "47.8125*N,122.2917*W";
            string expectedResult = "47*48.75'N,122*17.50'W";
            CoordinateConverter cc = new CoordinateConverter();
            string actualResult = cc.ConvertDDtoDDM(dd_CoordsInput);
            Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper(), $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
        }
        [TestMethod]
        public void Test_ConvertDMStoDDM()
        {
            string dms_CoordsInput = "47*48'45\"N,122*17'30\"W";    // 47*48'45\"N,122*17'30\"W
            string expectedResult = "47*48.75'N,122*17.50'W";        // 47*48.75'N,122*17.50'W // note forced 2-digit space in Decimal Minutes.
            CoordinateConverter cc = new CoordinateConverter();
            string actualResult = cc.ConvertDMStoDDM(dms_CoordsInput);
            Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper(), $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
        }
        [TestMethod]
        public void Test_ConvertDMStoDDM_Bravo()
        {
            string dms_CoordsInput = "47*48'45\"N,122*17'40\"W";    // 47*48'45\"N,122*17'40\"W
            string expectedResult = "47*48.75'N,122*17.67'W";        // 47*48.75'N,122*17.67'W
            CoordinateConverter cc = new CoordinateConverter();
            string actualResult = cc.ConvertDMStoDDM(dms_CoordsInput);
            Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper(), $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
        }
        [TestMethod]
        public void Test_DDtoGridsquare()
        {
            string expectedResult = $"CN87ut";  // six-character gridsquare
            string dd_CoordsInput = "47.8125*N,122.2917*W"; // decimal degrees
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string actualResult = cc.ConvertDDtoGridsquare(dd_CoordsInput);
                Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper(), $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false.");
            }
        }
        [TestMethod]
        public void Test_DMStoGridsquare()
        {
            string expectedResult = $"CN87ut";  // six-character gridsquare
            string dms_LatLongInput = "47*48'45\"N,122*17'30\"W"; // decimal degrees
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string actualResult = cc.ConvertDMStoGridsquare(dms_LatLongInput);
                Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper(), $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false.");
            }
        }
    }
}
