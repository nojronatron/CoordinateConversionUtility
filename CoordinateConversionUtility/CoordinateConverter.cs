using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics.Contracts;
using CoordinateConversionUtility.Models;

namespace CoordinateConversionUtility
{
    public class CoordinateConverter
    {
        private readonly CultureInfo currentCulture = CultureInfo.CurrentCulture;
        private static readonly List<string> alphabet = new List<string>(24)
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
            "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
        };

        // Lookup Tables for Grid->Coordinate calculations
        private static Dictionary<string, int> Table1G2CLookup;
        private static Dictionary<string, decimal> Table3G2CLookup;
        private static Dictionary<string, int> Table4G2CLookup;
        private static Dictionary<string, decimal> Table6G2CLookup;
        // Lookup Tables for Coordinate->Grid calculations
        private static Dictionary<decimal, string> Table1C2GLookupPositive;
        private static Dictionary<decimal, string> Table1C2GLookupNegative;
        private static Dictionary<int, int> Table2C2GLookupPositive;
        private static Dictionary<int, int> Table2C2GLookupNegative;
        private static Dictionary<decimal, string> Table3C2GLookup;
        private static Dictionary<decimal, string> Table4C2GLookupPositive;
        private static Dictionary<decimal, string> Table4C2GLookupNegative;
        private static Dictionary<decimal, string> Table6C2GLookup;


        private decimal LonMinsRound { get; } = 2.5m;         // used in gridsquare rounding calculations to find CENTER of 5th gridsquare character
        private decimal LatMinsRound { get; } = 1.25m;        // used in gridsquare rounding calculations to find CENTER of 6th gridsquare character
        private DDCoordinate DecimalDegrees { get; set; } = (DDCoordinate)default;   //  decimal degrees Lat and Lon e.g. 47.8125,-122.2917
        private DDMCoordinate DecimalDegreesMinutes { get; set; } //  decimal degreeMinutes Lat and Lon e.g. 47*48.75'N,122*17.50'W
        private DMSCoordinate DegreesMinutesSeconds { get; set; } //  degrees minutes seconds Lat and Lon e.g. N 47*48'45",W 122*17'30"
        //private static char DegreesSymbol { get { return (char)176; } }     //  degree symbol
        //private static char MinutesSymbol { get { return (char)39; } }      //  single quote
        //private static char SecondsSymbol { get { return (char)34; } }      //  double quote
        public decimal LatDirection { get; private set; }   // 1 or -1: represents N or S, respectively
        public decimal LonDirection { get; private set; }   // 1 or -1: represents E or W, respectively
        public decimal DDMlatDegrees { get; private set; }  //  ever negative?  // degree portion of DDM e.g.: 47 in 47*48.75"N
        public decimal DDMlatMinutes { get; private set; }  //  ever negative?  // decimal minutes portion of DDM e.g.: 48.75 in 47*48.75"N
        public decimal DDMlonDegrees { get; private set; }  //  evern egative?  // degree portion of DDM e.g.: -122 in 122*17.5"W
        public decimal DDMlonMinutes { get; private set; }  //  ever negative?  //  decimal minutes portion of DDM e.g.: 17.5 in 122*17.5"W
        public decimal RemainderLat { get; private set; }      // stores carry-over remainder value for Lattitude calculations
        public decimal RemainderLon { get; private set; }      // stores carry-over remainder value for Longitude calculations
        public StringBuilder Gridsquare { get; private set; }      // six-character representation of a validated Gridsquare coordinate
        public CoordinateConverter()                        //  custom CTOR to initialize StringBuilder Gridsquare
        {
            Gridsquare = new StringBuilder();
            DecimalDegrees = new DDCoordinate();
        }
        public bool AddDdCoordinates(DDCoordinate ddCoords)
        {
            if (ddCoords != null)
            {
                DecimalDegrees = new DDCoordinate(ddCoords.DegreesLattitude, ddCoords.DegreesLongitude);
                return true;
            }
            return false;
        }
        public bool AddDdmCoordinates(DDMCoordinate ddmCoords)
        {
            if (ddmCoords != null)
            {
                DecimalDegreesMinutes = new DDMCoordinate(ddmCoords.DegreesLattitude, ddmCoords.DegreesLongitude);
            }
            return false;
        }
        public bool AddDmsCoordinates(DMSCoordinate dmsCoords)
        {
            if (dmsCoords != null)
            {
                DegreesMinutesSeconds = new DMSCoordinate(dmsCoords.DegreesLattitude, dmsCoords.DegreesLongitude);
                return true;
            }
            return false;
        }
        public string GetDDcoordinatesSigned()
        {
            return DecimalDegrees.ToString();
        }
        public void SetGridsquare(string gridsquare)
        {   // use for testing. Externally setting this value can produce unexpected results
            Contract.Requires(gridsquare != null);
            if (0 < gridsquare.Length && gridsquare.Length < 7)
            {
                if (ValidateGridsquareInput(gridsquare))
                {
                    Gridsquare.Clear();
                    Gridsquare.Append(gridsquare);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Argument out of range in { System.Reflection.MethodInfo.GetCurrentMethod().Name }");
            }
        }

        public bool ValidateGridsquareInput(string gridsquare)
        {   // validates gridsquare input, sets property Gridsquare with validated portion, return True if valid, False otherwise
            if (string.IsNullOrEmpty(gridsquare))
            {
                return false;
            }
            string tempGridsquare = gridsquare.ToUpper(currentCulture);  // MSFT recommends using ToUpper() esp for string comparisons
            Regex rx = new Regex(@"[A-Z]{2}[0-9]{2}[A-Z]{2}");
            MatchCollection matches = rx.Matches(tempGridsquare);
            if (rx.IsMatch(tempGridsquare))
            {   // Found a gridsquare pattern in {gridsquare}
                Gridsquare.Append(matches[0].Value.ToString(currentCulture));
                return true;
            }
            return false;
        }
        public void GetLatDegrees()
        {   // Table4 Lookup primary Degrees Lattitude from a GridSquare character
            if (Table4G2CLookup.TryGetValue(Gridsquare[1].ToString(currentCulture).ToUpper(culture: currentCulture), out int latDegreesLookupResult))
            {
                if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "I" || latDegreesLookupResult < 0)
                {
                    LatDirection = -1;
                }
                else if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "J" || latDegreesLookupResult > 0)
                {
                    LatDirection = 1;
                }
                else
                {
                    throw new KeyNotFoundException($"{Gridsquare[1].ToString(currentCulture)} of {Gridsquare} not found in Table4");
                }
            }
            else
            {
                throw new InvalidOperationException($"Gridsquare member {Gridsquare[0]} (index zero of {Gridsquare}) or LonDegrees {DDMlatDegrees} did not match the program logic.");
            }
            DDMlatDegrees = Math.Abs(latDegreesLookupResult);
        }
        public void AddLatDegreesRemainder()
        {   // Table5 is calculated and will add to Degrees Lattitude, MUST BE ZERO_BIASED
            int lat_MinsAdjustment = 0; // -9;
            if (LatDirection == 1)
            {
                lat_MinsAdjustment = 0;
            }
            else if (LatDirection == -1)
            {
                lat_MinsAdjustment = -9;
            }
            DDMlatDegrees += ((int.Parse(Gridsquare[3].ToString(currentCulture), currentCulture) + lat_MinsAdjustment) * LatDirection);
        }
        public void GetLatMinutes()
        {   // Table6 Lookup Lattitude Minutes including Round and increment/decrement Lat Degrees with carry-over
            // NOTE Dependency on LatDirection
            if (Table6G2CLookup.TryGetValue(Gridsquare[5].ToString(currentCulture).ToUpper(currentCulture), out decimal latMinsLookupResult))
            {
                if (LatDirection > 0)
                {   // the positive side (0 thru 60) of the Table but ZERO_BIASED so add 2.5 less than 60
                    latMinsLookupResult += 57.5m + LatMinsRound;
                }
                else
                {   // the negative side (-60 thru 0) of the Table
                    latMinsLookupResult += (LatMinsRound * LatDirection);
                }
                while (Math.Abs(latMinsLookupResult) >= 60)
                {
                    DDMlatDegrees += (1 * LatDirection);
                    latMinsLookupResult -= 60;
                }
            }
            else
            {
                throw new KeyNotFoundException($"{Gridsquare[5]} of {Gridsquare} not found in Table6");
            }
            DDMlatMinutes = Math.Abs(latMinsLookupResult);
        }
        public void GetLonDegrees()
        {   // the 1st portion of Degrees Longitude IS the successfull lookup of first_lonChar in Table1
            if (Table1G2CLookup.TryGetValue(Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture), out int lonDegreesLookupResult))
            {
                //  DDMlonDegrees will be set to LonDirection MUST also be set
                if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "I" || lonDegreesLookupResult < 0)
                {   // if Gridsquare is I result should be between -20 and 0
                    LonDirection = -1;
                }
                else if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "J" || lonDegreesLookupResult >= 0)
                {   // if Gridsquare is J result should be between 0 and 20
                    LonDirection = 1;
                }
                else
                {
                    throw new InvalidOperationException($"Gridsquare member {Gridsquare[0]} (index zero of {Gridsquare}) or LonDegrees {lonDegreesLookupResult} did not match the program logic.");
                }
            }
            else
            {
                throw new KeyNotFoundException($"{Gridsquare[0]} of {Gridsquare} not found in Table1!");
            }
            DDMlonDegrees = Math.Abs(lonDegreesLookupResult);
        }
        public void AddLonDegreesRemainder()
        {   // Table2 lookup is calculated, then added to Degrees Longitude, MUST BE ZERO_BIASED
            //  LonDirection must be checked (should have been set PRIOR to reaching this method)
            if (LonDirection == 1)
            {   //  calc gridchar Number to Positive side of table is (num * 2)
                //lon_MinsAdjustment = 0;
                DDMlonDegrees += (int.Parse(Gridsquare[2].ToString(currentCulture), currentCulture) * 2);
            }
            else if (LonDirection == -1)
            {
                //  calc gridchar Number to Negative side of table: (num * 2) - 18 
                int lon_MinsAdjustment = -18;
                DDMlonDegrees += Math.Abs((lon_MinsAdjustment + (int.Parse(Gridsquare[2].ToString(currentCulture), currentCulture) * 2)));
            }
        }
        public void GetLonMinutes()
        {   // Table3 is Minutes Longitude Lookup table will also Round-up/down and in/decrement Lon Degrees with carry-over
            int lonMinsReducer;
            if (Table3G2CLookup.TryGetValue(Gridsquare[4].ToString(currentCulture).ToUpper(currentCulture), out decimal lonMinsLookupResult))
            {
                if (LonDirection > 0)
                {
                    lonMinsLookupResult += 115 + LonMinsRound;
                    lonMinsReducer = 60;
                }
                else
                {
                    lonMinsLookupResult += (LonDirection * LonMinsRound);
                    lonMinsReducer = -60;
                }
                // lonMinsLookupResult += (LonMinsRound * LonDirection);
                while (Math.Abs(lonMinsLookupResult) >= 60) //   120)
                {
                    DDMlonDegrees += 1;
                    lonMinsLookupResult -= lonMinsReducer;  //   120;
                }
            }
            else
            {
                throw new KeyNotFoundException($"{Gridsquare[4].ToString(currentCulture)} of {Gridsquare} not found in Table3!");
            }
            DDMlonMinutes = Math.Abs(lonMinsLookupResult);
        }
        public string ConvertGridsquareToDDM(string gridsquare)
        {   // input: 6-character string NN##nn
            // sets: DDMlatMinutes, DDMlonMinutes
            // output: DDM e.g.: 12*34.56"N,123*45.67"W
            //  TODO: Fix Lat result might be off by <1 degree
            //  TODO: Fix Lon result might be off by degree
            Contract.Requires(gridsquare != null);
            if (ValidateGridsquareInput(gridsquare))
            {
                GetLatDegrees();
                AddLatDegreesRemainder();
                GetLatMinutes();
                GetLonDegrees();
                AddLonDegreesRemainder();
                GetLonMinutes();
                DDMlatMinutes = GetNearestEvenMultiple(DDMlatMinutes, 1);
                DDMlonMinutes = GetNearestEvenMultiple(DDMlonMinutes, 2);
            }
            else
            {
                throw new ArgumentException($"Argument { gridsquare } did not pass validation.");
            }
            DecimalDegreesMinutes = new DDMCoordinate(DDMlatDegrees * LatDirection, DDMlatMinutes, DDMlonDegrees * LonDirection, DDMlonMinutes);
            //  TODO: Fix output zeroes placeholder ahead of minutes
            //  TODO: Fix output zeroes behind seconds
            return DecimalDegreesMinutes.ToString();
        }
        private static decimal GetNearestEvenMultiple(decimal minutesInput, int latOrLon)
        {   //  returns a decimal value that maps to the nearest even multiple of Lat (1) or Lon (2) coordinates for apropriate rounding
            decimal interval = 0.0m;
            if (latOrLon == 1)  //  1 = lattitude
            {   //  incremental steps mapping to gridsquare north and south edges (Lattitude)
                interval = 2.5m;
            }
            else
            {   //  incremental steps mapping to gridsquare east and west edges (Longitude)
                interval = 5.0m; 
            }
            if (minutesInput % interval == 0)
            {   //  interval is an even divisor of minutesInput so return it
                return minutesInput;
            }
            decimal LowEndMultiple = (Math.Truncate(minutesInput / interval)) * interval;
            decimal HighEndMultiple = LowEndMultiple + interval;
            decimal LowEndDifference = Math.Abs(minutesInput - LowEndMultiple);
            decimal HighEndDifference = Math.Abs(minutesInput - HighEndMultiple);
            if (LowEndDifference < HighEndDifference)
            {
                if (minutesInput < 0)
                {
                    return LowEndMultiple;
                }
                if (minutesInput > 0)
                {
                    return  HighEndMultiple;
                }
            }
            else if (LowEndDifference > HighEndDifference)
            {
                if (minutesInput > 0)
                {
                    return HighEndMultiple;
                }
                if (minutesInput < 0)
                {
                    return LowEndMultiple;
                }
            }
            else // LowEndDifference == HighEndDifference
            {
                if (minutesInput > 0)
                {
                    return HighEndMultiple;
                }
                if (minutesInput < 0)
                {
                    return LowEndMultiple;
                }
            }

            return 0.0m;    //  a solution was not found
        }
        private void GetSixthGridsquareCharacter()
        {   // Input: DDM_LatMinutes; Remainder_Lat
            // Sets: Gridsquare (sixth character by concatenation)
            decimal latMinsLookupValue;
            // check remainder and zero it out if in 2-degree increments otherwise...
            //   ...remove all but the remaining single-degree increment
            if (LatDirection > 0)
            {
                latMinsLookupValue = -60m + DDMlatMinutes;
                latMinsLookupValue = GetNearestEvenMultiple(latMinsLookupValue, 1);

                if (Table6C2GLookup.TryGetValue(latMinsLookupValue, out string table6LookupResult))
                {
                    Gridsquare.Append(table6LookupResult.ToLower(currentCulture));
                }
            }
            else if (LatDirection < 0)
            {
                latMinsLookupValue = LatDirection * DDMlatMinutes;

                if (latMinsLookupValue % 2.5m != 0)
                {
                    latMinsLookupValue -= (latMinsLookupValue % 2.5m);
                }
                if (Table6C2GLookup.TryGetValue(latMinsLookupValue, out string table6LookupResult))
                {
                    Gridsquare.Append(table6LookupResult.ToLower(currentCulture));
                }
            }
            else
            {
                Gridsquare.Append("?");
                throw new ArgumentOutOfRangeException($"Unable to determine character in method { System.Reflection.MethodBase.GetCurrentMethod().Name }");
            }
        }
        private void GetFifthGridsquareCharacter()
        {   // Inputs: Remainder_Lon; 
            // Sets: Gridsquare (fifth character by concatenation)
            decimal lonMinutesLookupValue;   // = 0;    // = DDMlonMinutes;
            decimal remainderCorrectionValue = 0;
            if (RemainderLon > 1)
            {
                throw new ArgumentOutOfRangeException($"RemainderLon was > 1 in { System.Reflection.MethodBase.GetCurrentMethod().Name }");
            }
            else if (RemainderLon == 1)
            {
                remainderCorrectionValue = 60;    //  convert the remainder degrees to minutes
                RemainderLon = 0;               //  zero-out the remainder now it has been used
            }
            // use remainder from Table1Lookup and Table2Lookup, express it as Minutes and Add to actual Minutes
            if (LonDirection > 0)
            {
                lonMinutesLookupValue = 0 - 120 + DDMlonMinutes + remainderCorrectionValue;
                lonMinutesLookupValue = GetNearestEvenMultiple(lonMinutesLookupValue, 2);

                if (Table3C2GLookup.TryGetValue(lonMinutesLookupValue, out string table3LookupResult))
                {
                    Gridsquare.Append(table3LookupResult.ToLower(currentCulture));
                }
            }
            else if (LonDirection < 0)
            {
                lonMinutesLookupValue = 0 - DDMlonMinutes - remainderCorrectionValue;
                if (lonMinutesLookupValue % 5 != 0)
                {
                    lonMinutesLookupValue -= (lonMinutesLookupValue % 5);   //  move the pointer down the table to the next multiple of 5
                }
                if (Table3C2GLookup.TryGetValue((int)lonMinutesLookupValue, out string table3LookupResult))
                {
                    Gridsquare.Append(table3LookupResult.ToLower(currentCulture));
                }
            }
            else
            {
                Gridsquare.Append("?");
            }
        }
        private void GetFourthGridsquareCharacter()
        {   // inputs: Remainder_Lat; LatDirection
            // Sets: Remainder_Lat; Gridsquare (fourth character in StringBuilder)
            //  NOTE: Fourth Gridsquare character is arranged in single-digit increments therefore no remainder
            decimal latLookupValue; // = RemainderLat;
            if (LatDirection < 0)
            {
                latLookupValue = RemainderLat + 9;
            }
            else if (LatDirection > 0)
            {
                latLookupValue = RemainderLat;
            }
            else
            {
                throw new IndexOutOfRangeException($"The lookup value {RemainderLat} is not within the range of -10 to 9.");
            }
            RemainderLat = 0;
            Gridsquare.Append(latLookupValue.ToString(currentCulture));
        }
        private void GetThirdGridsquareCharacter()
        {   // Inputs: LonDirection; Remainder_Lon;
            // Sets: Remainder_Lon; Gridsquare (third character in StringBuilder)
            decimal calculationNumber;  // = 0;
            if (LonDirection < 0)
            {

                if (RemainderLon % 2 != 0)
                {
                    calculationNumber = ((RemainderLon) + 21) / 2 - 1;
                    RemainderLon = 1;   // used up max even portion of RemainderLon so odd single is left and must be accounted for in last grid character
                }
                else
                {
                    calculationNumber = (18 + RemainderLon) / 2;
                }
            }
            else
            {
                calculationNumber = Math.Abs(RemainderLon) / 2;
            }

            if (RemainderLon % 2 != 0)  //  third character lookup is in 2-degree increments so an odd-number will result in a remainder of 1
            {
                RemainderLon = 1;
            }
            else
            {
                RemainderLon = 0;   //  decrement remainder
            }

            decimal lonDegreesCalculatedResult = Math.Truncate(calculationNumber);
            Gridsquare.Append(Math.Abs(lonDegreesCalculatedResult).ToString(currentCulture));
        }
        private void GetSecondGridsquareCharacter()
        {   // input: DDM_LatDegrees
            // Sets: LatDirection; Remainder_Lat; Gridsquare (2nd character in StringBuilder)
            decimal latDegreesLookupValue = DDMlatDegrees;
            if (DDMlatDegrees < 0)
            {
                LatDirection = -1;
            }
            else if (DDMlatDegrees > 0)
            {
                LatDirection = 1;
            }
            else
            {
                throw new IndexOutOfRangeException($"LatDirection ({ LatDirection }) could not be set for latDegreesLookupValue { latDegreesLookupValue }.");
            }

            RemainderLat = latDegreesLookupValue % 10;
            
            if (RemainderLat != 0)
            {
                latDegreesLookupValue -= RemainderLat;
            }
            
            if (LatDirection > 0)
            {
            
                if (Table4C2GLookupPositive.TryGetValue(latDegreesLookupValue, out string table4LookupResult))
                {
                    Gridsquare.Append(table4LookupResult);
                }
            }
            else if (LatDirection < 0)
            {
                
                if (Table4C2GLookupNegative.TryGetValue(Math.Abs(latDegreesLookupValue) * LatDirection, out string table4LookupResult))
                {
                    Gridsquare.Append(table4LookupResult);
                }
                else
                {
                    Gridsquare.Append("?");
                }
            }
            else
            {
                Gridsquare.Append("?");
            }
        }
        private void GetFirstGridsquareCharacter()
        {
            // inputs: int DDM_LonDegrees
            // sets: Gridsquare (first character in StringBuilder)
            // LonDirection MUST be set prior to entering this method
            decimal lonDegreesLookupValue;
            if (LonDirection != -1 && LonDirection != 1)
            {
                throw new IndexOutOfRangeException($"LonDirection ({LonDirection}) could not be set for lonDegreesLookupValue.");
            }
            Gridsquare.Clear();
            RemainderLon = DDMlonDegrees % 20;
            if (RemainderLon != 0)
            {
                lonDegreesLookupValue = DDMlonDegrees - RemainderLon;
            }
            else
            {
                lonDegreesLookupValue = DDMlonDegrees;
            }
            if (LonDirection < 0)
            {
                if (Table1C2GLookupNegative.TryGetValue(Math.Abs(lonDegreesLookupValue) * LonDirection, out string table1LookupResult))
                {
                    Gridsquare.Append(table1LookupResult);
                }
            }
            else if (LonDirection > 0)
            {
                if (Table1C2GLookupPositive.TryGetValue(lonDegreesLookupValue, out string table1LookupResult))
                {
                    Gridsquare.Append(table1LookupResult);
                }
                else
                {
                    Gridsquare.Append("?");
                }
            }
            else
            {
                Gridsquare.Append("?");
            }
        }
        public string ConvertDDMtoGridsquare(DDMCoordinate ddmCoordinates)
        {
            // input: ddm_coordinates as a single string
            // output: string representation of the calculated Gridsquare
            //  TODO: fix reliance on INTs, perhaps by referencing the passed-in object rather than setting this.Properties
            Contract.Requires(ddmCoordinates != null);
            if (ddmCoordinates != null)
            {
                this.DDMlatDegrees = ddmCoordinates.GetLatDegrees();
                this.DDMlonDegrees = ddmCoordinates.GetLonDegrees();
                this.DDMlatMinutes = ddmCoordinates.MinutesLattitude;
                this.DDMlonMinutes = ddmCoordinates.MinutesLongitude;
                this.LatDirection = ConversionHelper.ExtractPolarityNS(ddmCoordinates.ToString());
                this.LonDirection = ConversionHelper.ExtractPolarityEW(ddmCoordinates.ToString());
                GetFirstGridsquareCharacter();
                GetSecondGridsquareCharacter();
                GetThirdGridsquareCharacter();
                GetFourthGridsquareCharacter();
                GetFifthGridsquareCharacter();
                GetSixthGridsquareCharacter();
                return $"{ Gridsquare.ToString() }";
            }
            else
            {
                return string.Empty;    //  if input is empty output is empty
            }
        }
        public string ConvertDMStoGridsquare(DMSCoordinate dmsLatAndLongCoordinates)
        {
            //  input: dms coordinates as an object
            //  output: string representation of the calculated Gridsquare
            Contract.Requires(dmsLatAndLongCoordinates != null);
            if (dmsLatAndLongCoordinates != null)
            {
                string ddm = ConversionHelper.ToDDM(dmsLatAndLongCoordinates);
                DecimalDegreesMinutes = new DDMCoordinate(ddm);
                LatDirection = ConversionHelper.ExtractPolarityNS(ddm);
                LonDirection = ConversionHelper.ExtractPolarityEW(ddm);
                ConvertDDMtoGridsquare(DecimalDegreesMinutes);
                return $"{ Gridsquare.ToString() }";
            }
            else
            {
                return string.Empty;    //  if input is empty output is empty
            }
        }
        public static bool GenerateTableLookups()
        {
            Table1G2CLookup = new Dictionary<string, int>(18);
            Table3G2CLookup = new Dictionary<string, decimal>(24);
            Table4G2CLookup = new Dictionary<string, int>(18);
            Table6G2CLookup = new Dictionary<string, decimal>(24);
            Table1C2GLookupPositive = new Dictionary<decimal, string>(10);
            Table1C2GLookupNegative = new Dictionary<decimal, string>(10);
            Table2C2GLookupPositive = new Dictionary<int, int>(9);
            Table2C2GLookupNegative = new Dictionary<int, int>(9);
            Table3C2GLookup = new Dictionary<decimal, string>(24);
            Table4C2GLookupPositive = new Dictionary<decimal, string>(9);
            Table4C2GLookupNegative = new Dictionary<decimal, string>(9);
            Table6C2GLookup = new Dictionary<decimal, string>(24);

            try
            {
                int tracker = 0;
                decimal minsLongitude = -115m;
                decimal minsLattitude = -57.5m;
                while (tracker < 24)
                {
                    string letter = alphabet[tracker];
                    Table3G2CLookup.Add(letter, minsLongitude);
                    Table3C2GLookup.Add(minsLongitude, letter);
                    minsLongitude += 5m;
                    Table6G2CLookup.Add(letter, minsLattitude);
                    Table6C2GLookup.Add(minsLattitude, letter);
                    minsLattitude += 2.5m;
                    tracker++;
                }
                tracker = 0;
                int degreesLongitude = -160;
                int degreesLattitude = -80;
                while (tracker < 18)
                {
                    string letter = alphabet[tracker];
                    if (letter == "J")
                    {   // zero-biased tables!
                        degreesLongitude -= 20;
                        degreesLattitude -= 10;
                        Table1C2GLookupPositive.Add(degreesLongitude, letter);
                        Table4C2GLookupPositive.Add(degreesLattitude, letter);
                    }
                    Table1G2CLookup.Add(letter, degreesLongitude);
                    if (letter == "I")
                    {
                        Table1C2GLookupNegative.Add(degreesLongitude, letter);
                        Table4C2GLookupNegative.Add(degreesLattitude, letter);
                    }
                    if (degreesLongitude < 0)
                    {
                        Table1C2GLookupNegative.Add(degreesLongitude, letter);
                        Table4C2GLookupNegative.Add(degreesLattitude, letter);
                    }
                    if (degreesLongitude > 0)
                    {
                        Table1C2GLookupPositive.Add(degreesLongitude, letter);
                        Table4C2GLookupPositive.Add(degreesLattitude, letter);
                    }
                    Table4G2CLookup.Add(letter, degreesLattitude);
                    degreesLongitude += 20;
                    degreesLattitude += 10;
                    tracker++;
                }
                tracker = 0;
                int degreesNegativeLongitude = -18; // index: -18 to 0; value: 0 to 9
                degreesLongitude = 2;               // Index: 2 to 20; value: 0 to 9
                int degreesNegativeLattitude = -9;  // index: -9 to 0; value: 0 to 9
                degreesLattitude = 1;               // index: 1 to 10; value: 0 to 9
                while (tracker < 10)
                {
                    Table2C2GLookupPositive.Add(degreesLongitude, tracker);
                    Table2C2GLookupNegative.Add(degreesNegativeLongitude, tracker);
                    degreesLongitude += 2;
                    degreesNegativeLongitude += 2;
                    degreesLattitude++;
                    degreesNegativeLattitude++;
                    tracker++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}
