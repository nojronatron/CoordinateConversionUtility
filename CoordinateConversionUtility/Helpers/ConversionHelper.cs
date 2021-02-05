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

        public static decimal GetLatDegrees(LookupTablesHelper lookupTablesHelper, string Gridsquare)
        {   // Table4 Lookup primary Degrees Lattitude from a GridSquare character
            if (lookupTablesHelper == null)
            {
                return -90m;
            }

            int LatDirection = 0;

            if (lookupTablesHelper.GetTable4G2CLookup.TryGetValue(Gridsquare[1].ToString().ToUpper(), out int latDegreesLookupResult))
            {
                if (Gridsquare[0].ToString().ToUpper() == "I" || latDegreesLookupResult < 0)
                {
                    LatDirection = -1;
                }
                if (Gridsquare[0].ToString().ToUpper() == "J" || latDegreesLookupResult > 0)
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

            result = DDMLatDegress + (int.Parse(Gridsquare[3].ToString() + lat_MinsAdjustment) * LatDirection);
            return result;
        }

        public static decimal GetLatMinutes(LookupTablesHelper lookupTablesHelper, int LatDirection, decimal DDMlatDegrees, string Gridsquare, out decimal adjustedDDMlatDegrees)
        {   // Table6 Lookup Lattitude Minutes including Round and increment/decrement Lat Degrees with carry-over
            // NOTE Dependency on LatDirection
            decimal latMinsLookupResult = 0m;
            adjustedDDMlatDegrees = 0m;

            if (lookupTablesHelper == null)
            {
                return 0m;
            }

            if (lookupTablesHelper.GetTable6G2CLookup.TryGetValue(Gridsquare[5].ToString().ToUpper(), out latMinsLookupResult))
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
                    adjustedDDMlatDegrees = DDMlatDegrees + (1 * LatDirection);
                    latMinsLookupResult -= 60;
                }
            }
            else
            {
                //throw new KeyNotFoundException($"{Gridsquare[5]} of {Gridsquare} not found in Table6");
                return 0m;
            }

            decimal DDMlatMinutes = Math.Abs(latMinsLookupResult);
            return DDMlatMinutes;
        }

        public static decimal GetLonDegrees(LookupTablesHelper lookupTablesHelper, string Gridsquare)
        {   // the 1st portion of Degrees Longitude IS the successfull lookup of first_lonChar in Table1
            if (lookupTablesHelper == null)
            {
                return 0m;
            }

            int LonDirection = 0;

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

            decimal DDMlonDegrees = Math.Abs(lonDegreesLookupResult);
            return DDMlonDegrees;
        }

        public static decimal AddLonDegreesRemainder(decimal DDMlonDegrees, int LonDirection, string Gridsquare)
        {   // Table2 lookup is calculated, then added to Degrees Longitude, MUST BE ZERO_BIASED
            //  LonDirection must be checked (should have been set PRIOR to reaching this method)
            if (LonDirection == 1)
            {   //  calc gridchar Number to Positive side of table is (num * 2)
                //lon_MinsAdjustment = 0;
                DDMlonDegrees += int.Parse(Gridsquare[2].ToString()) * 2;
            }
            else if (LonDirection == -1)
            {
                //  calc gridchar Number to Negative side of table: (num * 2) - 18 
                int lon_MinsAdjustment = -18;
                DDMlonDegrees += Math.Abs(lon_MinsAdjustment + int.Parse(Gridsquare[2].ToString()) * 2);
            }

            return DDMlonDegrees;
        }

        public static decimal GetLonMinutes(LookupTablesHelper lookupTablesHelper, int LonDirection, decimal DDMlonDegrees, string Gridsquare, out decimal adjustedDDMlonDegrees)
        {   // Table3 is Minutes Longitude Lookup table will also Round-up/down and in/decrement Lon Degrees with carry-over
            adjustedDDMlonDegrees = 0m;

            if (lookupTablesHelper == null)
            {
                return 0m;
            }
            
            int lonMinsReducer;

            if (lookupTablesHelper.GetTable3G2CLookup.TryGetValue(Gridsquare[4].ToString().ToUpper(), out decimal lonMinsLookupResult))
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

                while (Math.Abs(lonMinsLookupResult) >= 60) //   120)
                {
                    adjustedDDMlonDegrees = DDMlonDegrees + 1;
                    lonMinsLookupResult -= lonMinsReducer;  //   120;
                }
            }
            else
            {
                //throw new KeyNotFoundException($"{Gridsquare[4].ToString(currentCulture)} of {Gridsquare} not found in Table3!");
                return 0m;
            }

            decimal DDMlonMinutes = Math.Abs(lonMinsLookupResult);
            return DDMlonMinutes;
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
                    return HighEndMultiple;
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

    }
}
