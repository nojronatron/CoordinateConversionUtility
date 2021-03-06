using CoordinateConversionUtility.Helpers;
using CoordinateConversionUtility.Models;
using System;
using System.Globalization;
using System.Text;

namespace CoordinateConversionUtility
{
    /// <summary>
    /// Static Helper class.
    /// Responsible for providing basic validation and state information for Coordinate objects and portions thereof.
    /// </summary>
    public static class ConversionHelper
    {
        private static readonly CultureInfo currentCulture = CultureInfo.CurrentCulture;
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

        internal static bool ValidEvenMultipleLat(decimal nearestEvenMultiple)
        {
            return (nearestEvenMultiple >= -60.0m && nearestEvenMultiple <= 60.0m);
        }

        internal static bool ValidEvenMultipleLon(decimal nearestEvenMultiple)
        {
            return (nearestEvenMultiple >= -120.0m && nearestEvenMultiple <= 120.0m);
        }

        internal static bool ValidRemainderLat(decimal remainderLattitude)
        {
            return (remainderLattitude >= -10.0m && remainderLattitude <= 10.0m);
        }

        internal static bool ValidRemainderLon(decimal remainderLongitude)
        {
            return (remainderLongitude >= -20.0m && remainderLongitude <= 20.0m);
        }
        /// <summary>
        /// Takes a DDM-ish string of characters and returns N, S, E, or W based on the index of the letter found.
        /// </summary>
        /// <param name="strDdmLatOrLon"></param>
        /// <returns></returns>
        public static string GetNSEW(string strDdmLatOrLon)
        {
            if (string.IsNullOrEmpty(strDdmLatOrLon))
            {
                return string.Empty;
            }

            var nsew = new StringBuilder(2);

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

        public static bool IsValidLatDegreesAndLonDegrees(decimal lattitude, decimal longitude)
        {
            return CoordinateBase.ValidateLatDegrees(lattitude) && CoordinateBase.ValidateLonDegrees(longitude);
        }

        public static bool IsValidGridsquare(string gridsquare, out string validGridsquare)
        {
            var gridsquareHelper = new GridSquareHelper();

            if (gridsquareHelper.ValidateGridsquareInput(gridsquare, out string vGridsquare))
            {
                validGridsquare = vGridsquare;
                return true;
            };

            validGridsquare = string.Empty;
            return false;
        }

        public static bool LatDecimalIsValid(decimal lattitudeDecimal)
        {
            return CoordinateBase.ValidateLatDegrees(lattitudeDecimal);
        }

        public static bool LonDecimalIsValid(decimal longitudeDecimal)
        {
            return CoordinateBase.ValidateLonDegrees(longitudeDecimal);
        }

        /// <summary>
        /// Takes an initialized LookupTablesHelper instance and a valid Gridsquare and returns a signed Lattitude per the table lookup.
        /// LatDirection: If positive, then returned decimal is positive (North); if negative, then returned decimal is negative (South).
        /// </summary>
        /// <param name="lookupTablesHelper"></param>
        /// <param name="Gridsquare"></param>
        /// <param name="LatDirection"></param>
        /// <returns></returns>
        public static decimal GetLatDegrees(LookupTablesHelper lookupTablesHelper, string Gridsquare, out short LatDirection)
        {
            LatDirection = 0;

            if (lookupTablesHelper == null || string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare))
            {
                return -90m;
            }

            var currentGridsquare = Gridsquare[1].ToString(currentCulture);

            if (lookupTablesHelper.GetTable4G2CLookup.TryGetValue(currentGridsquare.ToUpper(currentCulture), out int latDegreesLookupResult))
            {
                if (currentGridsquare.ToUpper(currentCulture) == "I" || latDegreesLookupResult < 0)
                {
                    LatDirection = -1;
                }
                if (currentGridsquare.ToUpper(currentCulture) == "J" || latDegreesLookupResult > 0)
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

        /// <summary>
        /// Accepts DDM Lat Degree and associated gridsquare and returns Lat Degrees with added Remainder calculated from fourth gridsquare character.
        /// </summary>
        /// <param name="DDMLatDegress"></param>
        /// <param name="LatDirection"></param>
        /// <param name="Gridsquare"></param>
        /// <returns></returns>
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

            var fourthGridChar = Gridsquare[3].ToString(currentCulture);

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

        /// <summary>
        /// Takes gridsquare character 6 and uses LookupTableHelper to find and return minutes Latitude.
        /// If greater than one degree Latgitude, one degree is removed from minutes and one degree is added to DDMLatDegrees.
        /// DDMLatDegrees is rounded to the nearest 1.25 minutes Latitude and output via adjustedLatDegrees, as an UNSIGNED decimal.
        /// LatDirection is necessary to ensure correct sign is applied at decimal return.
        /// </summary>
        /// <param name="lookupTablesHelper"></param>
        /// <param name="DDMLatDegrees"></param>
        /// <param name="LatDirection"></param>
        /// <param name="Gridsquare"></param>
        /// <param name="adjustedLatDegrees"></param>
        /// <returns></returns>
        public static decimal GetLatMinutes(LookupTablesHelper lookupTablesHelper, decimal DDMLatDegrees, int LatDirection, string Gridsquare, out decimal adjustedLatDegrees)
        {
            adjustedLatDegrees = 0.0m;

            if (lookupTablesHelper == null || string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare) || LatDirection == 0)
            {
                return 0.0m;
            }

            if (lookupTablesHelper.GetTable6G2CLookup.TryGetValue(Gridsquare[5].ToString(currentCulture).ToUpper(currentCulture), out decimal latMinsLookupResult))
            {

                if (LatDirection > 0)
                {
                    latMinsLookupResult += 57.5m;

                    if (latMinsLookupResult > 60)
                    {
                        adjustedLatDegrees++;
                        latMinsLookupResult -= 60;
                    }

                    adjustedLatDegrees += DDMLatDegrees;
                }

                if (LatDirection < 0)
                {
                    if (latMinsLookupResult < -60)
                    {
                        adjustedLatDegrees--;
                        latMinsLookupResult += 60;
                    }

                    adjustedLatDegrees -= Math.Abs(DDMLatDegrees);
                }
            }
            else
            {
                return 0m;
            }

            return Math.Abs(latMinsLookupResult) + LatMinsRound;
        }

        /// <summary>
        /// Takes an initialized LookupTablesHelper instance and a valid Gridsquare and returns a signed Longitude per the table lookup.
        /// LonDirection: If positive, then returned decimal is positive (North); if negative, then returned decimal is negative (South).
        /// </summary>
        /// <param name="lookupTablesHelper"></param>
        /// <param name="Gridsquare"></param>
        /// <param name="LonDirection"></param>
        /// <returns></returns>
        public static decimal GetLonDegrees(LookupTablesHelper lookupTablesHelper, string Gridsquare, out short LonDirection)
        {
            LonDirection = 0;

            if (lookupTablesHelper == null || string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare))
            {
                return 0m;
            }

            if (lookupTablesHelper.GetTable1G2CLookup.TryGetValue(Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture), out int lonDegreesLookupResult))
            {
                if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "I" || lonDegreesLookupResult < 0)
                {
                    LonDirection = -1;
                }
                else if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "J" || lonDegreesLookupResult >= 0)
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

        /// <summary>
        /// Accepts DDM Lon Degree and associated gridsquare and returns Lon Degrees with added Remainder calculated from fifth gridsquare character.
        /// </summary>
        /// <param name="DDMLonDegress"></param>
        /// <param name="LonDirection"></param>
        /// <param name="Gridsquare"></param>
        /// <returns></returns>
        public static decimal AddLonDegreesRemainder(decimal DDMlonDegrees, int LonDirection, string Gridsquare)
        {
            if (string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare) || LonDirection == 0)
            {
                return 0.0m;
            }

            var testResult = 0.0m;

            if (LonDirection > 0)
            {
                testResult = int.Parse(Gridsquare[2].ToString(currentCulture), currentCulture) * 2;
            }

            if (LonDirection < 0)
            {
                int lon_MinsAdjustment = -18;
                var gridChar = int.Parse(Gridsquare[2].ToString(currentCulture), currentCulture);
                testResult = lon_MinsAdjustment + (gridChar * 2);
            }

            return (testResult + DDMlonDegrees);
        }

        /// <summary>
        /// Takes gridsquare character 5 and uses LookupTableHelper to find and return minutes Longitude.
        /// If greater than one degree Longitude, one degree is removed from minutes and one degree is added to DDMLonDegrees.
        /// DDMLonDegrees is rounded to the nearest 2.5 minutes Longitude and output via adjustedLonDegrees, as an UNSIGNED decimal.
        /// LonDirection is necessary to ensure correct sign is applied at decimal return.
        /// </summary>
        /// <param name="lookupTablesHelper"></param>
        /// <param name="DDMlonDegrees"></param>
        /// <param name="LonDirection"></param>
        /// <param name="Gridsquare"></param>
        /// <param name="adjustedDDMlonDegrees"></param>
        /// <returns></returns>
        public static decimal GetLonMinutes(LookupTablesHelper lookupTablesHelper, decimal DDMlonDegrees, int LonDirection, string Gridsquare, out decimal adjustedDDMlonDegrees)
        {
            adjustedDDMlonDegrees = 0m;
            if (lookupTablesHelper == null || string.IsNullOrEmpty(Gridsquare) || string.IsNullOrWhiteSpace(Gridsquare) || LonDirection == 0)
            {
                return 0.0m;
            }

            var testResult = 0.0m;

            if (lookupTablesHelper.GetTable3G2CLookup.TryGetValue(Gridsquare[4].ToString(currentCulture).ToUpper(currentCulture), out decimal lonMinsLookupResult))
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
        /// Biased toward the lower-end nearby number due to how the table lookups work (look UP in the next column for value).
        /// </summary>
        /// <param name="minutesInput"></param>
        /// <param name="latOrLon"></param>
        /// <returns></returns>
        public static decimal GetNearestEvenMultiple(decimal minutesInput, int latOrLon)
        {
            decimal interval;

            if (latOrLon == 1)
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
