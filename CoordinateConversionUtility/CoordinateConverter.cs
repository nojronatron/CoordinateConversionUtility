﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

namespace CoordinateConversionUtility
{
    public class CoordinateConverter
    {
        private ResourceManager rm = new ResourceManager("ErrorMessages", typeof(CoordinateConverter).Assembly);
        private CultureInfo currentCulture = CultureInfo.CurrentCulture;
        private static List<string> alphabet = new List<string>(24)
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
            "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
        };
        private static int capacity9 = 9;
        private static int capacity10 = 10;
        private static int capacity18 = 18;
        private static int capacity24 = 24;
        // Lookup Tables for Grid->Coordinate calculations
        private static Dictionary<string, int> Table1G2CLookup;
        private static Dictionary<string, double> Table3G2CLookup;
        private static Dictionary<string, int> Table4G2CLookup;
        private static Dictionary<string, double> Table6G2CLookup;
        // Lookup Tables for Coordinate->Grid calculations
        private static Dictionary<int, string> Table1C2GLookupPositive;
        private static Dictionary<int, string> Table1C2GLookupNegative;
        private static Dictionary<int, int> Table2C2GLookupPositive;
        private static Dictionary<int, int> Table2C2GLookupNegative;
        private static Dictionary<int, string> Table3C2GLookup;
        private static Dictionary<int, string> Table4C2GLookupPositive;
        private static Dictionary<int, string> Table4C2GLookupNegative;
        private static Dictionary<double, string> Table6C2GLookup;
        private double LonMinsRound { get; } = 2.5;         // used in gridsquare rounding calculations to find CENTER of 5th gridsquare character
        private double LatMinsRound { get; } = 1.25;        // used in gridsquare rounding calculations to find CENTER of 6th gridsquare character
        private double DD_Lattitude { get; set; }           // decimal degrees Lattitude e.g.: 47.8125 in 47.8125*N
        private double DD_Longitude { get; set; }           // decimal degrees Longitude e.g.: -122.2917 in 122.2917*W
        public int LatDirection { get; set; }               // 1 or -1: represents N or S, respectively
        public int LonDirection { get; set; }               // 1 or -1: represents E or W, respectively
        public int DDMlatDegrees { get; private set; }     // degree portion of DDM e.g.: 47 in 47*48.75"N
        public double DDMlatMinutes { get; private set; }  // decimal minutes portion of DDM e.g.: 48.75 in 47*48.75"N
        public int DDMlonDegrees { get; private set; }     // degree portion of DDM e.g.: -122 in 122*17.5"W
        public double DDMlonMinutes { get; private set; }  // decimal minutes portion of DDM e.g.: 17.5 in 122*17.5"W
        public int RemainderLat { get; private set; }      // stores carry-over remainder value for Lattitude calculations
        public int RemainderLon { get; private set; }      // stores carry-over remainder value for Longitude calculations
        public string Gridsquare { get; private set; }      // six-character representation of a validated Gridsquare coordinate
        public void SetLatRemainder(int latRemainder)  // should only be used for testing! Can cause unexpected results!
        {
            RemainderLat = latRemainder;
        }
        public void SetLonRemainder(int lonRemainder)  // should only be used for testing! Can cause unexpected results!
        {
            RemainderLon = lonRemainder;
        }
        public void SetGridsquare(string gridsquare)
        {   // use for testing. Externally setting this value can produce unexpected results
            if (gridsquare is null)
            {
                throw new ArgumentNullException(gridsquare, message: rm.GetString("gridsquareArgumentNull", System.Globalization.CultureInfo.CurrentCulture));
            }
            if (0 < gridsquare.Length && gridsquare.Length < 7)
            {
                if (ValidateGridsquareInput(gridsquare))
                {
                    Gridsquare = gridsquare;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(gridsquare, message: rm.GetString("gridsqareArgumentOutOfRange", System.Globalization.CultureInfo.CurrentCulture));
            }
        }
        public string GetLonDDM()
        {   // returns a string representation of LonDDM
            if (Gridsquare.Length != 6)
            {
                return null;
            }
            if (DDMlonDegrees == 0 || DDMlonMinutes == 0)
            {
                GetLonDegrees();
                AddLonDegreesRemainder();
                GetLonMinutes();
            }
            string lonDirLetter = "E"; // default positive Longitude indicator
            if (LonDirection < 0)
            {
                lonDirLetter = "W";
            }
            return $"{Math.Abs(DDMlonDegrees)}*{Math.Abs(DDMlonMinutes)}\"{lonDirLetter}";
        }
        public string GetLatDDM()
        {   // returns a string representation of LatDDM
            if (Gridsquare.Length != 6)
            {
                return null;
            }
            if (DDMlatDegrees == 0 || DDMlatMinutes == 0)
            {
                GetLatDegrees();
                AddLatDegreesRemainder();
                GetLatMinutes();
            }
            string latDirLetter = "N";  // default positive Lattitude indicator
            if (LatDirection < 0)
            {
                latDirLetter = "S";
            }
            return $"{Math.Abs(DDMlatDegrees)}*{Math.Abs(DDMlatMinutes)}\"{latDirLetter}";
        }
        public bool ValidateDDMinput(string ddmCoordinates)
        {   // accept string input, test it for DDM format, if good set DDM_LatDegrees, DDM_LatMinutes, LatDirection,
            //   ...DDM_LonDegrees, DDM_LonMinutes, LonDirection, and return True else ONLY return false
            if (ddmCoordinates is null)
            {
                throw new ArgumentNullException(ddmCoordinates, message: rm.GetString("ddmCoordinatesArgumentNull", System.Globalization.CultureInfo.CurrentCulture));
            }
            bool valid_Lats = false;
            bool valid_Lons = false;
            int new_DDM_LatDegrees = 0;             // use these to catch variables so props are set ONLY if return value will be true
            double new_DDM_LatMinutes = 0.0;
            int new_DDM_LonDegrees = 0;
            double new_DDM_LonMinutes = 0.0;
            int new_DDM_LatDirection = 0;           // set to zero so unset can be detected
            int new_DDM_LonDirection = 0;           // set to zero so unset can be detected
            ddmCoordinates = ddmCoordinates.Trim(); // basic input cleanser of white space characters
            try
            {
                int commaSymbolIndex = ddmCoordinates.IndexOf(",", System.StringComparison.CurrentCulture);
                string lat_half = ddmCoordinates.Substring(0, commaSymbolIndex);
                string lat_half_direction = "N";
                if (lat_half.IndexOf("N", System.StringComparison.CurrentCulture) > -1)
                {
                    lat_half = lat_half.Replace("N", "");
                }
                else if (lat_half.IndexOf("S", System.StringComparison.CurrentCulture) > -1)
                {
                    lat_half = lat_half.Replace("S", "");
                    lat_half_direction = "S";
                }
                if (lat_half.IndexOf("-", System.StringComparison.CurrentCulture) == 0)
                {   // if negative sign preceeds latitude coordinate it is in the southern hemisphere so append "S" later
                    lat_half = lat_half.Replace("-", "");
                    lat_half_direction = "S";
                }
                string second_half = ddmCoordinates.Substring(commaSymbolIndex + 1, ddmCoordinates.Length - commaSymbolIndex - 1);
                if (lat_half.Length > 5 && lat_half.Length < 12)
                {   // test length for somewhere like or between 1*2.0" and 99*88.77" (directionals already stripped)
                    int degreeSymbolIndex = lat_half.IndexOf("*", System.StringComparison.CurrentCulture);
                    if (degreeSymbolIndex > 0 && degreeSymbolIndex < 3)
                    {   // test the degree symbol (*) is in an appropriate place
                        int minutesSymbolIndex = lat_half.IndexOf("'", System.StringComparison.CurrentCulture);
                        if (minutesSymbolIndex == lat_half.Length - 1)
                        {   // test the minutes symbol (") is in an appropriate place
                            if (lat_half_direction == "S")
                            {   // set LatDirection now since everything validates
                                new_DDM_LatDirection = -1;
                            }
                            else
                            {
                                new_DDM_LatDirection = 1;
                            }
                            // int decimalSymbolIndex = lat_half.IndexOf("*");
                            new_DDM_LatDegrees = int.Parse(lat_half.Substring(0, degreeSymbolIndex));   // 47
                            new_DDM_LatMinutes = double.Parse(lat_half.Substring(degreeSymbolIndex + 1, minutesSymbolIndex - degreeSymbolIndex - 1));   // 48.75
                            valid_Lats = true;
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                Console.WriteLine($"An Exception occurred while validating Lattitude: {aoore.Message}.");
                valid_Lats = false;
            }
            try
            {
                int commaSymbolIndex = ddmCoordinates.IndexOf(",", StringComparison.CurrentCulture);
                string lon_half = ddmCoordinates.Substring(commaSymbolIndex + 1, ddmCoordinates.Length - commaSymbolIndex - 1);
                string lon_half_direction = "E";
                if (lon_half.IndexOf("E", System.StringComparison.CurrentCulture) > -1)
                {
                    lon_half = lon_half.Replace("E", "");
                }
                else if (lon_half.IndexOf("W", System.StringComparison.CurrentCulture) > -1)
                {
                    lon_half = lon_half.Replace("W", "");
                    //lon_half = $"-{lon_half}";
                    lon_half_direction = "W";
                }
                if (lon_half.IndexOf("-", System.StringComparison.CurrentCulture) == 0)
                {   // if negative sign preceeds latitude coordinate it is in the western hemisphere so append "S" later
                    lon_half = lon_half.Replace("-", "");
                    lon_half_direction = "W";
                }
                string second_half = ddmCoordinates.Substring(commaSymbolIndex + 1, ddmCoordinates.Length - commaSymbolIndex - 2);
                if (lon_half.Length > 5 && lon_half.Length < 12)
                {   // test length for somewhere like or between 1*2.0' and 199*88.77' (directionals already stripped)
                    int degreeSymbolIndex = lon_half.IndexOf("*", System.StringComparison.CurrentCulture);
                    if (degreeSymbolIndex > 0 && degreeSymbolIndex < 4)
                    {   // test the degree symbol (*) is in an appropriate place
                        int minutesSymbolIndex = lon_half.IndexOf("'", System.StringComparison.CurrentCulture);
                        if (minutesSymbolIndex == lon_half.Length - 1)
                        {   // test the minutes symbol (") is in an appropriate place
                            if (lon_half_direction == "W")
                            {   // set LatDirection now since everything validates
                                new_DDM_LonDirection = -1;
                            }
                            else
                            {
                                new_DDM_LonDirection = 1;
                            }
                            int decimalSymbolIndex = lon_half.IndexOf(".", System.StringComparison.CurrentCulture);
                            new_DDM_LonDegrees = int.Parse(lon_half.Substring(0, degreeSymbolIndex));   // 122
                            new_DDM_LonMinutes = double.Parse(lon_half.Substring(degreeSymbolIndex + 1, minutesSymbolIndex - degreeSymbolIndex - 1));   // 17.5
                            valid_Lons = true;
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                Console.WriteLine($"An Exception occurred while validating Longitude: {aoore.Message}");
                valid_Lons = false;
            }
            if (valid_Lats && valid_Lons)
            {
                DDMlatDegrees = new_DDM_LatDegrees * new_DDM_LatDirection;
                DDMlatMinutes = Math.Abs(new_DDM_LatMinutes);
                LatDirection = new_DDM_LatDirection;
                DDMlonDegrees = new_DDM_LonDegrees * new_DDM_LonDirection;
                DDMlonMinutes = Math.Abs(new_DDM_LonMinutes);
                LonDirection = new_DDM_LonDirection;
                return true;
            }
            return false;
        }
        public bool ValidateGridsquareInput(string gridsquare)
        {
            // validates gridsquare input, sets property Gridsquare with validated portion, return True if valid, False otherwise
            if (gridsquare is null)
            {
                throw new ArgumentNullException(gridsquare, rm.GetString("gridsquareArgumentNull", currentCulture));
            }
            string tempGridsquare = gridsquare.ToUpper(currentCulture);  // MSFT recommends using ToUpper() esp for string comparisons
            Regex rx = new Regex(@"[A-Z]{2}[0-9]{2}[A-Z]{2}");
            MatchCollection matches = rx.Matches(tempGridsquare);
            if (rx.IsMatch(tempGridsquare))
            {
                // Found a gridsquare pattern in {gridsquare}
                Gridsquare = matches[0].Value.ToString();
                return true;
            }
            // Gridsquare {gridsquare} was not matched to a valid pattern
            return false;
        }
        public void GetLatDegrees()
        {   // Table4 Lookup primary Degrees Lattitude
            if (Table4G2CLookup.TryGetValue(Gridsquare[1].ToString(currentCulture).ToUpper(culture: currentCulture), out int latDegrees))
            {
                DDMlatDegrees = latDegrees;
            }
            else
            {
                throw new KeyNotFoundException($"{Gridsquare[1].ToString(currentCulture)} of {Gridsquare} not found in Table4");
            }
            if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "I" || DDMlatDegrees < 0)
            {
                LatDirection = -1;
            }
            else if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "J" || DDMlatDegrees > 0)
            {
                LatDirection = 1;
            }
            else
            {
                throw new InvalidOperationException($"Somehow, Gridsquare member {Gridsquare[0]} (index zero of {Gridsquare}) or LonDegrees {DDMlatDegrees} did not match the program logic.");
            }
        }
        public void AddLatDegreesRemainder()
        {   // Table5 is calculated and will add to Degrees Lattitude, MUST BE ZERO_BIASED
            //LatDirection = -1;
            int lat_MinsAdjustment = 0;// -9;
            if (LatDirection == 1)
            {
                lat_MinsAdjustment = 0;
            }
            else if (LatDirection == -1)
            {
                lat_MinsAdjustment = -9;
            }
            DDMlatDegrees += ((int.Parse(Gridsquare[3].ToString()) + lat_MinsAdjustment) * LatDirection);
        }
        public void GetLatMinutes()
        {   // Table6 Lookup Lattitude Minutes including Round and increment/decrement Lat Degrees with carry-over
            // NOTE Dependency on LatDirection
            if (Table6G2CLookup.TryGetValue(Gridsquare[5].ToString(currentCulture).ToUpper(currentCulture), out double latMinsLookupResult))
            {
                if (LatDirection > 0)
                {   // the positive side (0 thru 60) of the Table but ZERO_BIASED so add 2.5 less than 60
                    latMinsLookupResult += 57.5 + LatMinsRound;
                }
                else
                {   // the negative side (-60 thru 0) of the Table
                    latMinsLookupResult += LatMinsRound;
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
            if (Table1G2CLookup.TryGetValue(Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture), out int lonDegrees))
            {
                DDMlonDegrees = lonDegrees;
            }
            else
            {
                throw new KeyNotFoundException($"{Gridsquare[0]} of {Gridsquare} not found in Table1!");
            }
            if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "I" || DDMlonDegrees < 0)
            {   // if Gridsquare is I result should be between -20 and 0
                LonDirection = -1;
            }
            else if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "J" || DDMlonDegrees > 0)
            {   // if Gridsquare is J result should be between 0 and 20
                LonDirection = 1;
            }
            else
            {
                throw new InvalidOperationException($"Somehow, Gridsquare member {Gridsquare[0]} (index zero of {Gridsquare}) or LonDegrees {DDMlonDegrees} did not match the program logic.");
            }
        }
        public void AddLonDegreesRemainder()
        {   // Table2 lookup is calculated, then added to Degrees Longitude, MUST BE ZERO_BIASED
            int lon_MinsAdjustment = 0;// -18;
            if (LonDirection == 1)
            {
                lon_MinsAdjustment = 0;
            }
            else if (LonDirection == -1)
            {
                lon_MinsAdjustment = -18;
            }
            // MUST convert char to string then to int otherwise the char value of the string is returned e.x.: "8" = char(56)
            DDMlonDegrees += (lon_MinsAdjustment + (int.Parse(Gridsquare[2].ToString(currentCulture)) * 2));
        }
        public void GetLonMinutes()
        {   // Table3 is Minutes Longitude Lookup table will also Round-up/down and in/decrement Lon Degrees with carry-over
            if (Table3G2CLookup.TryGetValue(Gridsquare[4].ToString(currentCulture).ToUpper(currentCulture), out double lonMinsLookupResult))
            {
                if (LonDirection > 0)
                {
                    lonMinsLookupResult += 115 + LonMinsRound;
                }
                else
                {
                    lonMinsLookupResult += (LonDirection * LonMinsRound);
                }
                // lonMinsLookupResult += (LonMinsRound * LonDirection);
                while (Math.Abs(lonMinsLookupResult) >= 120)
                {
                    DDMlonDegrees += (1 * LonDirection);
                    lonMinsLookupResult -= 120;
                }
            }
            else
            {
                throw new KeyNotFoundException($"{Gridsquare[4].ToString()} of {Gridsquare} not found in Table3!");
            }
            DDMlonMinutes = Math.Abs(lonMinsLookupResult);
        }
        public string ConvertGridsquareToDDM(string gridsquare)
        {   // input: 6-character string NN##nn
            // sets: DDM_Lat, DDM_Lon
            // output: DDM e.g.: 12*34.56"N,123*45.67W
            //DDM_Lat = 
            if (ValidateGridsquareInput(gridsquare))
            {
                GetLatDegrees();
                AddLatDegreesRemainder();
                GetLatMinutes();
                GetLonDegrees();
                AddLonDegreesRemainder();
                GetLonMinutes();
            }
            else
            {
                throw new ArgumentOutOfRangeException(rm.GetString("gridsquareArgumentOutOfRange", currentCulture));
            }
            return $"{GetLatDDM()},{GetLonDDM()}";
        }
        public string ConvertDDMtoGridsquare(string ddmCoordinates)
        {
            // input: ddm_coordinates as a single string
            // sets: DDM Lat Degrees, DDM Lat Minutes, DDM Lon Degrees, and DDM Lon Minutes
            // sets: Gridsquare
            // output: string representation of Gridsquare
            ValidateDDMinput(ddmCoordinates);
            GetFirstGridsquareCharacter();
            GetSecondGridsquareCharacter();
            GetThirdGridsquareCharacter();
            GetFourthGridsquareCharacter();
            GetFifthGridsquareCharacter();
            GetSixthGridsquareCharacter();
            return $"{Gridsquare}";
        }

        private void GetSixthGridsquareCharacter()
        {   // Input: DDM_LatMinutes; Remainder_Lat
            // Sets: Gridsquare (sixth character by concatenation)
            string gridsquareSixthChar = "-";
            double latMinsLookupValue = DDMlatMinutes;
            // check remainder and zero it out if in 2-degree increments otherwise...
            //   ...remove all but the remaining single-degree increment
            if (Math.Abs(RemainderLat) > 0)
            {
                latMinsLookupValue += RemainderLat * 30;    // remainders won't be more than 1
            }
            if (latMinsLookupValue > 0)
            {
                latMinsLookupValue -= 60.0; // 57.5;                   // the key side of the table is in negative values only
            }
            if (latMinsLookupValue % 2.5 != 0)
            {
                latMinsLookupValue -= (latMinsLookupValue % 2.5);   // subtract rem/2.5 to get next-lowest index
            }
            if (Table6C2GLookup.TryGetValue(latMinsLookupValue, out string table6LookupResult))
            {
                gridsquareSixthChar = table6LookupResult.ToLower(currentCulture);
            }
            else
            {
                throw new IndexOutOfRangeException($"Lookup value {latMinsLookupValue} not found in Table6C2GLookup.");
            }
            Gridsquare += gridsquareSixthChar;
        }

        private void GetFifthGridsquareCharacter()
        {   // Inputs: Remainder_Lon; 
            // Sets: DDM_LonMinutes; Gridsquare (fifth character by concatenation)
            string gridsquareFifthChar = "-";
            double lonMinutesLookupValue = DDMlonMinutes;
            // use remainder from Table1Lookup and Table2Lookup, express it as Minutes and Add to actual Minutes
            // ... then lookup nearest (round UP) 5-minute increment
            if (Math.Abs(RemainderLon) > 0)
            {   // if remainder degrees exist convert to Minutes Longitude and add to existing minutesLon
                lonMinutesLookupValue += (RemainderLon * 60); // TODO: pretty sure LonMinutes is synonymous with DDM_LonMinutes
            }
            if (LonDirection > 0)
            {
                if (RemainderLon % 5 != 0)
                {
                    lonMinutesLookupValue = RemainderLon - (RemainderLon % 5);   // subtract r/5 remainder to get next-lowest index
                }
                else
                {
                    lonMinutesLookupValue = RemainderLon;
                }
                if (Table3C2GLookup.TryGetValue((int)lonMinutesLookupValue - 120, out string table3LookupResult))
                {
                    gridsquareFifthChar = table3LookupResult;
                }
                else
                {
                    throw new IndexOutOfRangeException($"Lookup value {lonMinutesLookupValue} not found in Table3C2GLookup.");
                }
            }
            if (LonDirection < 0)
            {
                //if (DDM_LonMinutes % 5 != 0)
                if (lonMinutesLookupValue % 5 != 0)
                {
                    lonMinutesLookupValue = (int)(lonMinutesLookupValue - (lonMinutesLookupValue % 5));   // subtract r/5 remainder to get next-lowest index
                }
                else
                {
                    lonMinutesLookupValue = (int)lonMinutesLookupValue;
                }
                lonMinutesLookupValue *= -1;  // lookup keys on the negative side of the Table
                if (Table3C2GLookup.TryGetValue((int)lonMinutesLookupValue, out string table3LookupResult))
                {
                    gridsquareFifthChar = table3LookupResult;
                }
                else
                {
                    throw new IndexOutOfRangeException($"Lookup value {lonMinutesLookupValue} not found in Table3C2GLookup. " +
                                                        "It was transformed to an int so index could have been {(int)lonMinutesLookupValue}.");
                }
            }
            Gridsquare += gridsquareFifthChar.ToLower(currentCulture);
        }

        private void GetFourthGridsquareCharacter()
        {   // inputs: Remainder_Lat; LatDirection
            // Sets: Remainder_Lat; Gridsquare (fourth character by concatenation)
            int latLookupValue = RemainderLat;
            if (LatDirection < 0 && latLookupValue > -9 && latLookupValue <= 0)
            {
                latLookupValue = RemainderLat - 9;
            }
            else if (LatDirection > 0 && latLookupValue > 0 && latLookupValue < 10)
            {
                latLookupValue = RemainderLat;
            }
            else if (LatDirection < 0 && latLookupValue == -10)
            {
                latLookupValue = 0;
            }
            else if (LatDirection > 0 && latLookupValue == 10)
            {
                throw new IndexOutOfRangeException($"The lookup value {RemainderLat} is not within the range of -10 to 9.");
            }
            else
            {
                throw new IndexOutOfRangeException($"The lookup value {RemainderLat} is not within the range of -10 to 9.");
            }
            RemainderLat = 0;
            Gridsquare += latLookupValue.ToString(currentCulture);
        }

        private void GetThirdGridsquareCharacter()
        {   // Inputs: Remainder_Lon;
            // Sets: Remainder_Lon; Gridsquare (third character by concatenation)
            string gridsquareThirdChar = "-";
            int lonDegreesCalculatedResult = 0;
            if (LonDirection < 0)
            {
                lonDegreesCalculatedResult = Math.Abs(RemainderLon) / 2 - 9;
            }
            else
            {
                lonDegreesCalculatedResult = Math.Abs(RemainderLon) / 2 - 1;
            }
            if (lonDegreesCalculatedResult > 9)
            {
                lonDegreesCalculatedResult = 9;
            }
            gridsquareThirdChar = Math.Abs(lonDegreesCalculatedResult).ToString();
            if (RemainderLon % 2 != 0)
            {
                RemainderLon = 1;
            }
            else
            {
                RemainderLon = 0;
            }
            Gridsquare += gridsquareThirdChar;
        }

        private void GetSecondGridsquareCharacter()
        {   // input: DDM_LatDegrees
            // Sets: LatDirection; Remainder_Lat; Gridsquare (2nd character by concatenation)
            string gridsquareSecondChar = "-";
            int latDegreesLookupValue = DDMlatDegrees;
            if (LonDirection == 0)
            {
                if (DDMlatDegrees < 0)
                {
                    LatDirection = -1;
                }
                if (DDMlatDegrees >= 0)
                {
                    LatDirection = 1;
                }
            }

            RemainderLat = latDegreesLookupValue % 10;
            if (RemainderLat != 0)
            {
                latDegreesLookupValue = latDegreesLookupValue - RemainderLat;
            }
            if (LatDirection > 0)
            {
                if (Table4C2GLookupPositive.TryGetValue(latDegreesLookupValue, out string table4LookupResult))
                {
                    gridsquareSecondChar = table4LookupResult;
                }
            }
            if (LatDirection < 0)
            {
                if (Table4C2GLookupNegative.TryGetValue(Math.Abs(latDegreesLookupValue) * LatDirection, out string table4LookupResult))
                {
                    gridsquareSecondChar = table4LookupResult;
                }
            }
            Gridsquare += gridsquareSecondChar;
        }

        public void GetFirstGridsquareCharacter()
        {
            // inputs: int DDM_LonDegrees
            // sets: LonDirection; Gridsquare (first character by concatination)
            int lonDegreesLookupValue = DDMlonDegrees;
            string gridsquareFirstChar = "-";
            if (LonDirection == 0)
            {
                if (DDMlonDegrees < 0)
                {   // use this to check pos/neg degrees and adjust sign accordingly as a multiplier
                    LonDirection = -1;
                }
                else if (DDMlonDegrees >= 0)
                {
                    LonDirection = 1;
                }
            }
            RemainderLon = DDMlonDegrees % 20;
            if (RemainderLon != 0)
            {
                lonDegreesLookupValue = DDMlonDegrees - RemainderLon;
            }
            if (LonDirection < 0)
            {
                if (Table1C2GLookupNegative.TryGetValue(Math.Abs(lonDegreesLookupValue) * LonDirection, out string table1LookupResult))
                {
                    gridsquareFirstChar = table1LookupResult;
                }
            }
            if (LonDirection > 0)
            {
                if (Table1C2GLookupPositive.TryGetValue(lonDegreesLookupValue, out string table1LookupResult))
                {
                    gridsquareFirstChar = table1LookupResult;
                }
            }
            Gridsquare += gridsquareFirstChar;
        }
        public string ConvertDDtoDDM(string ddCoordinates)
        {
            // input: dd_coordinates as a string like 47.2917*N,122.8125*W
            // process: validate dd_coordinates
            // set: DD_Lattitude, DD_Longitude, DDM_LatDegrees, DDM_LatMinutes, DDM_LonDegrees, DDM_LonMinutes
            // output: DD_Lattitude and DD_Longitude as a contactenated string
            if (ddCoordinates is null)
            {
                throw new ArgumentNullException(ddCoordinates, rm.GetString("ddCoordinatesArgumentNull", currentCulture));
            }
            string returnString = "";
            ddCoordinates = ddCoordinates.Trim();
            if (ddCoordinates.Length < 7 || 21 < ddCoordinates.Length) // 1*N,2*W || 12.4567*N,123.5678*W
            {
                returnString = "Unable to process DD Coordinates that are less than 7 characters or more than 21 characters long.";
                returnString += "Use format DD.dddd*[NS],DD.dddd*[EW]";
            }
            int temp_LatDirection = 1;
            int temp_LonDirection = 1;
            StringBuilder sbLatCoords = new StringBuilder();
            StringBuilder sbLonCoords = new StringBuilder();
            char period = char.Parse(".");
            char comma = char.Parse(",");
            char negSign = char.Parse("-");
            char south = char.Parse("S");
            char west = char.Parse("W");
            int commaSeen = 0;
            foreach (char item in ddCoordinates)
            {
                if (commaSeen == 1)
                {   // Longitude coordinate region
                    if (char.IsDigit(item) || item.Equals(period))
                    {
                        sbLonCoords.Append(item.ToString());
                    }
                    else if (item.Equals(west) || item.Equals(negSign))
                    {
                        sbLonCoords.Insert(0, negSign);
                        temp_LonDirection = -1;
                    }
                }
                else if (char.IsDigit(item) || item.Equals(period) || item.Equals(negSign) || item.Equals(south))
                {   // Lattitude coordinate region
                    if (item.Equals(south) || item.Equals(negSign))
                    {
                        sbLatCoords.Insert(0, negSign);
                        temp_LatDirection = -1;
                    }
                    else
                    {
                        sbLatCoords.Append(item.ToString());
                    }
                }
                else
                {
                    if (item.Equals(comma))
                    {
                        commaSeen += 1;
                    }
                }
                if (commaSeen > 1)
                {   //TODO: "or DDD*MM.mm'[EW],DD*MM.mm'[NS]"
                    throw new ArgumentOutOfRangeException($"Incorrect input. Use the following format: [-]DD*MM.mm'[NS],[-]DDD*MM.mm'[EW]");
                }
            }
            //  DD is valid so set DD_Lattitude and DD_Longitude
            DD_Lattitude = double.Parse(sbLatCoords.ToString());
            DD_Longitude = double.Parse(sbLonCoords.ToString());

            // select appropriate portions of the SB to define and set Lat Degrees, Lat Minutes, Lon Degrees, and Lon Minutes props
            int lat_decimal_index = sbLatCoords.ToString().IndexOf(".", System.StringComparison.CurrentCulture);
            DDMlatDegrees = Math.Abs(int.Parse(sbLatCoords.ToString().Substring(0, lat_decimal_index)));
            double temp_latMinutes = double.Parse(sbLatCoords.ToString().Substring(lat_decimal_index + 1, (sbLatCoords.Length - (lat_decimal_index + 1))));
            int lon_decimal_index = sbLonCoords.ToString().IndexOf(".", System.StringComparison.CurrentCulture);
            DDMlonDegrees = Math.Abs(int.Parse(sbLonCoords.ToString().Substring(0, lon_decimal_index)));
            double temp_lonMinutes = double.Parse(sbLonCoords.ToString().Substring(lon_decimal_index + 1, (sbLonCoords.Length - (lon_decimal_index + 1))));
            DDMlatMinutes = Math.Round((temp_latMinutes / 10_000) * 60, 2);
            DDMlonMinutes = Math.Round((temp_lonMinutes / 10_000) * 60, 2);
            LatDirection = temp_LatDirection;
            LonDirection = temp_LonDirection;

            returnString = $"{DDMlatDegrees}*{DDMlatMinutes}'";
            if (LatDirection < 0)
            {
                returnString += "S,";
            }
            else
            {
                returnString += "N,";
            }
            returnString += $"{DDMlonDegrees}*{DDMlonMinutes}'";
            if (LonDirection < 0)
            {
                returnString += "W";
            }
            else
            {
                returnString += "E";
            }
            return returnString;
        }
        public string ConvertDMStoDDM(string dmsCoordinates)
        {
            // input: dms_coordinates as a string like 47*48'45"N,122*17'30"W
            // process: validate dms_coordinates
            // set: DD_Lattitude, DD_Longitude, DMS_LatDegrees
            // output: DD_Lattitude and DD_Longitude converted to DMS as a contactenated string
            if (dmsCoordinates is null)
            {
                throw new ArgumentNullException(rm.GetString("dmsCoordinatesArgumentNull", currentCulture));
            }
            string returnString = "";
            dmsCoordinates = dmsCoordinates.Trim();
            if (dmsCoordinates.Length < 15 || 22 < dmsCoordinates.Length) // 1*2'3"N,2*3'4"W || 12*34'56"N,123*45'07"W
            {
                returnString = "Unable to process DD Coordinates that are less than 15 characters or more than 22 characters long.\n";
                returnString += "Use format DD*MM'SS\"[NS],DDD*MM'SS\"[EW]";
            }
            int temp_LatDirection = 0;
            int temp_LonDirection = 0;
            char starDegrees = char.Parse("*");
            char minutes = char.Parse("'");
            char seconds = char.Parse("\"");
            char south = char.Parse("S");
            char west = char.Parse("W");
            char comma = char.Parse(",");
            char negSign = char.Parse("-");
            int commaIndex = dmsCoordinates.IndexOf(comma);
            string LatRegion = dmsCoordinates.Substring(0, commaIndex - 1);   // isolate Lat Degrees e.x.:[+-]47
            string LonRegion = dmsCoordinates.Substring(commaIndex + 1, dmsCoordinates.Length - commaIndex - 1); // isolate Lon Degrees e.x.: [+-]122
            int sThing = LatRegion.IndexOf(south);
            if (0 <= LatRegion.IndexOf(negSign) && LatRegion.IndexOf(negSign) < LatRegion.Length)
            {   // LatRegion holds a negative number or an S so set temp_LatDirection to -1 else set to 1
                temp_LatDirection = -1;
            }
            else if (LatRegion.IndexOf(south) >= 0 && temp_LatDirection == 0)
            {   // LatRegion didn't have a negative sign but does have an S
                temp_LonDirection = -1;
            }
            else
            {
                temp_LatDirection = 1;
            }
            int wThing = LonRegion.IndexOf(west);
            if (0 <= LonRegion.IndexOf(negSign) && LonRegion.IndexOf(negSign) < LonRegion.Length)
            {   // LonRegion holds a negative number or a W so set temp_LonDirection to a -1 else set to 1
                temp_LonDirection = -1;
            }
            else if (LonRegion.IndexOf(west) >= 0 && temp_LonDirection == 0)
            {   // LonRegion didn't have a negative sign but does have an W
                temp_LonDirection = -1;
            }
            else
            {
                temp_LonDirection = 1;
            }
            string tempLatDegrees = LatRegion.Substring(0, LatRegion.IndexOf(starDegrees));
            string tempLatMinutes = LatRegion.Substring(LatRegion.IndexOf(starDegrees) + 1, 2);
            string tempLatSeconds = LatRegion.Substring(LatRegion.IndexOf(minutes) + 1, 2);
            string tempLonDegrees = LonRegion.Substring(0, LonRegion.IndexOf(starDegrees));
            string tempLonMinutes = LonRegion.Substring(LonRegion.IndexOf(starDegrees) + 1, 2);
            string tempLonSeconds = LonRegion.Substring(LonRegion.IndexOf(minutes) + 1, 2);

            // set Properties for: Lat Degrees, Lat Minutes, Lat Seconds, Lon Degrees, Lon Minutes and Lon Seconds
            DDMlatDegrees = int.Parse(tempLatDegrees);
            double tempLatMinutes2 = double.Parse(tempLatMinutes);
            tempLatMinutes2 += double.Parse(tempLatSeconds) / 60;
            DDMlatMinutes = tempLatMinutes2;
            DDMlonDegrees = int.Parse(tempLonDegrees);
            double tempLonMinutes2 = double.Parse(tempLonMinutes);
            tempLonMinutes2 += double.Parse(tempLonSeconds) / 60;
            DDMlonMinutes = tempLonMinutes2;
            LatDirection = temp_LatDirection;
            LonDirection = temp_LonDirection;
            returnString = $"{DDMlatDegrees}*{DDMlatMinutes:f2}'";

            if (LatDirection < 0)
            {
                returnString += "S,";
            }
            else
            {
                returnString += "N,";
            }
            returnString += $"{DDMlonDegrees}*{DDMlonMinutes:f2}'";
            if (LonDirection < 0)
            {
                returnString += "W";
            }
            else
            {
                returnString += "E";
            }
            return returnString;
        }
        public string ConvertDDtoGridsquare(string ddLatAndLonCoordinates)
        {
            string DDM_Lat_Lon_String = ConvertDDtoDDM(ddLatAndLonCoordinates);
            GetFirstGridsquareCharacter();
            GetSecondGridsquareCharacter();
            GetThirdGridsquareCharacter();
            GetFourthGridsquareCharacter();
            GetFifthGridsquareCharacter();
            GetSixthGridsquareCharacter();
            return Gridsquare;
        }
        public string ConvertDMStoGridsquare(string dmsLatAndLongCoordinates)
        {
            string DMS_Lat_Lon_String = ConvertDMStoDDM(dmsLatAndLongCoordinates);
            GetFirstGridsquareCharacter();
            GetSecondGridsquareCharacter();
            GetThirdGridsquareCharacter();
            GetFourthGridsquareCharacter();
            GetFifthGridsquareCharacter();
            GetSixthGridsquareCharacter();
            return Gridsquare;
        }

        public static bool GenerateTableLookups()
        {
            try
            {
                Table1G2CLookup = new Dictionary<string, int>(capacity18);
                Table3G2CLookup = new Dictionary<string, double>(capacity24);
                Table4G2CLookup = new Dictionary<string, int>(capacity18);
                Table6G2CLookup = new Dictionary<string, double>(capacity24);
                Table1C2GLookupPositive = new Dictionary<int, string>(capacity10);
                Table1C2GLookupNegative = new Dictionary<int, string>(capacity10);
                Table2C2GLookupPositive = new Dictionary<int, int>(capacity9);
                Table2C2GLookupNegative = new Dictionary<int, int>(capacity9);
                Table3C2GLookup = new Dictionary<int, string>(capacity24);
                Table4C2GLookupPositive = new Dictionary<int, string>(capacity9);
                Table4C2GLookupNegative = new Dictionary<int, string>(capacity9);
                Table6C2GLookup = new Dictionary<double, string>(capacity24);

                int tracker = 0;
                int minsLongitude = -115;
                double minsLattitude = -57.5;
                while (tracker < capacity24)
                {
                    string letter = alphabet[tracker];
                    Table3G2CLookup.Add(letter, minsLongitude);
                    Table3C2GLookup.Add(minsLongitude, letter);
                    minsLongitude += 5;
                    Table6G2CLookup.Add(letter, minsLattitude);
                    Table6C2GLookup.Add(minsLattitude, letter);
                    minsLattitude += 2.5;
                    tracker++;
                }
                tracker = 0;
                int degreesLongitude = -160;
                int degreesLattitude = -80;
                while (tracker < capacity18)
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
                while (tracker < capacity10) // DONE: this code works
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
                throw new Exception("Table Generation Failed", ex.InnerException);  //  Console.WriteLine(ex.Message); // return false;
            }
            return true;
        }
    }
}