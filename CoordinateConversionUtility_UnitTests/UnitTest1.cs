using CoordinateConversionUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateConversionUtility_UnitTests.TestModels;
using CoordinateConversionUtility.Models;

namespace CoordinateConversionUtility_UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private static char DegreesSymbol { get { return (char)176; } }     //  degree symbol
        private static char MinutesSymbol { get { return (char)39; } }      //  single quote
        private static char SecondsSymbol { get { return (char)34; } }      //  double quote

        [TestMethod]
        public void Test_DDCoordinateHelper_DecimalIn_StringOut()
        {
            WellingtonCoordinateModel wcm = new WellingtonCoordinateModel();
            decimal inputLattitude = wcm.DegreesLat;
            decimal inputLongitude = wcm.DegreesLon;
            DDCoordinate dDCoordindate = new DDCoordinate(inputLattitude, inputLongitude);
            string expectedResult = WellingtonCoordinateModel.strDD();
            string actualResult = dDCoordindate.ToString();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void Test_DDCoordinateHelper_StringIn_ObjectOut()
        {
            string strInputLatAndLon = WellingtonCoordinateModel.strDD();
            string expectedResult = "CoordinateConversionUtility.Models.DDCoordinate";
            DDCoordinate dDCoordindate = ConversionHelper.StringToDD(strInputLatAndLon);
            Console.WriteLine($"{ dDCoordindate.GetType() }");
            Assert.AreEqual(expectedResult, dDCoordindate.GetType().FullName.ToString());
        }
        [TestMethod]
        public void Test_DDCoordinateHelper_StringIn_ToStringOut()
        {
            //  strWellingtonDD has the expected result
            string strInputLatAndLon = WellingtonCoordinateModel.strDD();
            string expectedResult = $"{ WellingtonCoordinateModel.strDD() }";
            DDCoordinate dDCoordindate = ConversionHelper.StringToDD(WellingtonCoordinateModel.strDD());
            //DDCoordinate dDCoordindate = new DDCoordinate(WellingtonCoordinateModel.strDD());
            Assert.AreEqual(expectedResult, dDCoordindate.ToString());
        }
        [TestMethod]
        public void Test_ValidateGridsquareInput_ValidatesTrue()
        {
            string inputGridsquare = "CN87ut";
            string expectedResult = $"CN87UT";
            CoordinateConverter cc = new CoordinateConverter();
            string actualResult = "failed";
            if (cc.ValidateGridsquareInput(inputGridsquare))
            {
                actualResult = cc.Gridsquare.ToString();
            }
            Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
        }
        [TestMethod]
        public void Test_Munich_ConvertGridsquareToDDM_Passes()
        {
            CoordinateConverter cc = new CoordinateConverter();
            string gridsquare = MunichCoordinatesModel.strGridSquare();
            string expectedResult = MunichCoordinatesModel.strDDM();
            string actualResult = "failed";

            if (CoordinateConverter.GenerateTableLookups())
            {
                actualResult = cc.ConvertGridsquareToDDM(gridsquare);
                Assert.AreEqual(expectedResult, actualResult);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }
        [TestMethod]
        public void Test_Munich_ConvertDDMtoGridsquare_Passes()
        {
            CoordinateConverter cc = new CoordinateConverter();
            string ddmInput = MunichCoordinatesModel.strDDM();
            string expectedResult = MunichCoordinatesModel.strGridSquare();
            string actualResult = "failed";

            if (CoordinateConverter.GenerateTableLookups())
            {
                DDMCoordinate munichDDM = new DDMCoordinate(ddmInput);
                actualResult = cc.ConvertDDMtoGridsquare(munichDDM);
                Assert.AreEqual(expectedResult, actualResult);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }
        [TestMethod]
        public void Test_Montevideo_ConvertGridsquareToDDM_Passes()
        {
            CoordinateConverter cc = new CoordinateConverter();
            string gridsquare = MontevideoCoordinateModel.strGridsquare();
            string expectedResult = MontevideoCoordinateModel.strDDM();
            string actualResult = "failed";

            if (CoordinateConverter.GenerateTableLookups())
            {
                actualResult = cc.ConvertGridsquareToDDM(gridsquare);
                Assert.AreEqual(expectedResult, actualResult);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }
        [TestMethod]
        public void Test_Montevideo_ConvertDDMtoGridsquare_Passes()
        {
            CoordinateConverter cc = new CoordinateConverter();
            string ddmInput = MontevideoCoordinateModel.strMidGridDDM();
            string expectedResult = MontevideoCoordinateModel.strGridsquare();
            string actualResult = "failed";
            if (CoordinateConverter.GenerateTableLookups())
            {
                DDMCoordinate montevideoDDM = new DDMCoordinate(ddmInput);
                actualResult = cc.ConvertDDMtoGridsquare(montevideoDDM);
                Assert.AreEqual(expectedResult, actualResult);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }
        [TestMethod]
        public void Test_WashingtonDC_ConvertGridsquareToDDM_Passes()
        {
            CoordinateConverter cc = new CoordinateConverter();
            string gridsquare = WashingtondcCoordinateModel.strGridsquare();
            string expectedResult = WashingtondcCoordinateModel.strDDM();
            string actualResult = "failed";

            if (CoordinateConverter.GenerateTableLookups())
            {
                actualResult = cc.ConvertGridsquareToDDM(gridsquare);
                Assert.AreEqual(expectedResult, actualResult);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }
        [TestMethod]
        public void Test_WashingtonDC_ConvertDDMtoGridsquare_Passes()
        {
            CoordinateConverter cc = new CoordinateConverter();
            string ddmInput = WashingtondcCoordinateModel.strArrlDDM();
            string expectedResult = WashingtondcCoordinateModel.strGridsquare();
            string actualResult = "failed";

            if (CoordinateConverter.GenerateTableLookups())
            {
                DDMCoordinate washingtonDcDDM = new DDMCoordinate(ddmInput);
                actualResult = cc.ConvertDDMtoGridsquare(washingtonDcDDM);
                Assert.AreEqual(expectedResult, actualResult);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }
        [TestMethod]
        public void Test_WellingtonNZ_ConvertGridsquareToDDM_Passes()
        {
            CoordinateConverter cc = new CoordinateConverter();
            string gridsquare = WellingtonCoordinateModel.strGridsquare();
            string expectedResult = WellingtonCoordinateModel.strDDM();
            string actualResult = "failed";

            if (CoordinateConverter.GenerateTableLookups())
            {
                actualResult = cc.ConvertGridsquareToDDM(gridsquare);
                Assert.AreEqual(expectedResult, actualResult);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }

        [TestMethod]
        public void Test_WellingtonNZ_ConvertMidGridDDMtoGridsquare_Passes()
        {
            CoordinateConverter cc = new CoordinateConverter();
            string ddmInput = WellingtonCoordinateModel.strArrlDDM();
            string expectedResult = WellingtonCoordinateModel.strGridsquare();
            string actualResult = "failed";

            if (CoordinateConverter.GenerateTableLookups())
            {
                DDMCoordinate WellingtonDDM = new DDMCoordinate(ddmInput);
                actualResult = cc.ConvertDDMtoGridsquare(WellingtonDDM);
                Assert.AreEqual(expectedResult, actualResult);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }

        [TestMethod]
        public void Test_SanClemente_ConvertGridsquareToDDM_Passes()
        {
            CoordinateConverter cc = new CoordinateConverter();
            string gridsquare = SanClementeCoordinatesModel.strGridsquare();
            string expectedResult = SanClementeCoordinatesModel.strDDM();
            string actualResult = "failed";

            if (CoordinateConverter.GenerateTableLookups())
            {
                actualResult = cc.ConvertGridsquareToDDM(gridsquare);
                Assert.AreEqual(expectedResult, actualResult);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }
        [TestMethod]
        public void Test_SanClemente_ConvertDDMtoGridsquare_Passes()
        {
            CoordinateConverter cc = new CoordinateConverter();
            string ddmInput = SanClementeCoordinatesModel.strArrlDDM();
            string expectedResult = SanClementeCoordinatesModel.strGridsquare();
            string actualResult = "failed";

            if (CoordinateConverter.GenerateTableLookups())
            {
                DDMCoordinate WellingtonDDM = new DDMCoordinate(ddmInput);
                actualResult = cc.ConvertDDMtoGridsquare(WellingtonDDM);
                Assert.AreEqual(expectedResult, actualResult);
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
                    actualResult = cc.Gridsquare.ToString();
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
                actualResult = cc.Gridsquare.ToString();
            }
            Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
        }
        [TestMethod]
        public void Test_GetLonDegrees_FirstPortion()
        {   // Lon DDM Degrees
            string inputGridsquare = "CN87ut";
            string[] expectedResults = new string[2] { "-1", "120" };
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
                Assert.IsTrue(resultsMatch, $"expectedResults: {expectedResults[0]}, {expectedResults[1]};" +
                    $" actualResults: {actualResults[0]}, {actualResults[1]}.");
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
            string[] expectedResults = new string[2] { "-1", "122" };
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
                Assert.IsTrue(resultsMatch, $"expectedResults: {expectedResults[0]}, {expectedResults[1]};" +
                    $" actualResults: {actualResults[0]}, {actualResults[1]}.");
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
        public void Test_DDMtoGridsquare()
        {
            string expectedResult = "CN87ut";  // six-character gridsquare
            LynnwoodCoordinatesModel lcm = new LynnwoodCoordinatesModel();
            DDMCoordinate lynnwood = new DDMCoordinate(LynnwoodCoordinatesModel.strDDM());
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string actualResult = cc.ConvertDDMtoGridsquare(lynnwood);
                Assert.AreEqual(expectedResult.ToUpper(), actualResult.ToUpper(), $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
            }
            else
            {
                Assert.Fail("GenerateTableLookups() returned false.");
            }
        }
    }
}
