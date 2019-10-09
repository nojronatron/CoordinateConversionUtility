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
        // Console.WriteLine($"Input Gridsquare: {inputGridsquare}");
        // Console.WriteLine($"Expected Result: {expectedResult}");
        // Console.WriteLine($"Actual Result: {actualResult}");
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
            Assert.IsTrue(expectedResult == actualResult);
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
                Assert.IsTrue(expectedResult == actualResult);
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
            Assert.IsTrue(expectedResult == actualResult);
        }
        [TestMethod]
        public void Test_GetLonDegrees_FirstPortion()
        {   // Lon DDM Degrees
            string inputGridsquare = "CN87ut";
            string[] expectedResults = new string[2]{ "-1", "-120" };
            string[] actualResults = new string[2];  // concatenate stringified versions of output from cc
            bool resultsMatch = false;
            CoordinateConverter cc = new CoordinateConverter();
            CoordinateConverter.GenerateTableLookups();
            // sets LonDirection and DDM_LonDegrees
            cc.SetGridsquare(inputGridsquare);
            cc.GetLonDegrees_FirstPortion();
            actualResults[0] = $"{cc.LonDirection}"; // cc.LonDirection.ToString();
            actualResults[1] = $"{cc.DDM_LonDegrees}"; // cc.DDM_LonDegrees.ToString();
            if (expectedResults[0].ToString() == actualResults[0].ToString())
            {
                if (expectedResults[1].ToString() == actualResults[1].ToString())
                {
                    resultsMatch = true;
                }
            }
            Assert.IsTrue(resultsMatch);
        }
        [TestMethod]
        public void Test_GetLonDegrees_SecondPortion()
        {   // Lon DDM Degrees
            string inputGridsquare = "CN87ut";
            string[] expectedResults = new string[2] { "-1", "-122" };
            string[] actualResults = new string[2];
            bool resultsMatch = false;
            CoordinateConverter cc = new CoordinateConverter();
            CoordinateConverter.GenerateTableLookups();
            cc.SetGridsquare(inputGridsquare);
            cc.GetLonDegrees_FirstPortion();
            cc.AddLonDegrees_SecondPortion();
            actualResults[0] = $"{cc.LonDirection}";
            actualResults[1] = $"{cc.DDM_LonDegrees}";
            if (expectedResults[0].ToString() == actualResults[0].ToString())
            {
                if (expectedResults[1].ToString() == actualResults[1].ToString())
                {
                    resultsMatch = true;
                }
            }
            Assert.IsTrue(resultsMatch);
        }
        [TestMethod]
        public void Test_GetLonMinutes()
        {   // Lon DDM Minutes
            string inputGridsquare = "CN87ut";
            string expectedResult = "17.5";
            CoordinateConverter cc = new CoordinateConverter();
            CoordinateConverter.GenerateTableLookups();
            cc.SetGridsquare(inputGridsquare);
            cc.GetLonDegrees_FirstPortion();
            cc.AddLonDegrees_SecondPortion();
            cc.GetLonMinutes();
            string actualResult = $"{cc.DDM_LonMinutes}";
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Test_GetLatDegrees_FirstPortion()
        {   // Lon DDM Degrees
            string inputGridsquare = "CN87ut";
            string[] expectedResults = new string[2] { "1", "40" };
            string[] actualResults = new string[2];  // concatenate stringified versions of output from cc
            bool resultsMatch = false;
            CoordinateConverter cc = new CoordinateConverter();
            CoordinateConverter.GenerateTableLookups();
            // sets LonDirection and DDM_LonDegrees
            cc.SetGridsquare(inputGridsquare);
            cc.GetLatDegrees_FirstPortion();
            actualResults[0] = $"{cc.LatDirection}"; // cc.LonDirection.ToString();
            actualResults[1] = $"{cc.DDM_LatDegrees}"; // cc.DDM_LonDegrees.ToString();
            if (expectedResults[0].ToString() == actualResults[0].ToString())
            {
                if (expectedResults[1].ToString() == actualResults[1].ToString())
                {
                    resultsMatch = true;
                }
            }
            Assert.IsTrue(resultsMatch);
        }
        [TestMethod]
        public void Test_GetLatDegrees_SecondPortion()
        {   // Lon DDM Degrees
            string inputGridsquare = "CN87ut";
            string[] expectedResults = new string[2] { "1", "47" };
            string[] actualResults = new string[2];
            bool resultsMatch = false;
            CoordinateConverter cc = new CoordinateConverter();
            CoordinateConverter.GenerateTableLookups();
            cc.SetGridsquare(inputGridsquare);
            cc.GetLatDegrees_FirstPortion();
            cc.AddLatDegrees_SecondPortion();
            actualResults[0] = $"{cc.LatDirection}";
            actualResults[1] = $"{cc.DDM_LatDegrees}";
            if (expectedResults[0].ToString() == actualResults[0].ToString())
            {
                if (expectedResults[1].ToString() == actualResults[1].ToString())
                {
                    resultsMatch = true;
                }
            }
            Assert.IsTrue(resultsMatch);
        }
        [TestMethod]
        public void Test_GetLatMinutes()
        {   // Lon DDM Minutes
            string inputGridsquare = "CN87ut";
            string expectedResult = "48.75";
            CoordinateConverter cc = new CoordinateConverter();
            CoordinateConverter.GenerateTableLookups();
            cc.SetGridsquare(inputGridsquare);
            cc.GetLatDegrees_FirstPortion();
            cc.AddLatDegrees_SecondPortion();
            cc.GetLatMinutes();
            string actualResult = $"{cc.DDM_LatMinutes}";
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Test_ConverGridsquareToDDM_alpha()
        {
            string inputGridsquare = "CN87ut";
            Dictionary<object, string> expectedResults = new Dictionary<object, string>(5);
            expectedResults.Add("CN87UT","gridsquare");
            expectedResults.Add(47, "DDM_LatDegrees");
            expectedResults.Add(48.75, "DDM_LatDecimalMinutes");
            expectedResults.Add(-122, "DDM_LonDegrees");
            expectedResults.Add(17.5, "DDM_LonDecimalMinutes");
            string xrGridGood = "";
            int xrLatDGood = -999;
            int xrLonDGood = -999;
            double xrLatMGood = -99.99;
            double xrLonMGood = -99.99;
            CoordinateConverter.GenerateTableLookups();
            CoordinateConverter cc = new CoordinateConverter();
            cc.ConvertGridsquareToDDM(inputGridsquare);
            if (expectedResults.TryGetValue(cc.Gridsquare, out var dictGrid))
            {
                if (dictGrid == "gridsquare")
                {
                    xrGridGood = cc.Gridsquare;
                }
            }
            if (expectedResults.TryGetValue(cc.DDM_LatDegrees, out var dictDDMLatD))
            {
                if (dictDDMLatD == "DDM_LatDegrees")
                {
                    xrLatDGood = cc.DDM_LatDegrees;
                }
            }
            if (expectedResults.TryGetValue(Math.Abs(cc.DDM_LatMinutes), out var dictDDMLatM))
            {
                if (dictDDMLatM == "DDM_LatDecimalMinutes")
                {
                    xrLatMGood = cc.DDM_LatMinutes;
                }
            }
            if (expectedResults.TryGetValue(cc.DDM_LonDegrees, out var dictDDMLonD))
            {
                if (dictDDMLonD == "DDM_LonDegrees")
                {
                    xrLonDGood = cc.DDM_LonDegrees;
                }
            }
            if (expectedResults.TryGetValue(Math.Abs(cc.DDM_LonMinutes), out var dictDDMLonM))
            {
                if (dictDDMLonM == "DDM_LonDecimalMinutes")
                {
                    xrLonMGood = cc.DDM_LonMinutes;
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
        [TestMethod]
        public void Test_ConvertGridsquareToDDM_bravo()
        {
            string inputGridsquare = "CN87ut";
            string expectedResult = "47*48.75\"N,122*17.5\"W";
            CoordinateConverter cc = new CoordinateConverter();
            CoordinateConverter.GenerateTableLookups();
            string actualResult = cc.ConvertGridsquareToDDM(inputGridsquare);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Test_GetLatDDM()
        {   // should return a string of Lat in DDM format
            string inputGridsquare = "CN87ut";
            string expectedResult = "47*48.75\"N";
            CoordinateConverter cc = new CoordinateConverter();
            CoordinateConverter.GenerateTableLookups();
            cc.SetGridsquare(inputGridsquare);
            string actualResult = cc.GetLatDDM();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Test_GetLonDDM()
        {   // should return a string of Lon in DDM format
            string inputGridsquare = "CN87ut";
            string expectedResult = "122*17.5\"W";
            CoordinateConverter cc = new CoordinateConverter();
            CoordinateConverter.GenerateTableLookups();
            cc.SetGridsquare(inputGridsquare);
            string actualResult = cc.GetLonDDM();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Test_ValidateDDMinput()
        {
            bool passed = false;
            string ddmCoordsInput = "47*48.75',-122*17.5'"; // comma index=9
            CoordinateConverter cc = new CoordinateConverter();
            CoordinateConverter.GenerateTableLookups();
            if (cc.ValidateDDMinput(ddmCoordsInput))
            {
                passed = true;
            }
            else
            {
                passed = false;
            }
            Console.WriteLine($"Gridsquare: (Not calculated)\n" +
                              $"DDM Lat Degrees: {cc.DDM_LatDegrees}\nDDM Lat Minutes: {cc.DDM_LatMinutes}\nLat Direction: {cc.LatDirection}\n" +
                              $"DDM Lon Degrees: {cc.DDM_LonDegrees}\nDDM Lon Minutes: {cc.DDM_LonMinutes}\nLon Direction: {cc.LonDirection}");
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
                Assert.AreEqual(expectedResult, actualResult);
            }
            else
            {
                Assert.IsTrue(false, "Method GenerateTableLookups() returned 'false'.");
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
                Assert.AreEqual(expectedResult, actualResult);
            }
            else
            {
                Assert.IsTrue(false, "Method GenerateTableLookups() returned 'false'.");
            }
        }
        [TestMethod]
        public void Test_ConvertDDtoDDM()
        {
            string dd_CoordsInput = "47.8125*N,122.2917*W";
            string expectedResult = "47*48.75'N,122*17.5'W";
            CoordinateConverter cc = new CoordinateConverter();
            string actualResult = cc.ConvertDDtoDDM(dd_CoordsInput);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Test_ConvertDMStoDDM()
        {
            string dms_CoordsInput = "47*48'45\"N,122*17'30\"W";
            string expectedResult = "47*48.75'N,122*17.5'W";
            CoordinateConverter cc = new CoordinateConverter();
            string actualResult = cc.ConvertDMStoDDM(dms_CoordsInput);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
