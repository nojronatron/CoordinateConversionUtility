using System.Collections.Generic;

namespace CoordinateConversionUtility.Helpers
{
    /// <summary>
    /// Responsible for generating and managing the required lookup tables for GridSquare conversions.
    /// Provides methods to access the necessary table(s) to callers.
    /// </summary>
    public class LookupTablesHelper
    {
        private readonly List<string> alphabet = new List<string>(24)
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
            "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
        };

        // Lookup Tables for Grid->Coordinate calculations
        private Dictionary<string, int> Table1G2CLookup;
        private Dictionary<string, decimal> Table3G2CLookup;
        private Dictionary<string, int> Table4G2CLookup;
        private Dictionary<string, decimal> Table6G2CLookup;
        // Lookup Tables for Coordinate->Grid calculations
        private Dictionary<decimal, string> Table1C2GLookupPositive;
        private Dictionary<decimal, string> Table1C2GLookupNegative;
        private Dictionary<int, int> Table2C2GLookupPositive;
        private Dictionary<int, int> Table2C2GLookupNegative;
        private Dictionary<decimal, string> Table3C2GLookup;
        private Dictionary<decimal, string> Table4C2GLookupPositive;
        private Dictionary<decimal, string> Table4C2GLookupNegative;
        private Dictionary<decimal, string> Table6C2GLookup;

        public Dictionary<string, int> GetTable1G2CLookup => Table1G2CLookup;
        public Dictionary<string, decimal> GetTable3G2CLookup => Table3G2CLookup;
        public Dictionary<string, int> GetTable4G2CLookup => Table4G2CLookup;
        public Dictionary<string, decimal> GetTable6G2CLookup => Table6G2CLookup;

        public Dictionary<decimal, string> GetTable1C2GLookupPositive => Table1C2GLookupPositive;
        public Dictionary<decimal, string> GetTable1C2GLookupNegative => Table1C2GLookupNegative;
        public Dictionary<int, int> GetTable2C2GLookupPositive => Table2C2GLookupPositive;
        public Dictionary<int, int> GetTable2C2GLookupNegative => Table2C2GLookupNegative;
        public Dictionary<decimal, string> GetTable3C2GLookup => Table3C2GLookup;
        public Dictionary<decimal, string> GetTable4C2GLookupPositive => Table4C2GLookupPositive;
        public Dictionary<decimal, string> GetTable4C2GLookupNegative => Table4C2GLookupNegative;
        public Dictionary<decimal, string> GetTable6C2GLookup => Table6C2GLookup;

        public LookupTablesHelper()
        {
        }

        /// <summary>
        /// Creates lookup tables required for make conversions between GridSquare and DDM Coordinates and back.
        /// Returns True if all tables created, else returns False.
        /// </summary>
        /// <returns></returns>
        public bool GenerateTableLookups()
        {
            int tracker = 0;
            decimal minsLongitude = -115m;
            decimal minsLattitude = -57.5m;

            Table3G2CLookup = new Dictionary<string, decimal>(24);
            Table3C2GLookup = new Dictionary<decimal, string>(24);
            Table6G2CLookup = new Dictionary<string, decimal>(24);
            Table6C2GLookup = new Dictionary<decimal, string>(24);

            while (tracker < 24)
            {
                string letter = alphabet[tracker];
                Table3G2CLookup.Add(letter, minsLongitude);
                Table3C2GLookup.Add(minsLongitude, letter);
                minsLongitude += 5m;
                Table6G2CLookup.Add(letter, minsLattitude);
                Table6C2GLookup.Add(minsLattitude, letter);
                minsLattitude += 2.5m;
                tracker++;
            }

            tracker = 0;
            int degreesLongitude = -160;
            int degreesLattitude = -80;

            Table1C2GLookupPositive = new Dictionary<decimal, string>(10);
            Table4C2GLookupPositive = new Dictionary<decimal, string>(9);
            Table1C2GLookupNegative = new Dictionary<decimal, string>(10);
            Table4C2GLookupNegative = new Dictionary<decimal, string>(9);
            Table1G2CLookup = new Dictionary<string, int>(18);
            Table4G2CLookup = new Dictionary<string, int>(18);

            while (tracker < 18)
            {
                string letter = alphabet[tracker];

                if (letter == "J")
                {
                    degreesLongitude -= 20;
                    degreesLattitude -= 10;
                    Table1C2GLookupPositive.Add(degreesLongitude, letter);
                    Table4C2GLookupPositive.Add(degreesLattitude, letter);
                }

                Table1G2CLookup.Add(letter, degreesLongitude);

                if (letter == "I")
                {
                    Table1C2GLookupNegative.Add(degreesLongitude, letter);
                    Table4C2GLookupNegative.Add(degreesLattitude, letter);
                }

                if (degreesLongitude < 0)
                {
                    Table1C2GLookupNegative.Add(degreesLongitude, letter);
                    Table4C2GLookupNegative.Add(degreesLattitude, letter);
                }

                if (degreesLongitude > 0)
                {
                    Table1C2GLookupPositive.Add(degreesLongitude, letter);
                    Table4C2GLookupPositive.Add(degreesLattitude, letter);
                }

                Table4G2CLookup.Add(letter, degreesLattitude);

                degreesLongitude += 20;
                degreesLattitude += 10;
                tracker++;
            }

            tracker = 0;
            int degreesNegativeLongitude = -18;
            degreesLongitude = 2;
            int degreesNegativeLattitude = -9;
            degreesLattitude = 1;

            Table2C2GLookupPositive = new Dictionary<int, int>(9);
            Table2C2GLookupNegative = new Dictionary<int, int>(9);

            while (tracker < 10)
            {
                Table2C2GLookupPositive.Add(degreesLongitude, tracker);
                Table2C2GLookupNegative.Add(degreesNegativeLongitude, tracker);
                degreesLongitude += 2;
                degreesNegativeLongitude += 2;
                degreesLattitude++;
                degreesNegativeLattitude++;
                tracker++;
            }

            return true;
        }

    }
}
