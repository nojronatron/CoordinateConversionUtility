using System;
using System.Collections.Generic;
using System.Globalization;
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

        public GridSquareHelper()
        {
        }

        /// <summary>
        /// Takes a string input and tests the format as a Gridsquare (AA11AA). Returns bool and out string the valid-format Gridsquare.
        /// </summary>
        /// <param name="gridsquare"></param>
        /// <param name="validGridsquare"></param>
        /// <returns></returns>
        public bool ValidateGridsquareInput(string gridsquare, out string validGridsquare)
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

    }
}
