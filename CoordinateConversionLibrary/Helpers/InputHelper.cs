using CoordinateConversionLibrary.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CoordinateConversionLibrary.Helpers
{
    public static class InputHelper
    {
        private static readonly CultureInfo currentCulture = CultureInfo.CurrentCulture;
        private static string DdCommandPattern => @"(-dd)";
        private static string DwCommandPattern => @"(-direwolf)";
        private static string DdmCommandPattern => @"(-ddm)";
        private static string DmsCommandPattern => @"(-dms)";
        private static string GridCommandPattern => @"(-grid)";
        private static string DashHelpPattern => @"-h|--help";
        private static string DdPattern => @"(-?\s*[0-9]{1,3}(\.[0-9]{0,5})?)";    //  the decimal point might be missing but is still a valid coordinate
        private static string DwPattern => @"([nsew]\s*?[0-9]{1,3}\s*?[0-9]{1,2}\.[0-9]{1,4})";
        private static string DdmPattern => @"([0-9]{1,3}\s*[0-9]{1,2}(\.[0-9]{1,2})?\s*[nsew])";
        private static string DmsPattern => @"(\s*[nsew]\s*[0-9]{1,3})\s*([0-9]{1,2})\s*([0-9]{1,2}(\.[0-9]{1,2})?)\s*";

        private static TimeSpan Timespan => new(0, 0, 1);

        private static string LimitInputSpacing(string latOrLonPatternMatch)
        {
            var sb = new StringBuilder();
            string[] regexSplit = Regex.Split(latOrLonPatternMatch, @"\s+");

            foreach (string item in regexSplit)
            {
                sb.Append(item.Trim());
                sb.Append(' ');
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Processes sanitized inputCommand, inputArg, and outputCommand. Outputs a string representation of the conversion from inputArg to outputCommand format.
        /// </summary>
        /// <param name="inputCommand"></param>
        /// <param name="inputArg"></param>
        /// <param name="outputCommand"></param>
        /// <returns></returns>
        public static string OutputCommandProcessor(string inputCommand, string inputArg, string outputCommand)
        {
            if (string.IsNullOrEmpty(inputCommand) || string.IsNullOrWhiteSpace(inputCommand))
            {
                return string.Empty;
            }

            string result = string.Empty;
            var cc = new GridDdmExpert();

            switch (outputCommand)
            {
                case "-grid":
                    {
                        DDMCoordinate ddm = null;

                        if (inputCommand == "-grid")
                        {
                            result = inputArg;
                            break;
                        }
                        if (inputCommand == "-dms")
                        {
                            var dms = new DMSCoordinate(inputArg);
                            ddm = new DDMCoordinate(
                                dms.DegreesLattitude, dms.MinutesLattitude, dms.SecondsLattitude, dms.DegreesLongitude, dms.MinutesLongitude, dms.SecondsLongitude);
                        }
                        if (inputCommand == "-ddm" || inputCommand == "-direwolf")
                        {
                            ddm = new DDMCoordinate(inputArg);
                        }
                        if (inputCommand == "-dd")
                        {
                            var dd = new DDCoordinate(inputArg);
                            ddm = new DDMCoordinate(dd.GetLattitudeDD(), dd.GetLongitudeDD());
                        }

                        result = cc.ConvertDDMtoGridsquare(ddm);
                        break;
                    }
                case "-dms":
                    {
                        DMSCoordinate dms = null;

                        if (inputCommand == "-grid")
                        {
                            cc = new GridDdmExpert();
                            DDMCoordinate ddm = cc.ConvertGridsquareToDDM(inputArg);
                            dms = new DMSCoordinate(ddm.GetShortDegreesLat(), ddm.GetMinsLat(), ddm.GetShortDegreesLon(), ddm.GetMinsLon());
                        }
                        if (inputCommand == "-dms")
                        {
                            dms = new DMSCoordinate(inputArg);
                        }
                        if (inputCommand == "-ddm" || inputCommand == "-direwolf")
                        {
                            var ddm = new DDMCoordinate(inputArg);
                            dms = new DMSCoordinate(ddm.GetShortDegreesLat(), ddm.GetMinsLat(), ddm.GetShortDegreesLon(), ddm.GetMinsLon());
                        }
                        if (inputCommand == "-dd")
                        {
                            var dd = new DDCoordinate(inputArg);
                            dms = new DMSCoordinate(dd.GetLattitudeDD(), dd.GetLongitudeDD());
                        }

                        result = dms.ToString();
                        break;
                    }
                case "-ddm":
                    {
                        DDMCoordinate ddm = null;

                        if (inputCommand == "-grid")
                        {
                            cc = new GridDdmExpert();
                            ddm = cc.ConvertGridsquareToDDM(inputArg);
                        }
                        if (inputCommand == "-dms")
                        {
                            var dms = new DMSCoordinate(inputArg);
                            ddm = new DDMCoordinate(
                                dms.GetShortDegreesLat(), dms.GetMinsLat(), dms.GetSecondsLattitude(),
                                dms.GetShortDegreesLon(), dms.GetMinsLon(), dms.GetSecondsLongitude());
                        }
                        if (inputCommand == "-ddm" || inputCommand == "-direwolf")
                        {
                            ddm = new DDMCoordinate(inputArg);
                        }
                        if (inputCommand == "-dd")
                        {
                            var dd = new DDCoordinate(inputArg);
                            ddm = new DDMCoordinate(dd.GetLattitudeDD(), dd.GetLongitudeDD());
                        }

                        result = ddm.ToString();
                        break;
                    }
                case "-dd":
                    {
                        DDCoordinate dd = null;

                        if (inputCommand == "-ddm" || inputCommand == "-direwolf")
                        {
                            var ddm = new DDMCoordinate(inputArg);
                            dd = new DDCoordinate(ddm.GetShortDegreesLat(), ddm.MinutesLattitude, ddm.GetShortDegreesLon(), ddm.MinutesLongitude);
                        }
                        if (inputCommand == "-grid")
                        {
                            cc = new GridDdmExpert();
                            DDMCoordinate ddm = cc.ConvertGridsquareToDDM(inputArg);
                            dd = new DDCoordinate(ddm.GetShortDegreesLat(), ddm.GetMinsLat(), ddm.GetShortDegreesLon(), ddm.GetMinsLon());
                        }
                        if (inputCommand == "-dms")
                        {
                            var dms = new DMSCoordinate(inputArg);
                            dd = new DDCoordinate(
                                dms.GetShortDegreesLat(), dms.GetMinsLat(), dms.GetSecondsLattitude(),
                                dms.GetShortDegreesLon(), dms.GetMinsLon(), dms.GetSecondsLongitude());
                        }
                        if (inputCommand == "-dd")
                        {
                            dd = new DDCoordinate(inputArg);
                        }

                        result = dd.ToString();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return result;
        }

        /// <summary>
        /// Searches input for a user command e.g.: -dd, -ddm, or -dms and returns the first matched instance.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string GetCommand(string inputString)
        {
            if (Regex.Matches(inputString, DwCommandPattern, RegexOptions.IgnoreCase, Timespan).Count > 0)
            {
                return "-direwolf";
            }

            if (Regex.Matches(inputString, GridCommandPattern, RegexOptions.IgnoreCase, Timespan).Count > 0)
            {
                return "-grid";
            }

            if (Regex.Matches(inputString, DashHelpPattern, RegexOptions.IgnoreCase, Timespan).Count > 0)
            {
                return "-h";
            }

            if (Regex.Matches(inputString, DmsCommandPattern, RegexOptions.IgnoreCase, Timespan).Count > 0)
            {
                return "-dms";
            }

            if (Regex.Matches(inputString, DdmCommandPattern, RegexOptions.IgnoreCase, Timespan).Count > 0)
            {
                return "-ddm";
            }

            if (Regex.Matches(inputString, DdCommandPattern, RegexOptions.IgnoreCase, Timespan).Count > 0)
            {
                return "-dd";
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
        public static bool IsGridsquare(string gridsquareToParse, out string validatedGrid)
        {
            var gs = new GridSquareHelper();

            if (gs.ValidateGridsquareInput(gridsquareToParse, out string validatedGridsquare))
            {
                validatedGrid = validatedGridsquare;
                return true;
            }

            validatedGrid = "";
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
        public static bool ParseAsDDCoordinate(string coordinateToParse, out string validDD)
        {
            validDD = string.Empty;
            MatchCollection patternMatches = Regex.Matches(coordinateToParse, DdPattern, RegexOptions.IgnoreCase, Timespan);

            if (patternMatches.Count == 2)
            {
                string ddLattitudeRaw = LimitInputSpacing(patternMatches[0].Value);
                string ddLongitudeRaw = LimitInputSpacing(patternMatches[1].Value);
                decimal latDegrees = default;
                decimal lonDegrees = default;

                if (DDCoordinate.ValidateIsLatDegrees(ddLattitudeRaw, out decimal validLattitude))
                {
                    latDegrees = validLattitude;
                }
                else
                {
                    return false;
                }

                if (DDCoordinate.ValidateIsLonDegrees(ddLongitudeRaw, out decimal validLongitude))
                {
                    lonDegrees = validLongitude;
                }
                else
                {
                    return false;
                }

                var tempDDCoordinate = new DDCoordinate(latDegrees, lonDegrees);

                if (tempDDCoordinate.IsValid)
                {
                    validDD = tempDDCoordinate.ToString();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Takes a string lattitude (or longitude) and attempts to separate the integer component from the decimal component based on the decimal point position.
        /// Returns true and out string[] the separated components if there is a decimal point, otherwise returns false and string[] will both be string.Empty.
        /// </summary>
        /// <param name="latOrLonToParse"></param>
        /// <param name="fixedLatOrLon"></param>
        /// <returns></returns>
        public static bool DDMCoordFixer(string latOrLonToParse, out string[] fixedLatOrLon)
        {
            var decimalIdx = latOrLonToParse.IndexOf('.');

            if (decimalIdx >= 0)
            {
                var integerPart = latOrLonToParse.Substring(0, decimalIdx - 2);
                var decimalPart = latOrLonToParse.Substring(decimalIdx - 2);
                fixedLatOrLon = new string[] { integerPart.Trim(), decimalPart.Trim() };
                return true;
            }

            fixedLatOrLon = new string[] { string.Empty, string.Empty };
            return false;
        }

        /// <summary>
        /// Takes a string and attempts to parse with RegEx.
        /// Set param direwolf to true if accepting output from DIREWOLF app.
        /// Returns true and out a pretty validDDM string if parseable, otherwise false and out string.Empty.
        /// </summary>
        /// <param name="coordinateToParse"></param>
        /// <param name="direwolf"></param>
        /// <param name="validDDM"></param>
        /// <returns></returns>
        public static bool ParseAsDDMCoordinate(string coordinateToParse, bool direwolf, out string validDDM)
        {
            validDDM = string.Empty;
            string pattern;

            if (direwolf)
            {
                pattern = DwPattern;
            }
            else
            {
                pattern = DdmPattern;
            }

            MatchCollection patternMatches = Regex.Matches(coordinateToParse, pattern, RegexOptions.IgnoreCase, Timespan);

            if (patternMatches.Count == 2)
            {
                string[] patternMatch =
                {
                    LimitInputSpacing(patternMatches[0].Value),
                    LimitInputSpacing(patternMatches[1].Value)
                };

                var coordinates = new List<string>(4)
                {
                    patternMatch[0], "", patternMatch[1], ""
                };

                for (int i = 0; i < coordinates.Capacity; i += 2)
                {
                    if (DDMCoordFixer(coordinates[i], out string[] fixedItems))
                    {
                        coordinates[i] = fixedItems[0].ToUpper(currentCulture);
                        coordinates[i + 1] = fixedItems[1].ToUpper(currentCulture);
                    }
                    else
                    {
                        string thisCoord;
                        int spaceIdx = -1;
                        if (coordinates[i].Length > 0)
                        {
                            thisCoord = coordinates[i];
                            spaceIdx = thisCoord.IndexOf(' ');
                            coordinates[i] = thisCoord.Substring(0, spaceIdx);
                            coordinates[i + 1] = thisCoord.Substring(spaceIdx+1);
                        }
                        else
                        { 
                            return false;
                        }
                    }
                }

                int nsPolarity = 0;
                int ewPolarity = 0;

                for (int j = 0; j < coordinates.Capacity; j++)
                {
                    int nsIdx = -1;
                    string item = coordinates[j];

                    if (item.Contains("N"))
                    {
                        nsIdx = item.IndexOf("N", StringComparison.CurrentCultureIgnoreCase);
                        nsPolarity = 1;
                        coordinates[j] = item.Remove(nsIdx, 1);
                    }
                    if (item.Contains("S"))
                    {
                        nsIdx = item.IndexOf("S", StringComparison.CurrentCultureIgnoreCase);
                        nsPolarity = -1;
                        coordinates[j] = item.Remove(nsIdx, 1);
                    }

                    int ewIdx = -1;

                    if (item.Contains("E"))
                    {
                        ewIdx = item.IndexOf("E", StringComparison.CurrentCultureIgnoreCase);
                        ewPolarity = 1;
                        coordinates[j] = item.Remove(ewIdx, 1);
                    }
                    if (item.Contains("W"))
                    {
                        ewIdx = item.IndexOf("W", StringComparison.CurrentCultureIgnoreCase);
                        ewPolarity = -1;
                        coordinates[j] = item.Remove(ewIdx, 1);
                    }
                }

                if (nsPolarity == 0 || ewPolarity == 0)
                {
                    return false;
                }

                decimal latDegrees = default;
                decimal latMinutes = default;
                decimal lonDegrees = default;
                decimal lonMinutes = default;

                if (DDMCoordinate.ValidateIsLatDegrees(coordinates[0], out decimal validLattitude))
                {
                    latDegrees = Math.Abs(validLattitude) * nsPolarity;
                }
                else
                {
                    return false;
                }

                if (DDMCoordinate.ValidateIsMinutes(coordinates[1], out decimal validLatMins))
                {
                    latMinutes = Math.Abs(validLatMins);
                }
                else
                {
                    return false;
                }

                if (DDMCoordinate.ValidateIsLonDegrees(coordinates[2], out decimal validLongitude))
                {
                    lonDegrees = Math.Abs(validLongitude) * ewPolarity;
                }
                else
                {
                    return false;
                }

                if (DDMCoordinate.ValidateIsMinutes(coordinates[3], out decimal validLonMins))
                {
                    lonMinutes = Math.Abs(validLonMins);
                }
                else
                {
                    return false;
                }

                var tempDDMCoordinate = new DDMCoordinate(latDegrees, latMinutes, lonDegrees, lonMinutes);

                if (tempDDMCoordinate.IsValid)
                {
                    validDDM = tempDDMCoordinate.ToString();
                    return true;
                }
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
        public static bool ParseAsDMSCoordinate(string coordinateToParse, out string validDMS)
        {
            validDMS = string.Empty;
            MatchCollection patternMatches = Regex.Matches(coordinateToParse, DmsPattern, RegexOptions.IgnoreCase, Timespan);

            if (patternMatches.Count == 2)
            {
                string dmsLatDegreesRaw = LimitInputSpacing(patternMatches[0].Groups[1].Value);
                string dmsLonDegreesRaw = LimitInputSpacing(patternMatches[1].Groups[1].Value);
                string dmsLatMinsRaw = LimitInputSpacing(patternMatches[0].Groups[2].Value);
                string dmsLonMinsRaw = LimitInputSpacing(patternMatches[1].Groups[2].Value);
                string dmsLatSecsRaw = LimitInputSpacing(patternMatches[0].Groups[3].Value);
                string dmsLonSecsRaw = LimitInputSpacing(patternMatches[1].Groups[3].Value);

                int nsPolarity = ConversionHelper.ExtractPolarityNS(dmsLatDegreesRaw);
                int ewPolarity = ConversionHelper.ExtractPolarityEW(dmsLonDegreesRaw);

                if (nsPolarity == 0 || ewPolarity == 0)
                {
                    return false;
                }

                int nsIdx;

                if (nsPolarity > 0)
                {
                    nsIdx = dmsLatDegreesRaw.ToUpper(currentCulture).IndexOf("N", StringComparison.CurrentCultureIgnoreCase);
                }
                else
                {
                    nsIdx = dmsLatDegreesRaw.ToUpper(currentCulture).IndexOf("S", StringComparison.CurrentCultureIgnoreCase);
                }

                dmsLatDegreesRaw = dmsLatDegreesRaw.Remove(nsIdx, 1);
                dmsLatDegreesRaw = dmsLatDegreesRaw.Trim();
                int ewIdx;

                if (ewPolarity > 0)
                {
                    ewIdx = dmsLonDegreesRaw.ToUpper(currentCulture).IndexOf("E", StringComparison.CurrentCultureIgnoreCase);
                }
                else
                {
                    ewIdx = dmsLonDegreesRaw.ToUpper(currentCulture).IndexOf("W", StringComparison.CurrentCultureIgnoreCase);
                }

                dmsLonDegreesRaw = dmsLonDegreesRaw.Remove(ewIdx, 1);
                dmsLonDegreesRaw = dmsLonDegreesRaw.Trim();
                decimal latDegrees = default;
                decimal latMinutes = default;
                decimal latSeconds = default;
                decimal lonDegrees = default;
                decimal lonMinutes = default;
                decimal lonSeconds = default;

                if (DMSCoordinate.ValidateIsLatDegrees(dmsLatDegreesRaw, out decimal validLatDegrees))
                {
                    latDegrees = Math.Abs(validLatDegrees) * nsPolarity;
                }
                else
                {
                    return false;
                }

                if (DMSCoordinate.ValidateIsMinutes(dmsLatMinsRaw, out decimal validLatMins))
                {
                    latMinutes = Math.Abs(validLatMins);
                }
                else
                {
                    return false;
                }

                if (DMSCoordinate.ValidateIsSeconds(dmsLatSecsRaw, out decimal validLatSecs))
                {
                    latSeconds = Math.Abs(validLatSecs);
                }
                else
                {
                    return false;
                }

                if (DMSCoordinate.ValidateIsLonDegrees(dmsLonDegreesRaw, out decimal validLonDegrees))
                {
                    lonDegrees = Math.Abs(validLonDegrees) * ewPolarity;
                }
                else
                {
                    return false;
                }

                if (DMSCoordinate.ValidateIsMinutes(dmsLonMinsRaw, out decimal validLonMins))
                {
                    lonMinutes = Math.Abs(validLonMins);
                }
                else
                {
                    return false;
                }

                if (DMSCoordinate.ValidateIsSeconds(dmsLonSecsRaw, out decimal validLonSecs))
                {
                    lonSeconds = Math.Abs(validLonSecs);
                }
                else
                {
                    return false;
                }

                var tempDMSCoordinate = new DMSCoordinate(latDegrees, latMinutes, latSeconds, lonDegrees, lonMinutes, lonSeconds);

                if (tempDMSCoordinate.IsValid)
                {
                    validDMS = tempDMSCoordinate.ToString();
                    return true;
                }
            }

            return false;
        }

    }
}
