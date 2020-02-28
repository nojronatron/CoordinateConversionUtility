using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoordinateConversionUtility.Helpers
{
    public static class GridSquareHelper
    {
        //  TODO: Write this class.
        //      MUST validate incoming data to be six characters following global Gridsquare extents
        //      MUST output valid string representations (but not necessarily correct)

        private static readonly CultureInfo currentCulture = CultureInfo.CurrentCulture;

        public static bool ValidateGridsquareInput(string gridsquare, out StringBuilder validgrid)
        {   // validates gridsquare input, sets property Gridsquare with validated portion, return True if valid, False otherwise
            if (string.IsNullOrEmpty(gridsquare))
            {
                validgrid = new StringBuilder(0);
                return false;
            }
            string tempGridsquare = gridsquare.ToUpper(currentCulture);  // MSFT recommends using ToUpper() esp for string comparisons
            Regex rx = new Regex(@"[A-R]{2}[0-9]{2}[A-X]{2}");
            MatchCollection matches = rx.Matches(tempGridsquare);
            validgrid = new StringBuilder(6);
            if (rx.IsMatch(tempGridsquare))
            {   // Found a gridsquare pattern in {gridsquare}
                validgrid.Append(matches[0].Value.ToString(currentCulture));
                return true;
            }
            validgrid = new StringBuilder(0);
            return false;
        }
    }
}
