using CoordinateConversionUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateConversionUtility_UnitTests.TestModels;

namespace CoordinateConversionUtility_UnitTests
{
    [TestClass]
    public class CoordConvUtilUnitTests
    {
        public WellingtonCoordinateModel wcm = new WellingtonCoordinateModel();
        public WashingtondcCoordinateModel wdccm = new WashingtondcCoordinateModel();
        public SanClementeCoordinatesModel sccm = new SanClementeCoordinatesModel();
        public MunichCoordinatesModel mcm = new MunichCoordinatesModel();
        public MontevideoCoordinateModel mvcm = new MontevideoCoordinateModel();
        public LynnwoodCoordinatesModel lcm = new LynnwoodCoordinatesModel();

        [TestMethod]
        public void Test_ExploringRefactoredHelpersAndModels()
        {
            Console.WriteLine($"Wellington Decimal Degrees Lattitude: { wcm.DegreesLat() }.");
            Console.WriteLine($"Wellington Decimal Degrees Longitude: { wcm.DegreesLon() }.");
            Console.WriteLine($"Wellington ARRL DDM: { wcm.ArrlDDM() }.");
            Console.WriteLine($"Wellington ARRL Gridsquare: { wcm.ArrlGridsquare() }.");
            Console.WriteLine($"Wellington Calculated DDM { wcm.CalculatedDDM() }.");
            Console.WriteLine($"Wellington Attainable DDM { wcm.AttainableDDM() }.");
            Console.WriteLine($"Wellington GoogleMapDD: { wcm.GoogleMapsDD() }.");
            Console.WriteLine($"Wellington GoogleMapDMS: { wcm.GoogleMapsDMS() }.");
            Assert.AreEqual(true, true, "This is a test");
        }
        [TestMethod]
        public void Test_DDCoordinateHelper_DecimalIn_StringOut()
        {
            decimal inputLattitude = wcm.DegreesLat();
            decimal inputLongitude = wcm.DegreesLon();
            DDCoordindateHelper dDCoordindate = new DDCoordindateHelper(inputLattitude, inputLongitude);
            string actualResult = dDCoordindate.ToString();
            Assert.AreEqual("System.String", actualResult.GetType().FullName);
        }
        [TestMethod]
        public void Test_DDCoordinateHelper_StringIn_ObjectOut()
        {
            string strInputLatAndLon = wcm.GoogleMapsDD();
            string expectedResult = "CoordinateConversionUtility.DDCoordindateHelper";
            DDCoordindateHelper dDCoordindate = new DDCoordindateHelper(strInputLatAndLon);
            Console.WriteLine($"{ dDCoordindate.GetType() }");
            Assert.AreEqual(expectedResult, dDCoordindate.GetType().FullName.ToString());
        }
        [TestMethod]
        public void Test_DDCoordinateHelper_StringIn_ToStringOut()
        {
            string expectedResult = $"{ wcm.GoogleMapsDD() }";
            DDCoordindateHelper dDCoordindate = new DDCoordindateHelper(wcm.GoogleMapsDD());
            Assert.AreEqual(expectedResult, dDCoordindate.ToString());
        }
        //[TestMethod]
        //public void Test_ValidateGridsquareInput_ValidatesTrue()
        //{
        //    string inputGridsquare = "CN87ut";
        //    CoordinateConverter cc = new CoordinateConverter();
        //    if (cc.ValidateGridsquareInput(inputGridsquare))
        //    {
        //        string expectedResult = $"CN87UT";
        //        string actualResult = cc.Gridsquare.ToString();
        //        Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
        //    }
        //    else
        //    {
        //        Assert.Fail("ValidateGridsquareinput returned False");
        //    }
        //}
        [TestMethod]
        public void Test_Munich_ConvertGridsquareToDDM_Passes()
        {   //  TODO: fix the CoordinateConverter code that overshoots the AttainableDDM
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string gridsquare = mcm.ArrlGridsquare();
                string expectedResult = mcm.AttainableDDM();
                string actualResult = cc.ConvertGridsquareToDDM(gridsquare);
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
            if (CoordinateConverter.GenerateTableLookups())
            {
                string ddmInput = mcm.ArrlDDM();
                string expectedResult = mcm.ArrlGridsquare();
                DDMCoordinateHelper munichDDM = new DDMCoordinateHelper(ddmInput);
                string actualResult = cc.ConvertDDMtoGridsquare(munichDDM);
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
            if (CoordinateConverter.GenerateTableLookups())
            {
                string gridsquare = mvcm.ArrlGridsquare();
                string expectedResult = mvcm.AttainableDDM();
                string actualResult = cc.ConvertGridsquareToDDM(gridsquare);
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
            if (CoordinateConverter.GenerateTableLookups())
            {
                string ddmInput = mvcm.ArrlDDM();
                string expectedResult = mvcm.ArrlGridsquare();
                DDMCoordinateHelper montevideoDDM = new DDMCoordinateHelper(ddmInput);
                string actualResult = cc.ConvertDDMtoGridsquare(montevideoDDM);
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
            if (CoordinateConverter.GenerateTableLookups())
            {
                string gridsquare = wdccm.ArrlGridsquare();
                string expectedResult = wdccm.AttainableDDM();
                string actualResult = cc.ConvertGridsquareToDDM(gridsquare);
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
            if (CoordinateConverter.GenerateTableLookups())
            {
                string ddmInput = wdccm.ArrlDDM();
                string expectedResult = wdccm.ArrlGridsquare();
                DDMCoordinateHelper washingtonDcDDM = new DDMCoordinateHelper(ddmInput);
                string actualResult = cc.ConvertDDMtoGridsquare(washingtonDcDDM);
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
            if (CoordinateConverter.GenerateTableLookups())
            {
                string gridsquare = wcm.ArrlGridsquare();
                string expectedresult = wcm.AttainableDDM();
                string actualResult = cc.ConvertGridsquareToDDM(gridsquare);
                Assert.AreEqual(expectedresult, actualResult);
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
            if (CoordinateConverter.GenerateTableLookups())
            {
                string ddmInput = wcm.ArrlDDM();
                string expectedResult = wcm.ArrlGridsquare();
                DDMCoordinateHelper WellingtonDDM = new DDMCoordinateHelper(ddmInput);
                string actualResult = cc.ConvertDDMtoGridsquare(WellingtonDDM);
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
            if (CoordinateConverter.GenerateTableLookups())
            {
                string gridsquare = sccm.ArrlGridsquare();
                string expectedResult = sccm.AttainableDDM();
                string actualResult = cc.ConvertGridsquareToDDM(gridsquare);
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
            if (CoordinateConverter.GenerateTableLookups())
            {
                string ddmInput = sccm.ArrlDDM();
                string expectedResult = sccm.ArrlGridsquare();
                DDMCoordinateHelper WellingtonDDM = new DDMCoordinateHelper(ddmInput);
                string actualResult = cc.ConvertDDMtoGridsquare(WellingtonDDM);
                Assert.AreEqual(expectedResult, actualResult);
            }
            else
            {
                Assert.Fail("GenerateTableLookups() method failed, test aborted!");
            }
        }
        //[TestMethod]
        //public void Test_ValidateGridsquareInputBad_JumbledMatch()
        //{
        //    {
        //        string inputGridsquare = "783lkfCN87ut...ab";
        //        CoordinateConverter cc = new CoordinateConverter();
        //        if (cc.ValidateGridsquareInput(inputGridsquare))
        //        {
        //            string expectedResult = $"CN87UT";
        //            string actualResult = cc.Gridsquare.ToString();
        //            Assert.AreEqual(expectedResult, actualResult, $"Expected Result: {expectedResult}; Actual Result: {actualResult}");
        //        }
        //        else
        //        {
        //            Assert.Fail("ValidateGridsquareinput() returned False.");
        //        }
        //    }
        //}
        //[TestMethod]
        //public void Test_ValidateGridsquareInput_TooFewChars()
        //{
        //    string FiveCharGridsquare = "CN87u";
        //    CoordinateConverter cc = new CoordinateConverter();
        //    Assert.IsFalse(cc.ValidateGridsquareInput(FiveCharGridsquare));
        //}
        [TestMethod]
        public void Test_GetLonDegrees_FirstPortion()
        {   // TODO: Rewrite this test to SIMPLIFY it and remove all the logic
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                // sets LonDirection and DDM_LonDegrees
                string inputGridsquare = "CN87ut";
                string[] expectedResults = new string[2] { "-1", "120" };
                string[] actualResults = new string[2];  // concatenate stringified versions of output from cc
                bool resultsMatch = false;
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
        {   // TODO: Rewrite this test to SIMPLIFY it and remove all the logic
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string inputGridsquare = "CN87ut";
                cc.SetGridsquare(inputGridsquare);
                cc.GetLonDegrees();
                cc.AddLonDegreesRemainder();
                string[] expectedResults = new string[2] { "-1", "122" };
                string[] actualResults = new string[2];
                bool resultsMatch = false;
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
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string inputGridsquare = "CN87ut";
                cc.SetGridsquare(inputGridsquare);
                cc.GetLonDegrees();
                cc.AddLonDegreesRemainder();
                cc.GetLonMinutes();
                string expectedResult = "17.5";
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
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string inputGridsquare = "CN87ut";
                string[] expectedResults = new string[2] { "1", "40" };
                string[] actualResults = new string[2];
                bool resultsMatch = false;
                cc.SetGridsquare(inputGridsquare);
                cc.GetLatDegrees();
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
        public void Test_GetLatDegrees_SecondPortion()
        {
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string inputGridsquare = "CN87ut";
                string[] expectedResults = new string[2] { "1", "47" };
                string[] actualResults = new string[2];
                bool resultsMatch = false;
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
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string inputGridsquare = "CN87ut";
                string expectedResult = "48.75";
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
            CoordinateConverter cc = new CoordinateConverter();
            if (CoordinateConverter.GenerateTableLookups())
            {
                string expectedResult = lcm.ArrlGridsquare();
                string inputDDM = lcm.AttainableDDM();
                DDMCoordinateHelper lynnwood = new DDMCoordinateHelper(inputDDM);
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
