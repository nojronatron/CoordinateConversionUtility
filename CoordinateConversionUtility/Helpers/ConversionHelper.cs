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
        { 
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

        public static bool IsValid(string gridsquare, out string validGridsquare)
        {
            var gsh = new GridSquareHelper();

            if (gsh.ValidateGridsquareInput(gridsquare, out string vGridsquare))
            {
                validGridsquare = vGridsquare;
                return true;
            };

            validGridsquare = string.Empty;
            return false;
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
        {   
            LatDirection = 0;

            if (lookupTablesHelper == null || string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare))
            {
                return -90m;
            }

            var currentGridsquare = Gridsquare[1].ToString();

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
                LatDirection = 0;
            }

            return Math.Abs(latDegreesLookupResult) * LatDirection;
        }

        public static decimal AddLatDegreesRemainder(decimal DDMLatDegress, int LatDirection, string Gridsquare)
        {   
            if (string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare))
            {
                return 0.0m;
            }

            int lat_MinsAdjustment = 0;
            decimal result = 0m;

            if (LatDirection == 1)
            {
                lat_MinsAdjustment = 0;
            }
            else if (LatDirection == -1)
            {
                lat_MinsAdjustment = -9;
            }

            var fourthGridChar = Gridsquare[3].ToString();

            if (decimal.TryParse(fourthGridChar, out decimal intFourthGridChar))
            {
                result += (intFourthGridChar + DDMLatDegress + lat_MinsAdjustment);
            }
            else
            {
                result = 0.0m;
            }

            return result;
        }

        public static decimal GetLatMinutes(LookupTablesHelper lookupTablesHelper, decimal DDMLatDegrees, int LatDirection, string Gridsquare, out decimal adjustedLatDegrees)
        {   
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
                {   
                    testVariable += 57.5m;

                    if (testVariable > 60)
                    {
                        adjustedLatDegrees ++;
                        testVariable -= 60;
                    }

                    adjustedLatDegrees += DDMLatDegrees;
                }
                
                if (LatDirection < 0)
                {   
                    if (testVariable < -60)
                    {
                        adjustedLatDegrees --;
                        testVariable += 60;
                    }

                    adjustedLatDegrees -= Math.Abs(DDMLatDegrees);
                }
            }
            else
            {
                return 0m;
            }

            return Math.Abs(testVariable) + LatMinsRound;
        }

        public static decimal GetLonDegrees(LookupTablesHelper lookupTablesHelper, string Gridsquare, out short LonDirection)
        {   
            LonDirection = 0;

            if (lookupTablesHelper == null || string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare))
            {
                return 0m;
            }

            if (lookupTablesHelper.GetTable1G2CLookup.TryGetValue(Gridsquare[0].ToString().ToUpper(), out int lonDegreesLookupResult))
            {
                if (Gridsquare[0].ToString().ToUpper() == "I" || lonDegreesLookupResult < 0)
                { 
                    LonDirection = -1;
                }
                else if (Gridsquare[0].ToString().ToUpper() == "J" || lonDegreesLookupResult >= 0)
                {   
                    LonDirection = 1;
                }
                else
                {
                    return 0m;
                }
            }
            else
            {
                return 0m;
            }

            return Math.Abs(lonDegreesLookupResult) * LonDirection;
        }

        public static decimal AddLonDegreesRemainder(decimal DDMlonDegrees, int LonDirection, string Gridsquare)
        {   
            if (string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare) || LonDirection == 0)
            {
                return 0.0m;
            }

            var testResult = 0.0m;

            if (LonDirection > 0)
            {   
                testResult = (int.Parse(Gridsquare[2].ToString()) * 2);
            }
            
            if (LonDirection < 0)
            {
                int lon_MinsAdjustment = -18;
                var gridChar = int.Parse(Gridsquare[2].ToString());
                testResult = lon_MinsAdjustment + (gridChar * 2);
            }

            return (testResult + DDMlonDegrees);
        }

        public static decimal GetLonMinutes(LookupTablesHelper lookupTablesHelper, decimal DDMlonDegrees, int LonDirection, string Gridsquare, out decimal adjustedDDMlonDegrees)
        {   
            adjustedDDMlonDegrees = 0m;
            if (lookupTablesHelper == null || string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare) || LonDirection == 0)
            {
                return 0.0m;
            }

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
                }

                if (LonDirection < 0)
                {
                    if (lonMinsLookupResult < -60)
                    {
                        lonMinsLookupResult += 60;
                        adjustedDDMlonDegrees--;
                    }

                    testResult = lonMinsLookupResult;
                }

                adjustedDDMlonDegrees += DDMlonDegrees;
            }
            else
            {
                return 0m;
            }

            return (Math.Abs(testResult) + LonMinsRound);
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
            {   
                interval = 2.5m;
            }
            else
            {   
                interval = 5.0m;
            }

            if (minutesInput % interval == 0)
            {   
                return minutesInput;
            }

            decimal LowEndMultiple = (Math.Truncate(minutesInput / interval)) * interval;

            if (minutesInput < 0)
            {
                return LowEndMultiple;
            }

            if (minutesInput > 0)
            {
                return LowEndMultiple + interval;
            }

            return 0.0m;
        }

    }
}
