using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CoordinateConversionUtility.Helpers
{
    /// <summary>
    /// List of proposed functions: 
    ///     Validate a string formatted as a GridSquare
    ///     More TBD
    /// </summary>
    public class GridSquareHelper
    {
        private readonly CultureInfo currentCulture = CultureInfo.CurrentCulture;
        private LookupTablesHelper LookupTablesHelper { get; set; }

        public GridSquareHelper()
        {
            LookupTablesHelper = new LookupTablesHelper();
        }

        /// <summary>
        /// Takes a string input and tests the format as a Gridsquare (AA11AA). Returns bool and out string the valid-format Gridsquare.
        /// </summary>
        /// <param name="gridsquare"></param>
        /// <param name="validGridsquare"></param>
        /// <returns></returns>
        internal bool ValidateGridsquareInput(string gridsquare, out string validGridsquare)
        {   // validates gridsquare input, sets property Gridsquare with validated portion, return True if valid, False otherwise
            validGridsquare = "?";

            if (string.IsNullOrEmpty(gridsquare))
            {
                return false;
            }

            string tempGridsquare = gridsquare.ToUpper(currentCulture).Trim();
            Regex rx = new Regex(@"[A-Z]{2}[0-9]{2}[A-Z]{2}");
            MatchCollection matches = rx.Matches(tempGridsquare);

            if (rx.IsMatch(tempGridsquare))
            {   // Found a gridsquare pattern in {gridsquare}
                validGridsquare = matches[0].Value.ToString(currentCulture);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets 6th character of Gridsquare based on DDM_LatMins and Remainder_Lat
        /// </summary>
        internal string GetSixthGridsquareCharacter(decimal nearestEvenMultiple, decimal LatDirection)
        {
            var Gridsquare = new StringBuilder();
            decimal latMinsLookupValue = 0m;
            // check remainder and zero it out if in 2-degree increments otherwise...
            //   ...remove all but the remaining single-degree increment
            if (LatDirection > 0)
            {
                //latMinsLookupValue = -60m + DDMlatMinutes;
                latMinsLookupValue = nearestEvenMultiple;//GetNearestEvenMultiple(latMinsLookupValue, 1);

                if (LookupTablesHelper.GetTable6C2GLookup.TryGetValue(latMinsLookupValue, out string table6LookupResult))
                {
                    Gridsquare.Append(table6LookupResult.ToLower(currentCulture));
                }
            }
            else if (LatDirection < 0)
            {
                latMinsLookupValue = LatDirection * latMinsLookupValue;

                if (latMinsLookupValue % 2.5m != 0)
                {
                    latMinsLookupValue -= (latMinsLookupValue % 2.5m);
                }
                if (LookupTablesHelper.GetTable6C2GLookup.TryGetValue(latMinsLookupValue, out string table6LookupResult))
                {
                    Gridsquare.Append(table6LookupResult.ToLower(currentCulture));
                }
            }
            else
            {
                Gridsquare.Append("?");
                throw new ArgumentOutOfRangeException($"Unable to determine character in method { System.Reflection.MethodBase.GetCurrentMethod().Name }");
            }

            return Gridsquare.ToString();
        }

        /// <summary>
        /// Returns 5th Gridsquare character based on Remainder_Lon.
        /// </summary>
        internal string GetFifthGridsquareCharacter(decimal RemainderLon, int LonDirection, decimal nearestEvenMultiple)
        {
            decimal lonMinutesLookupValue;
            decimal remainderCorrectionValue = 0;
            var Gridsquare = new StringBuilder();

            if (RemainderLon > 1)
            {
                throw new ArgumentOutOfRangeException($"RemainderLon was > 1 in { System.Reflection.MethodBase.GetCurrentMethod().Name }");
            }
            else if (RemainderLon == 1)
            {
                remainderCorrectionValue = 60;    //  convert the remainder degrees to minutes
                RemainderLon = 0;               //  zero-out the remainder now it has been used
            }

            // use remainder from Table1Lookup and Table2Lookup, express it as Minutes and Add to actual Minutes
            if (LonDirection > 0)
            {
                //lonMinutesLookupValue = 0 - 120 + DDMlonMinutes + remainderCorrectionValue;
                lonMinutesLookupValue = nearestEvenMultiple;//GetNearestEvenMultiple(lonMinutesLookupValue, 2);

                if (LookupTablesHelper.GetTable3C2GLookup.TryGetValue(lonMinutesLookupValue, out string table3LookupResult))
                {
                    Gridsquare.Append(table3LookupResult.ToLower(currentCulture));
                }
            }
            else if (LonDirection < 0)
            {
                lonMinutesLookupValue = 0 - nearestEvenMultiple - remainderCorrectionValue;

                if (lonMinutesLookupValue % 5 != 0)
                {
                    lonMinutesLookupValue -= (lonMinutesLookupValue % 5);   //  move the pointer down the table to the next multiple of 5
                }
                if (LookupTablesHelper.GetTable3C2GLookup.TryGetValue((int)lonMinutesLookupValue, out string table3LookupResult))
                {
                    Gridsquare.Append(table3LookupResult.ToLower(currentCulture));
                }
            }
            else
            {
                Gridsquare.Append("?");
            }

            return Gridsquare.ToString();
        }

        /// <summary>
        /// Returns 4th GridSquare character based on Remainder_Lat and LatDirection.
        /// </summary>
        internal string GetFourthGridsquareCharacter(decimal RemainderLat, int LatDirection)
        {
            //  NOTE: Fourth Gridsquare character is arranged in single-digit increments therefore no remainder
            decimal latLookupValue; // = RemainderLat;
            var Gridsquare = new StringBuilder();

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
            Gridsquare.Append(latLookupValue.ToString(currentCulture));

            return Gridsquare.ToString();
        }

        /// <summary>
        /// Returns 3rd GridSquare character based on LonDirection and Remainder_Lon.
        /// </summary>
        internal string GetThirdGridsquareCharacter(decimal RemainderLon, int LonDirection)
        {
            decimal calculationNumber;
            var Gridsquare = new StringBuilder();

            if (LonDirection < 0)
            {

                if (RemainderLon % 2 != 0)
                {
                    calculationNumber = ((RemainderLon) + 21) / 2 - 1;
                    RemainderLon = 1;   // used up max even portion of RemainderLon so odd single is left and must be accounted for in last grid character
                }
                else
                {
                    calculationNumber = (18 + RemainderLon) / 2;
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

            decimal lonDegreesCalculatedResult = Math.Truncate(calculationNumber);
            Gridsquare.Append(Math.Abs(lonDegreesCalculatedResult).ToString(currentCulture));

            return Gridsquare.ToString();
        }

        /// <summary>
        /// Returns 2nd GridSquare character based on DDM_LatDegrees (whole number).
        /// </summary>
        internal string GetSecondGridsquareCharacter(decimal DDMlatDegrees, int LatDirection)
        {
            decimal latDegreesLookupValue = DDMlatDegrees;
            var Gridsquare = new StringBuilder();

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
                throw new IndexOutOfRangeException($"LatDirection ({ LatDirection }) could not be set for latDegreesLookupValue { latDegreesLookupValue }.");
            }

            decimal RemainderLat = latDegreesLookupValue % 10;

            if (RemainderLat != 0)
            {
                latDegreesLookupValue -= RemainderLat;
            }

            if (LatDirection > 0)
            {

                if (LookupTablesHelper.GetTable4C2GLookupPositive.TryGetValue(latDegreesLookupValue, out string table4LookupResult))
                {
                    Gridsquare.Append(table4LookupResult);
                }
            }
            else if (LatDirection < 0)
            {

                if (LookupTablesHelper.GetTable4C2GLookupNegative.TryGetValue(Math.Abs(latDegreesLookupValue) * LatDirection, out string table4LookupResult))
                {
                    Gridsquare.Append(table4LookupResult);
                }
                else
                {
                    Gridsquare.Append("?");
                }
            }
            else
            {
                Gridsquare.Append("?");
            }

            return Gridsquare.ToString();
        }

        /// <summary>
        /// Returns the 1st GridSquare character based on DDM_LonDegrees (whole number).
        /// </summary>
        internal string GetFirstGridsquareCharacter(decimal DDMlonDegrees, int LonDirection)
        {
            // LonDirection MUST be set prior to entering this method
            decimal lonDegreesLookupValue;
            if (LonDirection != -1 && LonDirection != 1)
            {
                throw new IndexOutOfRangeException($"LonDirection ({LonDirection}) could not be set for lonDegreesLookupValue.");
            }

            var Gridsquare = new StringBuilder();
            decimal RemainderLon = DDMlonDegrees % 20;

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
                if (LookupTablesHelper.GetTable1C2GLookupNegative.TryGetValue(Math.Abs(lonDegreesLookupValue) * LonDirection, out string table1LookupResult))
                {
                    Gridsquare.Append(table1LookupResult);
                }
            }
            else if (LonDirection > 0)
            {
                if (LookupTablesHelper.GetTable1C2GLookupPositive.TryGetValue(lonDegreesLookupValue, out string table1LookupResult))
                {
                    Gridsquare.Append(table1LookupResult);
                }
                else
                {
                    Gridsquare.Append("?");
                }
            }
            else
            {
                Gridsquare.Append("?");
            }

            return Gridsquare.ToString();
        }

    }
}
