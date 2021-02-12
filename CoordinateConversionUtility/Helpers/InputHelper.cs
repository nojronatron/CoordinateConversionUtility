using CoordinateConversionUtility.Models;
using System;
using System.Text.RegularExpressions;

namespace CoordinateConversionUtility.Helpers
{
    public class InputHelper
    {
        private static string DdCommandPattern { get; set; }
        private static string DdmCommandPattern { get; set; }
        private static string DmsCommandPattern { get; set; }
        private static string GridCommandPattern { get; set; }
        private static string DdPattern { get; set; }
        private static string DwLatPattern { get; set; }
        private static string DdmPattern { get; set; }
        private static string DmsPattern { get; set; }

        public InputHelper()
        {
            DdCommandPattern = @"(-d{2})";
            DdmCommandPattern = @"(-d{2}m)";
            DmsCommandPattern = @"(-dms)";
            GridCommandPattern = @"{-grid}";
            DdPattern = @"(-??\s*?[0-9]{1,3}\.[0-9]*)";
            DwLatPattern = @"([nsew]\s*?[0-9]{1,3}\s*?[0-9]{1,2}\.[0-9]{1,4})";
            DdmPattern = @"([0-9]{1,3}\s*?[0-9]{1,2}\.[0-9]{1,2}[nsew])";
            DmsPattern = @"([nsew]\s*?[0-9]{1,3})\s*?([0-9]{1,2})\s*?([0-9]{1,2}\.[0-9]{1,2})";   //  N47 49 31.2, W122 17 36.0
        }

        /// <summary>
        /// Searches input for a user command e.g.: -dd, -ddm, or -dms and returns the first matched instance.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public string GetCommand(string inputString)
        {
            if (Regex.Matches(inputString, DdCommandPattern, RegexOptions.IgnoreCase, new TimeSpan(0, 0, 1)).Count > 0)
            {
                return "-dd";
            }

            if (Regex.Matches(inputString, DdmCommandPattern, RegexOptions.IgnoreCase, new TimeSpan(0, 0, 1)).Count > 0)
            {
                return "-ddm";
            }

            if (Regex.Matches(inputString, DmsCommandPattern, RegexOptions.IgnoreCase, new TimeSpan(0, 0, 1)).Count > 0)
            {
                return "-dms";
            }

            if (Regex.Matches(inputString, GridCommandPattern, RegexOptions.IgnoreCase, new TimeSpan(0, 0, 1)).Count > 0)
            {
                return "-grid";
            }

            return string.Empty;
        }

        /// <summary>
        /// Calls underlying Library to validate the input string as a possible Gridsquare.
        /// If valid returns true and sets validatedGrid with clean string version of the input.
        /// If not valid returns false and sets validatedGrid as an empty string.
        /// </summary>
        /// <param name="gridsquareToParse"></param>
        /// <param name="validatedGrid"></param>
        /// <returns></returns>
        public bool IsGridsquare(string gridsquareToParse, out string validatedGrid)
        {
            validatedGrid = "";

            if (string.IsNullOrEmpty(gridsquareToParse) || string.IsNullOrWhiteSpace(gridsquareToParse))
            {
                return false;
            }

            gridsquareToParse = gridsquareToParse.Trim();
            var gs = new GridSquareHelper();

            if (gs.ValidateGridsquareInput(gridsquareToParse, out string validatedGridsquare))
            {
                validatedGrid = validatedGridsquare;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Takes a string and attempts to parse with RegEx. Preceeding minus sign required for degrees less than 0.
        /// If parseable (probably valid), returns True and out a pretty DD Coordinate string.
        /// If not parseable, returns False and sets the out string empty.
        /// </summary>
        /// <param name="DdmNSEW"></param>
        /// <param name="validDDM"></param>
        /// <returns></returns>
        public bool IsDD(string coordinateToParse, out string validDD)
        {
            validDD = string.Empty;
            MatchCollection patternMatches = Regex.Matches(coordinateToParse, DdPattern, RegexOptions.IgnoreCase, new TimeSpan(0, 0, 1));

            if (patternMatches.Count > 0)
            {
                string ddLattitudeRaw = patternMatches[0].Value.Trim();
                string ddLongitudeRaw = patternMatches[1].Value.Trim();
                decimal LatDegrees = 0.0m;
                decimal LonDegrees = 0.0m;

                if (ConversionHelper.ValidateIsLattitude(ddLattitudeRaw, out decimal validLattitude))
                {
                    LatDegrees = validLattitude;
                }

                if (ConversionHelper.ValidateIsLongitude(ddLongitudeRaw, out decimal validLongitude))
                {
                    LonDegrees = validLongitude;
                }

                var tempDDCoordinate = new DDCoordinate(LatDegrees, LonDegrees);
                validDD = tempDDCoordinate.ToString();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Takes a string and attempts to parse with RegEx. Directional NSEW required.
        /// Set param direwolf true if accepting output from DIREWOLF app.
        /// If parseable (probably valid), returns True and out a pretty DDMCoordinate string.
        /// If not parseable, returns False and sets the out string empty.
        /// </summary>
        /// <param name="DdmNSEW"></param>
        /// <param name="direwolf"></param>
        /// <param name="validDDM"></param>
        /// <returns></returns>
        public bool IsDDM(string coordinateToParse, bool direwolf, out string validDDM)
        {
            validDDM = string.Empty;
            string pattern = string.Empty;
            var tempDDMCoordinate = new DDMCoordinate();

            if (direwolf)
            {
                pattern = DwLatPattern;
            }
            else
            {
                pattern = DdmPattern;
            }

            MatchCollection patternMatches = Regex.Matches(coordinateToParse, pattern, RegexOptions.IgnoreCase, new TimeSpan(0, 0, 1));

            if (patternMatches.Count > 0)
            {
                string ddmLattitudeRaw = patternMatches[0].Value.Trim();
                string ddmLongitudeRaw = patternMatches[1].Value.Trim();
                int nsPolarity = ConversionHelper.ExtractPolarityNS(ddmLattitudeRaw);
                int ewPolarity = ConversionHelper.ExtractPolarityEW(ddmLongitudeRaw);

                if (nsPolarity == 0 || ewPolarity == 0)
                {
                    return false;
                }

                int nsIdx;

                if (nsPolarity > 0)
                {
                    nsIdx = ddmLattitudeRaw.ToUpper().IndexOf("N");
                }
                else
                {
                    nsIdx = ddmLattitudeRaw.ToUpper().IndexOf("S");
                }

                ddmLattitudeRaw = ddmLattitudeRaw.Remove(nsIdx, 1);
                ddmLattitudeRaw = ddmLattitudeRaw.Trim();
                int ewIdx;

                if (ewPolarity > 0)
                {
                    ewIdx = ddmLongitudeRaw.ToUpper().IndexOf("E");
                }
                else
                {
                    ewIdx = ddmLongitudeRaw.ToUpper().IndexOf("W");
                }

                ddmLongitudeRaw = ddmLongitudeRaw.Remove(ewIdx, 1);
                ddmLongitudeRaw = ddmLongitudeRaw.Trim();
                decimal latDegrees = 0m;
                decimal latMinutes = 0m;
                decimal lonDegrees = 0m;
                decimal lonMinutes = 0m;

                if (ConversionHelper.ValidateIsLattitude(ddmLattitudeRaw.Split(' ')[0], out decimal validLattitude))
                {
                    latDegrees = Math.Abs(validLattitude) * nsPolarity;
                }

                if (ConversionHelper.ValidateIsSecsOrMins(ddmLattitudeRaw.Split(' ')[1], out decimal validLatMins))
                {
                    latMinutes = Math.Abs(validLatMins);
                }

                if (ConversionHelper.ValidateIsLongitude(ddmLongitudeRaw.Split(' ')[0], out decimal validLongitude))
                {
                    lonDegrees = Math.Abs(validLongitude) * ewPolarity;
                }

                if (ConversionHelper.ValidateIsSecsOrMins(ddmLongitudeRaw.Split(' ')[1], out decimal validLonMins))
                {
                    lonMinutes = Math.Abs(validLonMins);
                }

                tempDDMCoordinate = new DDMCoordinate(latDegrees, latMinutes, lonDegrees, lonMinutes);
                validDDM = tempDDMCoordinate.ToString();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Takes a string and attempts to parse with RegEx. Directional NSEW required.
        /// Set param direwolf true if accepting output from DIREWOLF app.
        /// If parseable (probably valid), returns True and out a pretty DDMCoordinate string.
        /// If not parseable, returns False and sets the out string empty.
        /// </summary>
        /// <param name="DmsNSEW"></param>
        /// <param name="validDDM"></param>
        /// <returns></returns>
        public bool IsDMS(string coordinateToParse, out string validDMS)
        {
            validDMS = string.Empty;
            var tempDMSCoordinate = new DDMCoordinate();
            MatchCollection patternMatches = Regex.Matches(coordinateToParse, DmsPattern, RegexOptions.IgnoreCase, new TimeSpan(0, 0, 1));

            if (patternMatches.Count > 0)
            {
                string dmsLattitudeRaw = patternMatches[0].Value.Trim();
                string dmsLongitudeRaw = patternMatches[1].Value.Trim();
                string dmsLatDegreesRaw = patternMatches[0].Groups[1].Value.Trim();
                string dmsLonDegreesRaw = patternMatches[1].Groups[1].Value.Trim();
                string dmsLatMinsRaw = patternMatches[0].Groups[2].Value.Trim();
                string dmsLonMinsRaw = patternMatches[1].Groups[2].Value.Trim();
                string dmsLatSecsRaw = patternMatches[0].Groups[3].Value.Trim();
                string dmsLonSecsRaw = patternMatches[1].Groups[3].Value.Trim();
                int nsPolarity = ConversionHelper.ExtractPolarityNS(dmsLatDegreesRaw);
                int ewPolarity = ConversionHelper.ExtractPolarityEW(dmsLonDegreesRaw);

                if (nsPolarity == 0 || ewPolarity == 0)
                {
                    return false;
                }

                int nsIdx;

                if (nsPolarity > 0)
                {
                    nsIdx = dmsLatDegreesRaw.ToUpper().IndexOf("N");
                }
                else
                {
                    nsIdx = dmsLatDegreesRaw.ToUpper().IndexOf("S");
                }

                dmsLatDegreesRaw = dmsLatDegreesRaw.Remove(nsIdx, 1);
                dmsLatDegreesRaw = dmsLatDegreesRaw.Trim();
                int ewIdx;

                if (ewPolarity > 0)
                {
                    ewIdx = dmsLonDegreesRaw.ToUpper().IndexOf("E");
                }
                else
                {
                    ewIdx = dmsLonDegreesRaw.ToUpper().IndexOf("W");
                }

                dmsLonDegreesRaw = dmsLonDegreesRaw.Remove(ewIdx, 1);
                dmsLonDegreesRaw = dmsLonDegreesRaw.Trim();
                decimal latDegrees = 0m;
                decimal latMinutes = 0m;
                decimal latSeconds = 0m;
                decimal lonDegrees = 0m;
                decimal lonMinutes = 0m;
                decimal lonSeconds = 0m;

                if (ConversionHelper.ValidateIsLattitude(dmsLatDegreesRaw, out decimal validLatDegrees))
                {
                    latDegrees = Math.Abs(validLatDegrees) * nsPolarity;
                }

                if (ConversionHelper.ValidateIsSecsOrMins(dmsLatMinsRaw, out decimal validLatMins))
                {
                    latMinutes = Math.Abs(validLatMins);
                }

                if (ConversionHelper.ValidateIsSecsOrMins(dmsLatSecsRaw, out decimal validLatSecs))
                {
                    latSeconds = Math.Abs(validLatSecs);
                }

                if (ConversionHelper.ValidateIsLongitude(dmsLonDegreesRaw, out decimal validLonDegrees))
                {
                    lonDegrees = Math.Abs(validLonDegrees) * ewPolarity;
                }

                if (ConversionHelper.ValidateIsSecsOrMins(dmsLonMinsRaw, out decimal validLonMins))
                {
                    lonMinutes = Math.Abs(validLonMins);
                }

                if (ConversionHelper.ValidateIsSecsOrMins(dmsLonSecsRaw, out decimal validLonSecs))
                {
                    lonSeconds = Math.Abs(validLonSecs);
                }

                tempDMSCoordinate = new DMSCoordinate(latDegrees, latMinutes, latSeconds, lonDegrees, lonMinutes, lonSeconds);
                validDMS = tempDMSCoordinate.ToString();
                return true;
            }

            return false;
        }

    }
}
