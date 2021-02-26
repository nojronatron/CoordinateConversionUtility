using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CoordinateConversionUtility.Helpers
{
    /// <summary>
    /// List of proposed functions: 
    ///     Validate a string formatted as a GridSquare
    ///     Lookup Gridsquare characters.
    /// </summary>
    public class GridSquareHelper
    {
        private readonly CultureInfo currentCulture = CultureInfo.CurrentCulture;
        private LookupTablesHelper LookupTablesHelper { get; set; }

        public GridSquareHelper()
        {
            LookupTablesHelper = new LookupTablesHelper();
            LookupTablesHelper.GenerateTableLookups();
        }

        /// <summary>
        /// Takes a string input and tests the format as a Gridsquare (AA11AA). Returns true and  out string the valid-format Gridsquare if valid.
        /// If input is not validated, returns false and a question mark.
        /// </summary>
        /// <param name="gridsquare"></param>
        /// <param name="validGridsquare"></param>
        /// <returns></returns>
        public bool ValidateGridsquareInput(string gridsquare, out string validGridsquare)
        {
            validGridsquare = "?";

            if (string.IsNullOrEmpty(gridsquare))
            {
                return false;
            }

            string tempGridsquare = gridsquare.ToUpper(currentCulture).Trim();
            Regex rx = new Regex(@"[A-Z]{2}[0-9]{2}[A-Z]{2}");
            MatchCollection matches = rx.Matches(tempGridsquare);

            if (rx.IsMatch(tempGridsquare))
            {
                validGridsquare = matches[0].Value.ToString(currentCulture);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Takes direction of lattitude and the nearest even multiple (per lookup tables) and returns the resulting gridsquare character as a string.
        /// If no directional is supplied then a question mark is returned.
        /// </summary>
        /// <param name="LatDirection"></param>
        /// <param name="nearestEvenMultiple"></param>
        /// <returns></returns>
        internal string GetSixthGridsquareCharacter(short LatDirection, decimal nearestEvenMultiple)
        {
            if (LatDirection > 0)
            {
                decimal latMinsLookupValue = -60m + nearestEvenMultiple;

                if (LookupTablesHelper.GetTable6C2GLookup.TryGetValue(latMinsLookupValue, out string table6LookupResult))
                {
                    return table6LookupResult.ToLower();
                }
            }

            if (LatDirection < 0)
            {
                decimal latMinsLookupValue = nearestEvenMultiple;

                if (LookupTablesHelper.GetTable6C2GLookup.TryGetValue(latMinsLookupValue, out string table6LookupResult))
                {
                    return table6LookupResult.ToLower();
                }
            }

            return "?";
        }

        /// <summary>
        /// Takes direction of longitude and the nearest even multiple (per lookup table) and returns the resulting gridsquare character as a string.
        /// If no directional is supplied then a question mark is returned.
        /// </summary>
        /// <param name="LonDirection"></param>
        /// <param name="nearestEvenMultiple"></param>
        /// <returns></returns>
        internal string GetFifthGridsquareCharacter(short LonDirection, decimal nearestEvenMultiple)
        {
            if (LonDirection != 0 || nearestEvenMultiple != 0)
            {
                if (LonDirection > 0)
                {
                    decimal lonMinutesLookupValue = 0 - 120 + nearestEvenMultiple;

                    if (LookupTablesHelper.GetTable3C2GLookup.TryGetValue(lonMinutesLookupValue, out string table3LookupResult))
                    {
                        return table3LookupResult.ToLower();
                    }
                }

                if (LonDirection < 0)
                {
                    decimal lonMinutesLookupValue = nearestEvenMultiple;

                    if (LookupTablesHelper.GetTable3C2GLookup.TryGetValue(lonMinutesLookupValue, out string table3LookupResult))
                    {
                        return table3LookupResult.ToLower();
                    }
                }
            }

            return "?";
        }

        /// <summary>
        /// Takes remaining lattitude minutes and returns a string character representing the fourth gridsquare character.
        /// </summary>
        /// <param name="RemainderLat"></param>
        /// <param name="LatDirection"></param>
        /// <param name="minsRemainderLat"></param>
        /// <returns></returns>
        internal string GetFourthGridsquareCharacter(decimal RemainderLat, int LatDirection)//, out decimal minsRemainderLat)
        {
            //minsRemainderLat = 0;

            if (LatDirection != 0)
            {

                if (LatDirection < 0)
                {
                    return $"{ RemainderLat + 9 }";
                }

                if (LatDirection > 0)
                {
                    return $"{ RemainderLat }";
                }
            }

            return "?";
        }

        /// <summary>
        /// Takes remaining longitude minutes and returns a string character representing the third gridsquare character.
        /// Will output any minutes longitude remaining after gridsquare calculation.
        /// </summary>
        /// <param name="RemainderLon"></param>
        /// <param name="LonDirection"></param>
        /// <param name="minsRemainderLon"></param>
        /// <returns></returns>
        internal string GetThirdGridsquareCharacter(decimal RemainderLon, int LonDirection, out decimal minsRemainderLon)
        {
            decimal calculationNumber = 0.0m;

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

            if (RemainderLon % 2 != 0)  //  third character lookup is in 2-degree increments so an odd-number will result in a remainder of 1 degree or 60 minutes
            {
                minsRemainderLon = 60m;
            }
            else
            {
                minsRemainderLon = 0m;   //  decrement remainder
            }

            return $"{ Math.Abs(Math.Truncate(calculationNumber)) }";
        }

        /// <summary>
        /// Takes lattitude degrees whole-number and directional indicator. Does a lookup of degrees lattitude and returns the character representing the second gridsquare character.
        /// Also outputs decimal representing remaining degrees lattitude. Invalid directional input or failed lookup causes a question mark return string without any processing.
        /// </summary>
        /// <param name="DDMlatDegrees"></param>
        /// <param name="LatDirection"></param>
        /// <param name="RemainderLat"></param>
        /// <returns></returns>
        internal string GetSecondGridsquareCharacter(decimal DDMlatDegrees, int LatDirection, out decimal RemainderLat)
        {
            RemainderLat = 0.0m;

            if (LatDirection == 0)
            {
                return "?";
            }

            decimal latDegreesLookupValue = DDMlatDegrees;
            RemainderLat = latDegreesLookupValue % 10;
            latDegreesLookupValue -= RemainderLat;

            if (LatDirection > 0)
            {

                if (LookupTablesHelper.GetTable4C2GLookupPositive.TryGetValue(latDegreesLookupValue, out string table4LookupResult))
                {
                    return $"{ table4LookupResult }";
                }
            }

            if (LatDirection < 0)
            {

                if (LookupTablesHelper.GetTable4C2GLookupNegative.TryGetValue(Math.Abs(latDegreesLookupValue) * LatDirection, out string table4LookupResult))
                {
                    return $"{ table4LookupResult }";
                }
            }

            return $"?";
        }

        /// <summary>
        /// Takes longitude degrees whole-number and directional indicator. Does a lookup of degrees longitude and returns the character representing the first gridsquare character.
        /// Also outputs decimal representing remaining degrees longitude. Invalid directional input or failed lookup causes a question mark return string without any processing.
        /// </summary>
        /// <param name="DDMlonDegrees"></param>
        /// <param name="LonDirection"></param>
        /// <param name="RemainderLon"></param>
        /// <returns></returns>
        internal string GetFirstGridsquareCharacter(decimal DDMlonDegrees, int LonDirection, out decimal RemainderLon)
        {
            if (LonDirection == 0)
            {
                RemainderLon = 0.0m;
                return "?";
            }

            decimal lonDegreesLookupValue;
            RemainderLon = DDMlonDegrees % 20;

            if (RemainderLon != 0)
            {
                lonDegreesLookupValue = LonDirection * (Math.Abs(DDMlonDegrees) - Math.Abs(RemainderLon));
            }
            else
            {
                lonDegreesLookupValue = DDMlonDegrees;
            }

            if (LonDirection < 0)
            {
                if (LookupTablesHelper.GetTable1C2GLookupNegative.TryGetValue(Math.Abs(lonDegreesLookupValue) * LonDirection, out string table1LookupResult))
                {
                    return table1LookupResult;
                }
            }
            
            if (LonDirection > 0)
            {
                if (LookupTablesHelper.GetTable1C2GLookupPositive.TryGetValue(lonDegreesLookupValue, out string table1LookupResult))
                {
                    return table1LookupResult;
                }
            }

            return "?";
        }

    }
}
