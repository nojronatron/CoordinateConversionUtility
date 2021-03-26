using System;
using System.Collections.Generic;

namespace CoordinateConversionUtility.Models.Tests
{
    public class UnitTestsBase
    {
        internal static char CommaSymbol => (char)44;    //  comma symbol
        internal static char DegreesSymbol => (char)176; //  degree symbol
        internal static char MinutesSymbol => (char)39;      //  single quote
        internal static char SecondsSymbol => (char)34;      //  double quote
        internal static char SpaceCharacter => (char)32;    //  spacebar
        internal char[] trimChars = { CommaSymbol, DegreesSymbol, MinutesSymbol, SecondsSymbol, SpaceCharacter };
        internal static decimal DegreeAccuracyThreshold = 0.0012m;
        internal static decimal LatMinsAccuracyThreshold = 1.25m;
        internal static decimal LonMinsAccuracyThreshold = 2.50m;
        internal static decimal LatSecsAccuracyThreshold = 59.0m;
        internal static decimal LonSecsAccuracyThreshold = 59.0m;
        protected static void DisplayOutput(string expectedResult, string actualResult, Dictionary<string, decimal> diffs)
        {
            Console.WriteLine($"Expected: { expectedResult }");
            Console.WriteLine($"Actual: { actualResult }");

            foreach (KeyValuePair<string, decimal> diff in diffs)
            {
                Console.WriteLine($"{ diff.Key }: { diff.Value }");
            }
        }

    }
}
