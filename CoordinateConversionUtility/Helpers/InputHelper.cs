using CoordinateConversionUtility.Models;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CoordinateConversionUtility.Helpers
{
    public class InputHelper
    {
        private static string DwCommandPattern { get; set; }
        private static string DdCommandPattern { get; set; }
        private static string DdmCommandPattern { get; set; }
        private static string DmsCommandPattern { get; set; }
        private static string GridCommandPattern { get; set; }
        private static string DashHelpPattern { get; set; }
        private static string DdPattern { get; set; }
        private static string DwPattern { get; set; }
        private static string DdmPattern { get; set; }
        private static string DmsPattern { get; set; }
        
        private static TimeSpan Timespan { get; set; }

        public InputHelper()
        {
            DdCommandPattern = @"(-dd)";
            DdmCommandPattern = @"(-ddm)";
            DmsCommandPattern = @"(-dms)";
            DwCommandPattern = @"(-direwolf)";
            GridCommandPattern = @"(-grid)";
            DdPattern = @"(-??\s*?[0-9]{1,3}\.[0-9]*)";
            DwPattern = @"([nsew]\s*?[0-9]{1,3}\s*?[0-9]{1,2}\.[0-9]{1,4})";
            DdmPattern = @"([0-9]{1,3}\s*?[0-9]{1,2}\.[0-9]{1,2}[nsew])";
            DmsPattern = @"([nsew]\s*?[0-9]{1,3})\s*?([0-9]{1,2})\s*?([0-9]{1,2}\.[0-9]{1,2})";
            DashHelpPattern = @"-h|--help";
            Timespan = new TimeSpan(0, 0, 1);
        }

        private string LimitInputSpacing(string latOrLonPatternMatch)
        {
            var sb = new StringBuilder();
            var regexSplit = Regex.Split(latOrLonPatternMatch, @"\s+");
            foreach (var item in regexSplit)
            {
                sb.Append(item.Trim());
                sb.Append(" ");
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
        public string OutputCommandProcessor(string inputCommand, string inputArg, string outputCommand)
        {
            string result = string.Empty;
            var cc = new CoordinateConverter();

            switch (outputCommand)
            {
                case "-grid":
                    {
                        DDMCoordinate ddm = null;

                        if (inputCommand.Equals("-grid"))
                        {
                            result = inputArg;
                            break;
                        }
                        if (inputCommand.Equals("-dms"))
                        {
                            var dms = new DMSCoordinate(inputArg);
                            ddm = new DDMCoordinate(
                                dms.DegreesLattitude, dms.MinutesLattitude, dms.SecondsLattitude, dms.DegreesLongitude, dms.MinutesLongitude, dms.SecondsLongitude);
                        }
                        if (inputCommand.Equals("-ddm") || inputCommand.Equals("-direwolf"))
                        {
                            ddm = new DDMCoordinate(inputArg);
                        }
                        if (inputCommand.Equals("-dd"))
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

                        if (inputCommand.Equals("-grid"))
                        {
                            cc = new CoordinateConverter();
                            var ddm = cc.ConvertGridsquareToDDM(inputArg);
                            dms = new DMSCoordinate(ddm.GetShortDegreesLat(), ddm.GetMinsLat(), ddm.GetShortDegreesLon(), ddm.GetMinsLon());
                        }
                        if (inputCommand.Equals("-dms"))
                        {
                            dms = new DMSCoordinate(inputArg);
                        }
                        if (inputCommand.Equals("-ddm") || inputCommand.Equals("-direwolf"))
                        {
                            var ddm = new DDMCoordinate(inputArg);
                            dms = new DMSCoordinate(ddm.GetShortDegreesLat(), ddm.GetMinsLat(), ddm.GetShortDegreesLon(), ddm.GetMinsLon());
                        }
                        if (inputCommand.Equals("-dd"))
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

                        if (inputCommand.Equals("-grid"))
                        {
                            cc = new CoordinateConverter();
                            ddm = cc.ConvertGridsquareToDDM(inputArg);
                        }
                        if (inputCommand.Equals("-dms"))
                        {
                            var dms = new DMSCoordinate(inputArg);
                            ddm = new DDMCoordinate(
                                dms.GetShortDegreesLat(), dms.GetMinsLat(), dms.GetSecondsLattitude(),
                                dms.GetShortDegreesLon(), dms.GetMinsLon(), dms.GetSecondsLongitude());
                        }
                        if (inputCommand.Equals("-ddm") || inputCommand.Equals("-direwolf"))
                        {
                            ddm = new DDMCoordinate(inputArg);
                        }
                        if (inputCommand.Equals("-dd"))
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

                        if (inputCommand.Equals("-ddm") || inputCommand.Equals("-direwolf"))
                        {
                            var ddm = new DDMCoordinate(inputArg);
                            dd = new DDCoordinate(ddm.GetShortDegreesLat(), ddm.MinutesLattitude, ddm.GetShortDegreesLon(), ddm.MinutesLongitude);
                        }
                        if (inputCommand.Equals("-grid"))
                        {
                            cc = new CoordinateConverter();
                            var ddm = cc.ConvertGridsquareToDDM(inputArg);
                            dd = new DDCoordinate(ddm.GetShortDegreesLat(), ddm.GetMinsLat(), ddm.GetShortDegreesLon(), ddm.GetMinsLon());
                        }
                        if (inputCommand.Equals("-dms"))
                        {
                            var dms = new DMSCoordinate(inputArg);
                            dd = new DDCoordinate(
                                dms.GetShortDegreesLat(), dms.GetMinsLat(), dms.GetSecondsLattitude(),
                                dms.GetShortDegreesLon(), dms.GetMinsLon(), dms.GetSecondsLongitude());
                        }
                        if (inputCommand.Equals("-dd"))
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
        public string GetCommand(string inputString)
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
            MatchCollection patternMatches = Regex.Matches(coordinateToParse, DdPattern, RegexOptions.IgnoreCase, Timespan);

            if (patternMatches.Count > 0)
            {
                string ddLattitudeRaw = LimitInputSpacing(patternMatches[0].Value);
                string ddLongitudeRaw = LimitInputSpacing(patternMatches[1].Value);
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
                pattern = DwPattern;
            }
            else
            {
                pattern = DdmPattern;
            }

            MatchCollection patternMatches = Regex.Matches(coordinateToParse, pattern, RegexOptions.IgnoreCase, Timespan);

            if (patternMatches.Count > 0)
            {
                string ddmLattitudeRaw = string.Empty;
                string firstPatternMatch = LimitInputSpacing(patternMatches[0].Value);
                firstPatternMatch = firstPatternMatch.ToUpper();
                
                string ddmLongitudeRaw = "";
                string secondPatternMatch = LimitInputSpacing(patternMatches[1].Value);
                secondPatternMatch = secondPatternMatch.ToUpper();

                if (firstPatternMatch.IndexOf("N") > -1 || firstPatternMatch.IndexOf("S") > -1)
                {
                    ddmLattitudeRaw = firstPatternMatch;
                    ddmLongitudeRaw = secondPatternMatch;
                }
                else if (firstPatternMatch.IndexOf("E") > -1 || firstPatternMatch.IndexOf("W") > -1)
                {
                    ddmLongitudeRaw = firstPatternMatch;
                    ddmLattitudeRaw = secondPatternMatch;
                }
                else
                {
                    return false;
                }

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
            MatchCollection patternMatches = Regex.Matches(coordinateToParse, DmsPattern, RegexOptions.IgnoreCase, Timespan);

            if (patternMatches.Count > 0)
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
