using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoordinateConversionUtility
{
    public class CoordinateConverter
    {
        //  Goal: Accept DDM (47*48.25,-122*45.71) and return a six-character Gridsquare (JJ00XX)
        //  Goal: Accept a six-character Gridsquare and return a DDM (II99AA)
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
        public int DDM_LatDegrees { get; private set; }     // degree portion of DDM e.g.: 47 in 57*48.75"N
        public double DDM_LatMinutes { get; private set; }  // decimal minutes portion of DDM e.g.: 48.75 in 47*48.75"N
        public int DDM_LonDegrees { get; private set; }     // degree portion of DDM e.g.: -122 in 122*17.5"W
        public double DDM_LonMinutes { get; private set; }  // decimal minutes portion of DDM e.g.: 17.5 in 122*17.5"W
        public int Remainder_Lat { get; private set; }      // stores carry-over remainder value for Lattitude calculations
        public int Remainder_Lon { get; private set; }      // stores carry-over remainder value for Longitude calculations
        public string Gridsquare { get; private set; }      // six-character representation of a validated Gridsquare coordinate
        public void SetLatRemainder(int lat_remainder)
        {   // should only be used for testing! Can cause unexpected results!
            Remainder_Lat = lat_remainder;
        }
        public void SetLonRemainder(int lon_remainder)
        {   // should only be used for testing! Can cause unexpected results!
            Remainder_Lon = lon_remainder;
        }
        public void SetGridsquare(string gridsquare)
        {   // use for testing. Externally setting this value can produce unexpected results
            if (0 < gridsquare.Length && gridsquare.Length < 7)
            {
                if (ValidateGridsquareInput(gridsquare))
                {
                    Gridsquare = gridsquare;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Gridsquare argument {gridsquare} is not valid.");
            }
        }
        public string GetLonDDM()
        {   // returns a string representation of LonDDM
            if (Gridsquare.Length != 6)
            {
                return null;
            }
            if (DDM_LonDegrees == 0 || DDM_LonMinutes == 0)
            {
                GetLonDegrees_FirstPortion();
                AddLonDegrees_SecondPortion();
                GetLonMinutes();
            }
            string lonDirLetter = "E"; // default positive Longitude indicator
            if (LonDirection < 0)
            {
                lonDirLetter = "W";
            }
            return $"{Math.Abs(DDM_LonDegrees)}*{Math.Abs(DDM_LonMinutes)}\"{lonDirLetter}";
        }
        public string GetLatDDM()
        {   // returns a string representation of LatDDM
            if (Gridsquare.Length != 6)
            {
                return null;
            }
            if (DDM_LatDegrees == 0 || DDM_LatMinutes == 0)
            {
                GetLatDegrees_FirstPortion();
                AddLatDegrees_SecondPortion();
                GetLatMinutes();
            }
            string latDirLetter = "N";  // default positive Lattitude indicator
            if (LatDirection < 0)
            {
                latDirLetter = "S";
            }
            return $"{Math.Abs(DDM_LatDegrees)}*{Math.Abs(DDM_LatMinutes)}\"{latDirLetter}";
        }
        public bool ValidateDDMinput(string ddm_coordinates)
        {   // accept string input, test it for DDM format, if good set DDM_LatDegrees, DDM_LatMinutes, LatDirection,
            //   ...DDM_LonDegrees, DDM_LonMinutes, LonDirection, and return True else ONLY return false
            bool valid_Lats = false;
            bool valid_Lons = false;
            int new_DDM_LatDegrees = 0;         // use these to catch variables so props are set ONLY if return value will be true
            double new_DDM_LatMinutes = 0.0;
            int new_DDM_LonDegrees = 0;
            double new_DDM_LonMinutes = 0.0;
            int new_DDM_LatDirection = 0;       // set to zero so unset can be detected
            int new_DDM_LonDirection = 0;       // set to zero so unset can be detected
            ddm_coordinates.Trim(); // basic input cleanser of white space characters
            try
            {
                int commaSymbolIndex = ddm_coordinates.IndexOf(",");
                string lat_half = ddm_coordinates.Substring(0, commaSymbolIndex);
                string lat_half_direction = "N";
                if (lat_half.IndexOf("N") > -1)
                {
                    lat_half = lat_half.Replace("N", "");
                }
                else if ( lat_half.IndexOf("S") > -1)
                {
                    lat_half = lat_half.Replace("S", "");
                    //lat_half = $"-{lat_half}";
                    lat_half_direction = "S";
                }
                if (lat_half.IndexOf("-") == 0)
                {   // if negative sign preceeds latitude coordinate it is in the southern hemisphere so append "S" later
                    lat_half = lat_half.Replace("-", "");
                    lat_half_direction = "S";
                }
                string second_half = ddm_coordinates.Substring(commaSymbolIndex + 1, ddm_coordinates.Length - commaSymbolIndex - 1);
                if (lat_half.Length > 5 && lat_half.Length < 12)
                {   // test length for somewhere like or between 1*2.0" and 99*88.77" (directionals already stripped)
                    int degreeSymbolIndex = lat_half.IndexOf("*");
                    if (degreeSymbolIndex > 0 && degreeSymbolIndex < 3)
                    {   // test the degree symbol (*) is in an appropriate place
                        int minutesSymbolIndex = lat_half.IndexOf("'");
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
            catch (Exception ex)
            {
                Console.WriteLine($"An Exception occurred while validating Lattitude: {ex.Message}.");
                valid_Lats = false;
            }
            try
            {
                int commaSymbolIndex = ddm_coordinates.IndexOf(",");
                string lon_half = ddm_coordinates.Substring(commaSymbolIndex + 1, ddm_coordinates.Length - commaSymbolIndex - 1);
                string lon_half_direction = "E";
                if (lon_half.IndexOf("E") > -1)
                {
                    lon_half = lon_half.Replace("E", "");
                }
                else if (lon_half.IndexOf("W") > -1)
                {
                    lon_half = lon_half.Replace("W", "");
                    //lon_half = $"-{lon_half}";
                    lon_half_direction = "W";
                }
                if (lon_half.IndexOf("-") == 0)
                {   // if negative sign preceeds latitude coordinate it is in the western hemisphere so append "S" later
                    lon_half = lon_half.Replace("-", "");
                    lon_half_direction = "W";
                }
                string second_half = ddm_coordinates.Substring(commaSymbolIndex + 1, ddm_coordinates.Length - commaSymbolIndex - 2);
                if (lon_half.Length > 5 && lon_half.Length < 12)
                {   // test length for somewhere like or between 1*2.0' and 199*88.77' (directionals already stripped)
                    int degreeSymbolIndex = lon_half.IndexOf("*");
                    if (degreeSymbolIndex > 0 && degreeSymbolIndex < 4)
                    {   // test the degree symbol (*) is in an appropriate place
                        int minutesSymbolIndex = lon_half.IndexOf("'");
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
                            int decimalSymbolIndex = lon_half.IndexOf(".");
                            new_DDM_LonDegrees = int.Parse(lon_half.Substring(0, degreeSymbolIndex));   // 122
                            new_DDM_LonMinutes = double.Parse(lon_half.Substring(degreeSymbolIndex + 1, minutesSymbolIndex - degreeSymbolIndex - 1));   // 17.5
                            valid_Lons = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Exception occurred while validating Longitude: {ex.Message}");
                valid_Lons = false;
            }
            if (valid_Lats && valid_Lons)
            {
                DDM_LatDegrees = new_DDM_LatDegrees * new_DDM_LatDirection;
                DDM_LatMinutes = Math.Abs(new_DDM_LatMinutes);
                LatDirection = new_DDM_LatDirection;
                DDM_LonDegrees = new_DDM_LonDegrees * new_DDM_LonDirection;
                DDM_LonMinutes = Math.Abs(new_DDM_LonMinutes);
                LonDirection = new_DDM_LonDirection;
                return true;
            }
            return false;
        }
        public bool ValidateGridsquareInput(string gridsquare)
        {
            // validates gridsquare input, sets property Gridsquare with validated portion, return True if valid, False otherwise
            string tempGridsquare = gridsquare.ToUpper();  // MSFT recommends using ToUpper() esp for string comparisons
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
        public void GetLatDegrees_FirstPortion()
        {   // Table4 Lookup primary Degrees Lattitude
            if (Table4G2CLookup.TryGetValue(Gridsquare[1].ToString().ToUpper(), out int latDegrees))
            {
                DDM_LatDegrees = latDegrees;
            }
            else
            {
                throw new KeyNotFoundException($"{Gridsquare[1].ToString()} of {Gridsquare} not found in Table4");
            }
            if (Gridsquare[0].ToString().ToUpper() == "I" || DDM_LatDegrees < 0)
            {
                LatDirection = -1;
            }
            else if (Gridsquare[0].ToString().ToUpper() == "J" || DDM_LatDegrees > 0)
            {
                LatDirection = 1;
            }
            else
            {
                throw new InvalidOperationException($"Somehow, Gridsquare member {Gridsquare[0]} (index zero of {Gridsquare}) or LonDegrees {DDM_LatDegrees} did not match the program logic.");
            }
        }
        public void AddLatDegrees_SecondPortion()
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
            DDM_LatDegrees += ((int.Parse(Gridsquare[3].ToString()) + lat_MinsAdjustment) * LatDirection);
        }
        public void GetLatMinutes()
        {   // Table6 Lookup Lattitude Minutes including Round and increment/decrement Lat Degrees with carry-over
            // NOTE Dependency on LatDirection
            if (Table6G2CLookup.TryGetValue(Gridsquare[5].ToString().ToUpper(), out double latMinsLookupResult))
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
                    DDM_LatDegrees += (1 * LatDirection);
                    latMinsLookupResult -= 60;
                }
            }
            else
            {
                throw new KeyNotFoundException($"{Gridsquare[5]} of {Gridsquare} not found in Table6");
            }
            DDM_LatMinutes = Math.Abs(latMinsLookupResult);
        }
        public void GetLonDegrees_FirstPortion()
        {   // the 1st portion of Degrees Longitude IS the successfull lookup of first_lonChar in Table1
            if (Table1G2CLookup.TryGetValue(Gridsquare[0].ToString().ToUpper(), out int lonDegrees))
            {
                DDM_LonDegrees = lonDegrees;
            }
            else
            {
                throw new KeyNotFoundException($"{Gridsquare[0]} of {Gridsquare} not found in Table1!");
            }
            if (Gridsquare[0].ToString().ToUpper() == "I" || DDM_LonDegrees < 0)
            {   // if Gridsquare is I result should be between -20 and 0
                LonDirection = -1;
            }
            else if (Gridsquare[0].ToString().ToUpper() == "J" || DDM_LonDegrees > 0)
            {   // if Gridsquare is J result should be between 0 and 20
                LonDirection = 1;
            }
            else
            {
                throw new InvalidOperationException($"Somehow, Gridsquare member {Gridsquare[0]} (index zero of {Gridsquare}) or LonDegrees {DDM_LonDegrees} did not match the program logic.");
            }
        }
        public void AddLonDegrees_SecondPortion()
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
            DDM_LonDegrees += (lon_MinsAdjustment + (int.Parse(Gridsquare[2].ToString()) * 2));
        }
        public void GetLonMinutes()
        {   // Table3 is Minutes Longitude Lookup table will also Round-up/down and in/decrement Lon Degrees with carry-over
            if (Table3G2CLookup.TryGetValue(Gridsquare[4].ToString().ToUpper(), out double lonMinsLookupResult))
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
                    DDM_LonDegrees += (1 * LonDirection);
                    lonMinsLookupResult -= 120;
                }
            }
            else
            {
                throw new KeyNotFoundException($"{Gridsquare[4].ToString()} of {Gridsquare} not found in Table3!");
            }
            DDM_LonMinutes = Math.Abs(lonMinsLookupResult);
        }
        public string ConvertGridsquareToDDM(string gridsquare)
        {   // input: 6-character string NN##nn
            // sets: DDM_Lat, DDM_Lon
            // output: DDM e.g.: 12*34.56"N,123*45.67W
            //DDM_Lat = 
            if (ValidateGridsquareInput(gridsquare))
            {
                GetLatDegrees_FirstPortion();
                AddLatDegrees_SecondPortion();
                GetLatMinutes();
                GetLonDegrees_FirstPortion();
                AddLonDegrees_SecondPortion();
                GetLonMinutes();
            }
            else
            {
                throw new ArgumentException($"Gridsquare input did not meet format requirements AA00AA to XX99XX");
            }
            return $"{GetLatDDM()},{GetLonDDM()}";
        }
        public string ConvertDDMtoGridsquare(string ddm_coordinates)
        {
            // input: ddm_coordinates as a single string
            // sets: DDM Lat Degrees, DDM Lat Minutes, DDM Lon Degrees, and DDM Lon Minutes
            // sets: Gridsquare
            // output: string representation of Gridsquare
            ValidateDDMinput(ddm_coordinates);
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
            double latMinsLookupValue = DDM_LatMinutes;
            // check remainder and zero it out if in 2-degree increments otherwise...
            //   ...remove all but the remaining single-degree increment
            if (Math.Abs(Remainder_Lat) > 0)
            {
                latMinsLookupValue += Remainder_Lat * 30;    // remainders won't be more than 1
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
                gridsquareSixthChar = table6LookupResult.ToLower();
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
            int lonMinutesLookupValue = 0;
            // use remainder from Table1Lookup and Table2Lookup, express it as Minutes and Add to actual Minutes
            // ... then lookup nearest (round UP) 5-minute increment
            if (Math.Abs(Remainder_Lon) > 0)
            {   // if remainder degrees exist convert to Minutes Longitude and add to existing minutesLon
                DDM_LonMinutes += (Remainder_Lon * 60); // TODO: pretty sure LonMinutes is synonymous with DDM_LonMinutes
            }
            if (LonDirection > 0)
            {
                if (Remainder_Lon % 5 != 0)
                {
                    lonMinutesLookupValue = Remainder_Lon - (Remainder_Lon % 5);   // subtract r/5 remainder to get next-lowest index
                }
                else
                {
                    lonMinutesLookupValue = Remainder_Lon;
                }
                if (Table3C2GLookup.TryGetValue(lonMinutesLookupValue - 120, out string table3LookupResult))
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
                if (DDM_LonMinutes % 5 != 0)
                {
                    lonMinutesLookupValue = (int)(DDM_LonMinutes - (DDM_LonMinutes % 5));   // subtract r/5 remainder to get next-lowest index
                }
                else
                {
                    lonMinutesLookupValue = (int)DDM_LonMinutes;
                }
                lonMinutesLookupValue *= -1;  // lookup keys on the negative side of the Table
                if (Table3C2GLookup.TryGetValue(lonMinutesLookupValue, out string table3LookupResult))
                {
                    gridsquareFifthChar = table3LookupResult;
                }
                else
                {
                    throw new IndexOutOfRangeException($"Lookup value {lonMinutesLookupValue} not found in Table3C2GLookup.");
                }
            }
            Gridsquare += gridsquareFifthChar.ToLower();
        }

        private void GetFourthGridsquareCharacter()
        {   // inputs: Remainder_Lat; LatDirection
            // Sets: Remainder_Lat; Gridsquare (fourth character by concatenation)
            int latLookupValue = Remainder_Lat;
            if (LatDirection < 0 && latLookupValue > -9 && latLookupValue <= 0)
            {
                latLookupValue = Remainder_Lat - 9;
            }
            else if (LatDirection > 0 && latLookupValue > 0 && latLookupValue < 10)
            {
                latLookupValue = Remainder_Lat;
            }
            else if (LatDirection < 0 && latLookupValue == -10)
            {
                latLookupValue = 0;
            }
            else if (LatDirection > 0 && latLookupValue == 10)
            {
                throw new IndexOutOfRangeException($"The lookup value {Remainder_Lat} is not within the range of -10 to 9.");
            }
            else
            {
                throw new IndexOutOfRangeException($"The lookup value {Remainder_Lat} is not within the range of -10 to 9.");
            }
            Remainder_Lat = 0;
            Gridsquare += latLookupValue.ToString();
        }

        private void GetThirdGridsquareCharacter()
        {   // Inputs: Remainder_Lon;
            // Sets: Remainder_Lon; Gridsquare (third character by concatenation)
            string gridsquareThirdChar = "-";
            int lonDegreesCalculatedResult = 0;
            if (LonDirection < 0)
            {
                lonDegreesCalculatedResult = (int)Math.Abs(Remainder_Lon) / 2 - 9;
            }
            else
            {
                lonDegreesCalculatedResult = (int)Math.Abs(Remainder_Lon) / 2 - 1;
            }
            if (lonDegreesCalculatedResult > 9)
            {
                lonDegreesCalculatedResult = 9;
            }
            gridsquareThirdChar = Math.Abs(lonDegreesCalculatedResult).ToString();
            if (Remainder_Lon % 2 != 0)
            {
                Remainder_Lon = 1;
            }
            else
            {
                Remainder_Lon = 0;
            }
            Gridsquare += gridsquareThirdChar;
        }

        private void GetSecondGridsquareCharacter()
        {   // input: DDM_LatDegrees
            // Sets: LatDirection; Remainder_Lat; Gridsquare (2nd character by concatenation)
            string gridsquareSecondChar = "-";
            int latDegreesLookupValue = DDM_LatDegrees;
            if (DDM_LatDegrees < 0)
            {
                LatDirection = -1;
            }
            if (DDM_LatDegrees >= 0)
            {
                LatDirection = 1;
            }
            Remainder_Lat = latDegreesLookupValue % 10;
            if (Remainder_Lat != 0)
            {
                latDegreesLookupValue = latDegreesLookupValue - Remainder_Lat;
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
                if (Table4C2GLookupNegative.TryGetValue(latDegreesLookupValue, out string table4LookupResult))
                {
                    gridsquareSecondChar = table4LookupResult;
                }
            }
            if (gridsquareSecondChar == "-")
            {
                throw new Exception($"Gridsquare second character was not propertly set. Expecting [A-X] not a {gridsquareSecondChar}.");
            }
            else
            {
                Gridsquare += gridsquareSecondChar;
            }
        }

        public void GetFirstGridsquareCharacter()
        {
            // inputs: int DDM_LonDegrees
            // sets: LonDirection; Gridsquare (first character by concatination)
            int lonDegreesLookupValue = DDM_LonDegrees;
            string gridsquareFirstChar = "-";
            //LonDirection = 1;
            if (DDM_LonDegrees < 0)
            {   // use this to check pos/neg degrees and adjust sign accordingly as a multiplier
                LonDirection = -1;
            }
            else if (DDM_LonDegrees >= 0)
            {
                LonDirection = 1;
            }
            Remainder_Lon = DDM_LonDegrees % 20;
            if (Remainder_Lon != 0)
            {
                lonDegreesLookupValue = DDM_LonDegrees - Remainder_Lon;
            }
            if (LonDirection < 0)
            {
                if (Table1C2GLookupNegative.TryGetValue(lonDegreesLookupValue, out string table1LookupResult))
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
        public string ConvertDDtoDDM(string dd_coordinates)
        {
            // input: dd_coordinates as a string like 47.2917*N,122.8125*W
            // process: validate dd_coordinates
            // set: DD_Lattitude, DD_Longitude, DDM_LatDegrees, DDM_LatMinutes, DDM_LonDegrees, DDM_LonMinutes
            // output: DD_Lattitude and DD_Longitude as a contactenated string
            string returnString = "";
            dd_coordinates.Trim();
            if (dd_coordinates.Length < 7 || 21 < dd_coordinates.Length) // 1*N,2*W || 12.4567*N,123.5678*W
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
            foreach (char item in dd_coordinates)
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
            // select appropriate portions of the SB to define and set Lat Degrees, Lat Minutes, Lon Degrees, and Lon Minutes props
            int lat_decimal_index = sbLatCoords.ToString().IndexOf(".");
            DDM_LatDegrees = Math.Abs(int.Parse(sbLatCoords.ToString().Substring(0, lat_decimal_index)));
            double temp_latMinutes = double.Parse(sbLatCoords.ToString().Substring(lat_decimal_index + 1, (sbLatCoords.Length - (lat_decimal_index + 1))));

            int lon_decimal_index = sbLonCoords.ToString().IndexOf(".");
            DDM_LonDegrees = Math.Abs(int.Parse(sbLonCoords.ToString().Substring(0, lon_decimal_index)));
            double temp_lonMinutes = double.Parse(sbLonCoords.ToString().Substring(lon_decimal_index + 1, (sbLonCoords.Length - (lon_decimal_index +1))));

            DDM_LatMinutes = Math.Round((temp_latMinutes / 10_000) * 60, 2);
            DDM_LonMinutes = Math.Round((temp_lonMinutes / 10_000) * 60, 2);
            LatDirection = temp_LatDirection;
            LonDirection = temp_LonDirection;

            returnString = $"{DDM_LatDegrees}*{DDM_LatMinutes}'";
            if (LatDirection < 0)
            {
                returnString += "S,";
            }
            else
            { returnString += "N,";
            }
            returnString += $"{DDM_LonDegrees}*{DDM_LonMinutes}'";
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
        public string ConvertDMStoDDM(string dms_coordinates)
        {
            // input: dms_coordinates as a string like 47*48'45"N,122*17'30"W
            // process: validate dms_coordinates
            // set: DD_Lattitude, DD_Longitude, DMS_LatDegrees
            // output: DD_Lattitude and DD_Longitude converted to DMS as a contactenated string

            string returnString = "";
            dms_coordinates.Trim();
            if (dms_coordinates.Length < 15 || 22 < dms_coordinates.Length) // 1*2'3"N,2*3'4"W || 12*34'56"N,123*45'07"W
            {
                returnString = "Unable to process DD Coordinates that are less than 15 characters or more than 22 characters long.";
                returnString += "Use format DD*MM'SS\"[NS],DDD*MM'SS\"[EW]";
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
            char starDegrees = char.Parse("*");
            char Minutes = char.Parse("'");
            char Seconds = char.Parse("\"");
            int commaSeen = 0;

            //  47*48'45"N => 47, 48, 45/60=Rnd(0.75)
            //  122*17'30"W => 122, 17, 30/60=rnd(0.50)


            // TODO: Revamp the IF statements within the foreach loop following notes on physical notepad

            foreach (char item in dms_coordinates)
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
                {   
                    throw new ArgumentOutOfRangeException($"Incorrect input. Use the following format: [-]DD*MM.mm'[NS],[-]DDD*MM.mm'[EW]");
                }
            }
            // select appropriate portions of the SB to define and set Lat Degrees, Lat Minutes, Lon Degrees, and Lon Minutes props
            int lat_decimal_index = sbLatCoords.ToString().IndexOf(".");
            DDM_LatDegrees = Math.Abs(int.Parse(sbLatCoords.ToString().Substring(0, lat_decimal_index)));
            double temp_latMinutes = double.Parse(sbLatCoords.ToString().Substring(lat_decimal_index + 1, (sbLatCoords.Length - (lat_decimal_index + 1))));

            int lon_decimal_index = sbLonCoords.ToString().IndexOf(".");
            DDM_LonDegrees = Math.Abs(int.Parse(sbLonCoords.ToString().Substring(0, lon_decimal_index)));
            double temp_lonMinutes = double.Parse(sbLonCoords.ToString().Substring(lon_decimal_index + 1, (sbLonCoords.Length - (lon_decimal_index + 1))));

            DDM_LatMinutes = Math.Round((temp_latMinutes / 10_000) * 60, 2);
            DDM_LonMinutes = Math.Round((temp_lonMinutes / 10_000) * 60, 2);
            LatDirection = temp_LatDirection;
            LonDirection = temp_LonDirection;


            returnString = $"{DDM_LatDegrees}*{DDM_LatMinutes}'";


            if (LatDirection < 0)
            {
                returnString += "S,";
            }
            else
            {
                returnString += "N,";
            }
            returnString += $"{DDM_LonDegrees}*{DDM_LonMinutes}'";
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
        public void ConvertDDtoGridsquare(string dd_coordinates)
        {

        }
        public void ConvertDMStoGridsquare(string dms_coordinates)
        {

        }
        //public void ConvertToDMS(string lat_dd, string lon_dd, out string degreesMinutesSeconds)
        //{
        //    // Use LatDMS and LonDMS. Degrees Minutes Seconds: 1 Degree = 15 Minutes, 1 Minute = 60 Seconds
        //    // internally relies on LatDegrees, LatMinutes, LonDegrees, and LonMinutes as set by ValidateDDcoordinates() method
        //    //LatDirection;
        //    //LonDirection;
        //    if (lat_dd.Length < 6 || lat_dd.Length > 10 || lon_dd.Length < 7 || lon_dd.Length > 11)
        //    {
        //        throw new ArgumentOutOfRangeException($"Unable to convert an DD to DMS. LatDD: {lat_dd}; LonDD: {lon_dd}.");
        //    }
        //    else
        //    {
        //        ValidateDDcoordinates($"{lat_dd},{lon_dd}");

        //        LatDMS_Degrees = LatDegrees;
        //        LatDMS_Minutes = (int)LatMinutes * 60;
        //        LatDMS_Seconds = ((int)LatMinutes * 3600) - (60 * LatDMS_Minutes);  // yes LatMinutes and LatDMS_Minutes must both be used in the calculation

        //        LonDMS_Degrees = LonDegrees;
        //        LonDMS_Minutes = (int)LonMinutes * 60;
        //        LonDMS_Seconds = ((int)LonMinutes * 3600) - (60 * LonDMS_Minutes);
        //    }
        //    degreesMinutesSeconds = $"{LatDMS_Degrees}*{LatDMS_Minutes}\"{LatDMS_Seconds}',{LonDMS_Degrees}*{LonDMS_Minutes}\"{LonDMS_Seconds}'";
        //}




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
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}
