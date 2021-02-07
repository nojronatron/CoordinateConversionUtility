using CoordinateConversionUtility.Helpers;

using System;
using System.Text;

namespace CoordinateConversionUtility
{
    /// <summary>
    /// Static Helper class.
    /// Responsible for providing basic validation and state information for Coordinate objects and portions thereof.
    /// </summary>
    public static class ConversionHelper
    {
#pragma warning disable CA1304 // Specify CultureInfo
#pragma warning disable CA1305 // Specify IFormatProvider
        private static decimal LonMinsRound { get; } = 2.5m;         // used in gridsquare rounding calculations to find CENTER of 5th gridsquare character
        private static decimal LatMinsRound { get; } = 1.25m;        // used in gridsquare rounding calculations to find CENTER of 6th gridsquare character
        public static short ExtractPolarity(decimal number)
        {
            if (number > 0)
            {
                return 1;
            }

            if (number < 0)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Returns integer representing Decimal direction: 1 for N; -1 for S; 0 for bad input.
        /// Apply to absolute Longitude to transform from unsigned to signed value.
        /// </summary>
        /// <param name="ddmLattitude"></param>
        /// <returns></returns>
        public static short ExtractPolarityNS(string ddmLattitude)
        {
            if (string.IsNullOrEmpty(ddmLattitude) != true)
            {

                if (ddmLattitude.IndexOf('S') > -1)
                {
                    return -1;
                }

                if (ddmLattitude.IndexOf('N') > -1)
                {
                    return 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// Returns integer representing Decimal direction: 1 for E; -1 for W; 0 for bad input.
        /// Apply to Absolute Lattitude to transform from unsigned to signed value.
        /// </summary>
        /// <param name="ddmLongitude"></param>
        /// <returns></returns>
        public static short ExtractPolarityEW(string ddmLongitude)
        {
            if (string.IsNullOrEmpty(ddmLongitude) != true)
            {

                if (ddmLongitude.IndexOf('W') > -1)
                {
                    return -1;
                }

                if (ddmLongitude.IndexOf('E') > -1)
                {
                    return 1;
                }
            }

            return 0;
        }

        public static string GetNSEW(string strDdmLatOrLon)
        {
            if (string.IsNullOrEmpty(strDdmLatOrLon))
            {
                return string.Empty;
            }

            StringBuilder nsew = new StringBuilder(2);

            if (strDdmLatOrLon.IndexOf('N') > -1)
            {
                nsew.Append("N");
            }
            else if (strDdmLatOrLon.IndexOf('S') > -1)
            {
                nsew.Append("S");
            }

            if (strDdmLatOrLon.IndexOf('E') > -1)
            {
                nsew.Append("E");
            }
            else if (strDdmLatOrLon.IndexOf('W') > -1)
            {
                nsew.Append("W");
            }

            return nsew.ToString();
        }

        /// <summary>
        /// Returns string N, S, E, or W. Use LatOrLon=1 for Lattitudes; LatOrLon=2 for Longitudes.
        /// </summary>
        /// <param name="degreesLatOrLon"></param>
        /// <param name="LatOrLon"></param>
        /// <returns></returns>
        public static string GetNSEW(decimal degreesLatOrLon, int LatOrLon)
        {   //  lat = 1; lon = 2
            switch (LatOrLon)
            {
                case 1:
                    {
                        string NS = "N";
                        if (degreesLatOrLon < 0)
                        {
                            NS = "S";
                        }
                        return NS;
                    }
                case 2:
                    {
                        string EW = "E";
                        if (degreesLatOrLon < 0)
                        {
                            EW = "W";
                        }
                        return EW;
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        public static bool IsValid(decimal lattitude, decimal longitude)
        {

            if (!LatDecimalIsValid(lattitude))
            {
                return false;
            }

            if (!LonDecimalIsValid(longitude))
            {
                return false;
            }

            return true;
        }

        private static bool LatDecimalIsValid(decimal lattitudeDecimal)
        {
            if (-90 <= lattitudeDecimal && lattitudeDecimal <= 90)
            {
                return true;
            }

            return false;
        }

        private static bool LonDecimalIsValid(decimal longitudeDecimal)
        {
            if (-180 <= longitudeDecimal && longitudeDecimal <= 180)
            {
                return true;
            }

            return false;
        }

        public static decimal GetLatDegrees(LookupTablesHelper lookupTablesHelper, string Gridsquare, out short LatDirection)
        {   // Table4 Lookup primary Degrees Lattitude from a GridSquare character
            LatDirection = 0;

            if (lookupTablesHelper == null || string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare))
            {
                return -90m;
            }

            var currentGridsquare = Gridsquare[1].ToString();
            //int LatDirection = 0;

            if (lookupTablesHelper.GetTable4G2CLookup.TryGetValue(currentGridsquare.ToUpper(), out int latDegreesLookupResult))
            {
                if (currentGridsquare.ToUpper() == "I" || latDegreesLookupResult < 0)
                {
                    LatDirection = -1;
                }
                if (currentGridsquare.ToUpper() == "J" || latDegreesLookupResult > 0)
                {
                    LatDirection = 1;
                }
            }
            else
            {
                //throw new InvalidOperationException($"Gridsquare member {Gridsquare[0]} (index zero of {Gridsquare}) or LonDegrees {DDMlatDegrees} did not match the program logic.");
                LatDirection = 0;
            }

            decimal DDMlatDegrees = Math.Abs(latDegreesLookupResult) * LatDirection;
            return DDMlatDegrees;
        }

        public static decimal AddLatDegreesRemainder(decimal DDMLatDegress, int LatDirection, string Gridsquare)
        {   // Table5 is calculated and will add to Degrees Lattitude, MUST BE ZERO_BIASED
            if (string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare))
            {
                return 0.0m;
            }

            int lat_MinsAdjustment = 0; // -9;
            decimal result = 0m;

            if (LatDirection == 1)
            {
                lat_MinsAdjustment = 0;
            }
            else if (LatDirection == -1)
            {
                lat_MinsAdjustment = -9;
            }

            ;   //  buggy result: 3 returns 60 instead of 3
            //result = DDMLatDegress + (int.Parse(Gridsquare[3].ToString() + lat_MinsAdjustment) * LatDirection);
            var fourthGridChar = Gridsquare[3].ToString();
            if( decimal.TryParse(fourthGridChar, out decimal intFourthGridChar))
                {
                result += (intFourthGridChar + DDMLatDegress + lat_MinsAdjustment);
            }
            else
            {
                result = 0.0m;
            }
            //result = DDMLatDegress + intFourthGridChar

            return result;
        }

        public static decimal GetLatMinutes(LookupTablesHelper lookupTablesHelper, decimal DDMLatDegrees, int LatDirection, string Gridsquare, out decimal adjustedLatDegrees)
        {   // Table6 Lookup Lattitude Minutes including Round and increment/decrement Lat Degrees with carry-over
            //adjustedDDMlatDegrees = 0.0m;
            adjustedLatDegrees = 0.0m;

            if (lookupTablesHelper == null || string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare) || LatDirection == 0)
            {
                return 0.0m;
            }

            var testVariable = 0.0m;
            decimal latMinsLookupResult = 0m;

            if (lookupTablesHelper.GetTable6G2CLookup.TryGetValue(Gridsquare[5].ToString().ToUpper(), out latMinsLookupResult))
            {
                testVariable = latMinsLookupResult;

                if (LatDirection > 0)
                {   // the positive side (0 thru 60) of the Table but ZERO_BIASED so add 2.5 less than 60
                    //  latMinsLookupResult += (57.5m + LatMinsRound);
                    testVariable += 57.5m;

                    if (testVariable > 60)
                    {
                        adjustedLatDegrees ++;
                        testVariable -= 60;
                    }

                    adjustedLatDegrees += DDMLatDegrees;
                    //latMinsLookupResult = testVariable;
                }
                
                if (LatDirection < 0)
                {   // the negative side (-60 thru 0) of the Table
                    //latMinsLookupResult += (LatMinsRound * LatDirection);
                    if (testVariable < -60)
                    {
                        adjustedLatDegrees --;
                        testVariable += 60;
                    }

                    adjustedLatDegrees -= Math.Abs(DDMLatDegrees);
                    //latMinsLookupResult = testVariable;
                }

                //while (Math.Abs(latMinsLookupResult) >= 60)
                //{
                //    //adjustedDDMlatDegrees = DDMlatDegrees + (1 * LatDirection);
                //    latMinsLookupResult -= 60;
                //}
            }
            else
            {
                //throw new KeyNotFoundException($"{Gridsquare[5]} of {Gridsquare} not found in Table6");
                return 0m;
            }

            //decimal DDMlatMinutes = Math.Abs(latMinsLookupResult) + LatMinsRound;
            decimal DDMlatMinutes = Math.Abs(testVariable) + LatMinsRound;
            return DDMlatMinutes;
        }

        public static decimal GetLonDegrees(LookupTablesHelper lookupTablesHelper, string Gridsquare, out short LonDirection)
        {   // the 1st portion of Degrees Longitude IS the successfull lookup of first_lonChar in Table1
            LonDirection = 0;

            if (lookupTablesHelper == null || string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare))
            {
                return 0m;
            }

            //int LonDirection = 0;

            if (lookupTablesHelper.GetTable1G2CLookup.TryGetValue(Gridsquare[0].ToString().ToUpper(), out int lonDegreesLookupResult))
            {
                //  DDMlonDegrees will be set to LonDirection MUST also be set
                if (Gridsquare[0].ToString().ToUpper() == "I" || lonDegreesLookupResult < 0)
                {   // if Gridsquare is I result should be between -20 and 0
                    LonDirection = -1;
                }
                else if (Gridsquare[0].ToString().ToUpper() == "J" || lonDegreesLookupResult >= 0)
                {   // if Gridsquare is J result should be between 0 and 20
                    LonDirection = 1;
                }
                else
                {
                    //throw new InvalidOperationException($"Gridsquare member {Gridsquare[0]} (index zero of {Gridsquare}) or LonDegrees {lonDegreesLookupResult} did not match the program logic.");
                    return 0m;
                }
            }
            else
            {
                //throw new KeyNotFoundException($"{Gridsquare[0]} of {Gridsquare} not found in Table1!");
                return 0m;
            }

            decimal DDMlonDegrees = Math.Abs(lonDegreesLookupResult) * LonDirection;
            return DDMlonDegrees;
        }

        public static decimal AddLonDegreesRemainder(decimal DDMlonDegrees, int LonDirection, string Gridsquare)
        {   // Table2 lookup is calculated, then added to Degrees Longitude, MUST BE ZERO_BIASED
            //  LonDirection must be checked (should have been set PRIOR to reaching this method)
            if (string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare) || LonDirection == 0)
            {
                return 0.0m;
            }

            var testResult = 0.0m;

            if (LonDirection > 0)
            {   //  calc gridchar Number to Positive side of table is (num * 2)
                //lon_MinsAdjustment = 0;
                //DDMlonDegrees += int.Parse(Gridsquare[2].ToString()) * 2;
                testResult = DDMlonDegrees + (int.Parse(Gridsquare[2].ToString()) * 2);
            }
            
            if (LonDirection < 0)
            {
                //  calc gridchar Number to Negative side of table: (num * 2) - 18 
                int lon_MinsAdjustment = -18;
                //DDMlonDegrees += Math.Abs(lon_MinsAdjustment + int.Parse(Gridsquare[2].ToString()) * 2);
                var gridChar = int.Parse(Gridsquare[2].ToString());
                testResult = lon_MinsAdjustment + (gridChar * 2);
            }

            //return DDMlonDegrees;
            return testResult + DDMlonDegrees;
        }

        public static decimal GetLonMinutes(LookupTablesHelper lookupTablesHelper, decimal DDMlonDegrees, int LonDirection, string Gridsquare, out decimal adjustedDDMlonDegrees)
        {   // Table3 is Minutes Longitude Lookup table will also Round-up/down and in/decrement Lon Degrees with carry-over
            adjustedDDMlonDegrees = 0m;
            if (lookupTablesHelper == null || string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare) || LonDirection == 0)
            {
                return 0.0m;
            }

            //int lonMinsReducer;
            int lonMinsReducer = 60;
            var testResult = 0.0m;

            if (lookupTablesHelper.GetTable3G2CLookup.TryGetValue(Gridsquare[4].ToString().ToUpper(), out decimal lonMinsLookupResult))
            {

                if (LonDirection > 0)
                {
                    if (lonMinsLookupResult < 0)
                    {
                        lonMinsLookupResult += 115;
                    }

                    if (lonMinsLookupResult > 60)
                    {
                        lonMinsLookupResult -= 60;
                        adjustedDDMlonDegrees++;
                    }

                    testResult = lonMinsLookupResult;
                    //lonMinsLookupResult += 115 + LonMinsRound;
                    //lonMinsReducer = 60;
                }
                //else
                if (LonDirection < 0)
                {
                    if (lonMinsLookupResult < -60)
                    {
                        lonMinsLookupResult += 60;
                        adjustedDDMlonDegrees--;
                    }

                    testResult = lonMinsLookupResult;
                    //lonMinsReducer = -60;
                }

                adjustedDDMlonDegrees += DDMlonDegrees;
                //while (Math.Abs(lonMinsLookupResult) >= 60) //   120)
                //{
                //    //adjustedDDMlonDegrees = DDMlonDegrees + 1;
                //    //lonMinsLookupResult -= lonMinsReducer;  //   120;
                //}
            }
            else
            {
                //throw new KeyNotFoundException($"{Gridsquare[4].ToString(currentCulture)} of {Gridsquare} not found in Table3!");
                return 0m;
            }

            //decimal DDMlonMinutes = Math.Abs(lonMinsLookupResult);
            //return DDMlonMinutes;
            testResult = Math.Abs(testResult) + LonMinsRound;

            return testResult;
        }

        /// <summary>
        /// Returns decimal value that maps to nearest even multiple of Lat (1) or Lon (2) coordinates, or 0.0m if solution not found.
        /// </summary>
        /// <param name="minutesInput"></param>
        /// <param name="latOrLon"></param>
        /// <returns></returns>
        public static decimal GetNearestEvenMultiple(decimal minutesInput, int latOrLon)
        {
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
            //decimal HighEndMultiple = LowEndMultiple + interval;
            //decimal LowEndDifference = Math.Abs(minutesInput - LowEndMultiple);
            //decimal HighEndDifference = Math.Abs(minutesInput - HighEndMultiple);

            //if (LowEndDifference < HighEndDifference)
            //{
                if (minutesInput < 0)
                {
                    return LowEndMultiple;
                }

                if (minutesInput > 0)
                {
                //return HighEndMultiple;
                    return LowEndMultiple + interval;
                }
            //}
            //else if (LowEndDifference > HighEndDifference)
            //{
            //    if (minutesInput > 0)
            //    {
            //        return HighEndMultiple;
            //    }

            //    if (minutesInput < 0)
            //    {
            //        return LowEndMultiple;
            //    }
            //}
            //else // LowEndDifference == HighEndDifference
            //{
            //    if (minutesInput > 0)
            //    {
            //        return HighEndMultiple;
            //    }

            //    if (minutesInput < 0)
            //    {
            //        return LowEndMultiple;
            //    }
            //}

            return 0.0m;    //  a solution was not found
        }

    }
}
