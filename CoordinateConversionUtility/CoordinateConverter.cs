using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics.Contracts;

namespace CoordinateConversionUtility
{
    public class CoordinateConverter //: IFormatProvider, ICustomFormatter
    {
        private readonly ResourceManager rm = new ResourceManager("ErrorMessages", typeof(CoordinateConverter).Assembly);
        private readonly CultureInfo currentCulture = CultureInfo.CurrentCulture;
        private static readonly List<string> alphabet = new List<string>(24)
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
            "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
        };
        private const int capacity9 = 9;
        private const int capacity10 = 10;
        private const int capacity18 = 18;
        private const int capacity24 = 24;
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
        public int LatDirection { get; private set; }               // 1 or -1: represents N or S, respectively
        public int LonDirection { get; private set; }               // 1 or -1: represents E or W, respectively
        public int DDMlatDegrees { get; private set; }     // degree portion of DDM e.g.: 47 in 47*48.75"N
        public double DDMlatMinutes { get; private set; }  // decimal minutes portion of DDM e.g.: 48.75 in 47*48.75"N
        public int DDMlonDegrees { get; private set; }     // degree portion of DDM e.g.: -122 in 122*17.5"W
        public double DDMlonMinutes { get; private set; }  // decimal minutes portion of DDM e.g.: 17.5 in 122*17.5"W
        public int RemainderLat { get; private set; }      // stores carry-over remainder value for Lattitude calculations
        public int RemainderLon { get; private set; }      // stores carry-over remainder value for Longitude calculations
        public string LatCompass { get; private set; }      // Lattitude compass heading N or S
        public string LonCompass { get; private set; }      // Longitude compass heading E or W
        public string Gridsquare { get; private set; }      // six-character representation of a validated Gridsquare coordinate
        public string GetDDcoordinatesSigned()
        {
            return $"{Math.Abs(DD_Lattitude) * LatDirection}*,{Math.Abs(DD_Longitude) * LonDirection}*";
        }
        public string GetDDcoordinatesCompass()
        {
            if (LonDirection < 0)
            {
                LonCompass = "W";
            }
            else
            {
                LonCompass = "E";
            }
            if (LatDirection < 0)
            {
                LatCompass = "S";
            }
            else
            {
                LatCompass = "N";
            }
            return $"{DD_Lattitude}*{LatCompass},{DD_Longitude}*{LonCompass}";
        }
        public void SetGridsquare(string gridsquare)
        {   // use for testing. Externally setting this value can produce unexpected results
            Contract.Requires(gridsquare != null);
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
            return $"{Math.Abs(DDMlonDegrees)}*{Math.Abs(DDMlonMinutes):f2}'{lonDirLetter}";
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
            return $"{Math.Abs(DDMlatDegrees)}*{Math.Abs(DDMlatMinutes):f2}'{latDirLetter}";
        }
        public bool ValidateDDMinput(string ddmCoordinates)
        {   // accept string input, test it for DDM format, if good set DDM_LatDegrees, DDM_LatMinutes, LatDirection,
            //   ...DDM_LonDegrees, DDM_LonMinutes, LonDirection, and return True else ONLY return false
            bool valid_Lats = false;
            bool valid_Lons = false;
            if (string.IsNullOrEmpty(ddmCoordinates))
            {   // see https://softwareengineering.stackexchange.com/questions/336179/is-there-an-easier-way-to-test-argument-validation-and-field-initialization-in-a
                return false;
            }
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
                if (lat_half.Length > 5 && lat_half.Length < 12)
                {   // test length for somewhere like or between 1*2.0" and 99*88.77" (directionals already stripped)
                    int degreeSymbolIndex = lat_half.IndexOf("*", System.StringComparison.CurrentCulture);
                    if (degreeSymbolIndex > 0 && degreeSymbolIndex <= 2)
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
                            new_DDM_LatDegrees = int.Parse(lat_half.Substring(0, degreeSymbolIndex), currentCulture);   // 47
                            new_DDM_LatMinutes = double.Parse(lat_half.Substring(degreeSymbolIndex + 1, minutesSymbolIndex - degreeSymbolIndex - 1), currentCulture);   // 48.75
                            valid_Lats = true;
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
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
                    if (degreeSymbolIndex > 0 && degreeSymbolIndex <= 3)
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
                            new_DDM_LonDegrees = int.Parse(lon_half.Substring(0, degreeSymbolIndex), currentCulture);   // 122
                            new_DDM_LonMinutes = double.Parse(lon_half.Substring(degreeSymbolIndex + 1, minutesSymbolIndex - degreeSymbolIndex - 1), currentCulture);   // 17.5
                            valid_Lons = true;
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                valid_Lons = false;
            }
            if (valid_Lats && valid_Lons)
            {
                DDMlatDegrees = new_DDM_LatDegrees;// * new_DDM_LatDirection;
                DDMlatMinutes = Math.Abs(new_DDM_LatMinutes);
                LatDirection = new_DDM_LatDirection;    //  DDMlatDegrees is set to LatDirection MUST also be set
                DDMlonDegrees = new_DDM_LonDegrees;// * new_DDM_LonDirection;
                DDMlonMinutes = Math.Abs(new_DDM_LonMinutes);
                LonDirection = new_DDM_LonDirection;    //  DDMlonDegrees is set so LonDirection MUST also be set
                return true;
            }
            return false;
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
                Gridsquare = matches[0].Value.ToString(currentCulture);
                return true;
            }
            return false;
        }
        public void GetLatDegrees()
        {   // Table4 Lookup primary Degrees Lattitude
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
                throw new InvalidOperationException($"Somehow, Gridsquare member {Gridsquare[0]} (index zero of {Gridsquare}) or LonDegrees {DDMlatDegrees} did not match the program logic.");
            }
            DDMlatDegrees = Math.Abs(latDegreesLookupResult);
        }
        public void AddLatDegreesRemainder()
        {   // Table5 is calculated and will add to Degrees Lattitude, MUST BE ZERO_BIASED
            // LatDirection = -1;
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
            if (Table6G2CLookup.TryGetValue(Gridsquare[5].ToString(currentCulture).ToUpper(currentCulture), out double latMinsLookupResult))
            {
                if (LatDirection > 0)
                {   // the positive side (0 thru 60) of the Table but ZERO_BIASED so add 2.5 less than 60
                    latMinsLookupResult += 57.5 + LatMinsRound;
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
                else if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "J" || lonDegreesLookupResult > 0)
                {   // if Gridsquare is J result should be between 0 and 20
                    LonDirection = 1;
                }
                else
                {
                    throw new InvalidOperationException($"Somehow, Gridsquare member {Gridsquare[0]} (index zero of {Gridsquare}) or LonDegrees {lonDegreesLookupResult} did not match the program logic.");
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
            if (Table3G2CLookup.TryGetValue(Gridsquare[4].ToString(currentCulture).ToUpper(currentCulture), out double lonMinsLookupResult))
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
            // sets: DDM_Lat, DDM_Lon
            // output: DDM e.g.: 12*34.56"N,123*45.67"W
            Contract.Requires(gridsquare != null);
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
            Contract.Requires(ddmCoordinates != null);
            if (ValidateDDMinput(ddmCoordinates))
            {
                GetFirstGridsquareCharacter();
                GetSecondGridsquareCharacter();
                GetThirdGridsquareCharacter();
                GetFourthGridsquareCharacter();
                GetFifthGridsquareCharacter();
                GetSixthGridsquareCharacter();
                return $"{Gridsquare}";
            }
            else
            {
                throw new ArgumentOutOfRangeException(rm.GetString("ddmCoordinatesInputError", currentCulture));
            }
        }
        private void GetSixthGridsquareCharacter()
        {   // Input: DDM_LatMinutes; Remainder_Lat
            // Sets: Gridsquare (sixth character by concatenation)
            double latMinsLookupValue;
            // check remainder and zero it out if in 2-degree increments otherwise...
            //   ...remove all but the remaining single-degree increment
            if (LatDirection > 0)
            {
                latMinsLookupValue =- 60 + DDMlatMinutes;
                if (latMinsLookupValue % 2.5 != 0)    //  test for divisible by 5 if not reduce to next LOWEST (by the table) multiple of 5
                {
                    latMinsLookupValue -= (latMinsLookupValue % 2.5);
                }
                if (Table6C2GLookup.TryGetValue(latMinsLookupValue, out string table6LookupResult))
                {
                    Gridsquare += table6LookupResult.ToLower(currentCulture);
                }
            }
            else if (LatDirection < 0)
            {
                latMinsLookupValue = LatDirection * DDMlatMinutes;
                if (latMinsLookupValue % 2.5 != 0)
                {
                    latMinsLookupValue -= (latMinsLookupValue % 2.5);
                }
                if (Table6C2GLookup.TryGetValue(latMinsLookupValue, out string table6LookupResult))
                {
                    Gridsquare += table6LookupResult.ToLower(currentCulture);
                }
            }
            else
            {
                Gridsquare += "?";
                throw new IndexOutOfRangeException(rm.GetString("gridsquareIndexOutOfRange", currentCulture));
            }
        }
        private void GetFifthGridsquareCharacter()
        {   // Inputs: Remainder_Lon; 
            // Sets: DDM_LonMinutes; Gridsquare (fifth character by concatenation)
            double lonMinutesLookupValue;   // = 0;    // = DDMlonMinutes;
            double remainderCorrectionValue = 0;
            if (RemainderLon > 1)
            {
                throw new IndexOutOfRangeException(rm.GetString("gridsquareIndexOutOfRange", currentCulture));
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
                if (lonMinutesLookupValue % 5 != 0) //  test for divisible by 5 if not reduce to next LOWEST (per the table) multiple of 5
                {
                    lonMinutesLookupValue -= (lonMinutesLookupValue % 5);   //  += (lonMinutesLookupValue % 5);   //    
                }
                if (Table3C2GLookup.TryGetValue((int)lonMinutesLookupValue, out string table3LookupResult))
                {
                    Gridsquare += table3LookupResult.ToLower(currentCulture);
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
                    Gridsquare += table3LookupResult.ToLower(currentCulture);
                }
            }
            else
            {
                Gridsquare += "?";
                throw new IndexOutOfRangeException(rm.GetString("gridsquareIndexOutOfRange", currentCulture));
            }
        }
        private void GetFourthGridsquareCharacter()
        {   // inputs: Remainder_Lat; LatDirection
            // Sets: Remainder_Lat; Gridsquare (fourth character by concatenation)
            //  NOTE: Fourth Gridsquare character is arranged in single-digit increments therefore no remainder
            int latLookupValue; // = RemainderLat;
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
            Gridsquare += latLookupValue.ToString(currentCulture);
        }
        private void GetThirdGridsquareCharacter()
        {   // Inputs: Remainder_Lon;
            // Sets: Remainder_Lon; Gridsquare (third character by concatenation)
            int calculationNumber;  // = 0;
            if (LonDirection < 0)
            {
                if (RemainderLon % 2 != 0)  //  TRY THIS to solve odd-numbered negative-degrees Longitude e.g.: -5 needs to append "7"
                {
                    //calculationNumber = (RemainderLon + 21) / 2 - 1;      // *might* be incorrect calculation after all
                    calculationNumber = ((RemainderLon * LonDirection) + 21) / 2 - 1;
                    RemainderLon = 1;   // used up max even portion of RemainderLon so odd single is left and must be accounted for in last grid character
                }
                else
                {
                    //calculationNumber = (RemainderLon + 20) / 2 - 1;      // returns '10' when RemainderLon = 2 and LonDirection = -1
                    calculationNumber = ((RemainderLon * LonDirection) + 20) / 2 - 1;
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
            int lonDegreesCalculatedResult = calculationNumber;
            Gridsquare += Math.Abs(lonDegreesCalculatedResult).ToString(currentCulture);
        }
        private void GetSecondGridsquareCharacter()
        {   // input: DDM_LatDegrees
            // Sets: LatDirection; Remainder_Lat; Gridsquare (2nd character by concatenation)
            int latDegreesLookupValue = DDMlatDegrees;
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
                throw new IndexOutOfRangeException($"LatDirection ({LatDirection}) could not be set for latDegreesLookupValue {latDegreesLookupValue}.");
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
                    Gridsquare += table4LookupResult;
                }
            }
            else if (LatDirection < 0)
            {
                if (Table4C2GLookupNegative.TryGetValue(Math.Abs(latDegreesLookupValue) * LatDirection, out string table4LookupResult))
                {
                    Gridsquare += table4LookupResult;
                }
                else
                {
                    Gridsquare += "?";
                }
            }
            else
            {
                Gridsquare += "?";
            }
        }
        private void GetFirstGridsquareCharacter()
        {
            // inputs: int DDM_LonDegrees
            // sets: Gridsquare (first character by concatination)
            // LonDirection MUST be set prior to entering this method
            int lonDegreesLookupValue;
            if (LonDirection != -1 && LonDirection != 1)
            {
                throw new IndexOutOfRangeException($"LonDirection ({LonDirection}) could not be set for lonDegreesLookupValue.");
            }
            this.Gridsquare = "";
            //if (DDMlonDegrees < 0)
            //{   // use this to check pos/neg degrees and adjust sign accordingly as a multiplier
            //    LonDirection = -1;
            //}
            //else if (DDMlonDegrees > 0)
            //{
            //    LonDirection = 1;
            //}
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
                    Gridsquare += table1LookupResult;
                }
            }
            else if (LonDirection > 0)
            {
                if (Table1C2GLookupPositive.TryGetValue(lonDegreesLookupValue, out string table1LookupResult))
                {
                    Gridsquare += table1LookupResult;
                }
                else
                {
                    Gridsquare += "?";
                }
            }
            else
            {
                Gridsquare += "?";
            }
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
            ddCoordinates = ddCoordinates.Trim();
            if (ddCoordinates.Length < 7 || 21 < ddCoordinates.Length) // 1*N,2*W || 12.4567*N,123.5678*W
            {
                return "Unable to process DD Coordinates that are less than 7 characters or more than 21 characters long. " +
                    "Use format DD.dddd*[NS],DD.dddd*[EW]";
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
                        sbLonCoords.Append(item.ToString(currentCulture));
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
                        sbLatCoords.Append(item.ToString(currentCulture));
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
            DD_Lattitude = double.Parse(sbLatCoords.ToString(), currentCulture);
            DD_Longitude = double.Parse(sbLonCoords.ToString(), currentCulture);

            // select appropriate portions of the SB to define and set Lat Degrees, Lat Minutes, Lon Degrees, and Lon Minutes props
            int lat_decimal_index = sbLatCoords.ToString().IndexOf(".", System.StringComparison.CurrentCulture);
            DDMlatDegrees = Math.Abs(int.Parse(sbLatCoords.ToString().Substring(0, lat_decimal_index), currentCulture));
            double temp_latMinutes = double.Parse(sbLatCoords.ToString().Substring(lat_decimal_index + 1, (sbLatCoords.Length - (lat_decimal_index + 1))), currentCulture);
            int lon_decimal_index = sbLonCoords.ToString().IndexOf(".", System.StringComparison.CurrentCulture);
            DDMlonDegrees = Math.Abs(int.Parse(sbLonCoords.ToString().Substring(0, lon_decimal_index), currentCulture));
            double temp_lonMinutes = double.Parse(sbLonCoords.ToString().Substring(lon_decimal_index + 1, (sbLonCoords.Length - (lon_decimal_index + 1))), currentCulture);
            DDMlatMinutes = Math.Round((temp_latMinutes / 10_000) * 60, 2);
            DDMlonMinutes = Math.Round((temp_lonMinutes / 10_000) * 60, 2);
            LatDirection = temp_LatDirection;   //  LatDirection MUST be set when DDMlatDegrees has been set
            LonDirection = temp_LonDirection;   //  LonDirection MUST be set when DDMlonDegrees has been set

            string returnString = $"{DDMlatDegrees}*{DDMlatMinutes:f2}'";
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
            dmsCoordinates = dmsCoordinates.Trim();
            if (22 < dmsCoordinates.Length || dmsCoordinates.Length < 15 ) // 1*2'3"N,2*3'4"W || 12*34'56"N,123*45'07"W
            {
                return "Unable to process DD Coordinates that are less than 15 characters or more than 22 characters long.\n" +
                    "Use format DD*MM'SS\"[NS],DDD*MM'SS\"[EW]";
            }
            else
            {
                int temp_LatDirection = 0;
                int temp_LonDirection = 0;
                char starDegrees = char.Parse("*");
                char minutes = char.Parse("'");
                char south = char.Parse("S");
                char west = char.Parse("W");
                char comma = char.Parse(",");
                char negSign = char.Parse("-");
                int commaIndex = dmsCoordinates.IndexOf(comma);
                string LatRegion = dmsCoordinates.Substring(0, commaIndex - 1);   // isolate Lat Degrees e.x.:[+-]47
                string LonRegion = dmsCoordinates.Substring(commaIndex + 1, dmsCoordinates.Length - commaIndex - 1); // isolate Lon Degrees e.x.: [+-]122
                                                                                                                     //  int sThing = LatRegion.IndexOf(south);  //  never used
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
                DDMlatDegrees = int.Parse(tempLatDegrees, currentCulture);
                double tempLatMinutes2 = double.Parse(tempLatMinutes, currentCulture);
                tempLatMinutes2 += double.Parse(tempLatSeconds, currentCulture) / 60;
                DDMlatMinutes = tempLatMinutes2;
                DDMlonDegrees = int.Parse(tempLonDegrees, currentCulture);
                double tempLonMinutes2 = double.Parse(tempLonMinutes, currentCulture);
                tempLonMinutes2 += double.Parse(tempLonSeconds, currentCulture) / 60;
                DDMlonMinutes = tempLonMinutes2;
                LatDirection = temp_LatDirection;   //  LatDirection MUST be set when DDMlatDegrees is set
                LonDirection = temp_LonDirection;   //  LonDirection MUST be set when DDMlonDegrees is set
                string returnString = $"{DDMlatDegrees}*{DDMlatMinutes:f2}'";

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
        }
        public string ConvertDDtoGridsquare(string ddLatAndLonCoordinates)
        {
            Contract.Requires(ddLatAndLonCoordinates != null);
            ConvertDDtoDDM(ddLatAndLonCoordinates);
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
            Contract.Requires(dmsLatAndLongCoordinates != null);
            ConvertDMStoDDM(dmsLatAndLongCoordinates);
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
            ResourceManager rmGTL = new ResourceManager("ErrorMessages", typeof(CoordinateConverter).Assembly);
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
                throw new Exception(rmGTL.GetString("tableGenerationFailed", CultureInfo.CurrentCulture), ex.InnerException);
            }
            return true;
        }
    }
}
