using System;
using System.Collections.Generic;

namespace CoordinateConversionUtility.Models.Tests
{
    public class UnitTestsBase
    {
        protected static void DisplayOutput(string expectedResult, string actualResult, Dictionary<string, decimal> diffs)
        {
            Console.WriteLine($"Expected: { expectedResult }");
            Console.WriteLine($"Actual: { actualResult }");

            foreach (KeyValuePair<string, decimal> diff in diffs)
            {
                Console.WriteLine($"{diff.Key}: {diff.Value.ToString()}");
            }
        }

    }
}
