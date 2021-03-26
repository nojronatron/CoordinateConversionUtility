using System.Collections.Generic;

namespace CoordinateConversionLibrary.Helpers
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

        public Dictionary<string, int> GetTable1G2CLookup { get; private set; }
        public Dictionary<string, decimal> GetTable3G2CLookup { get; private set; }
        public Dictionary<string, int> GetTable4G2CLookup { get; private set; }
        public Dictionary<string, decimal> GetTable6G2CLookup { get; private set; }

        public Dictionary<decimal, string> GetTable1C2GLookupPositive { get; private set; }
        public Dictionary<decimal, string> GetTable1C2GLookupNegative { get; private set; }
        public Dictionary<int, int> GetTable2C2GLookupPositive { get; private set; }
        public Dictionary<int, int> GetTable2C2GLookupNegative { get; private set; }
        public Dictionary<decimal, string> GetTable3C2GLookup { get; private set; }
        public Dictionary<decimal, string> GetTable4C2GLookupPositive { get; private set; }
        public Dictionary<decimal, string> GetTable4C2GLookupNegative { get; private set; }
        public Dictionary<decimal, string> GetTable6C2GLookup { get; private set; }

        public LookupTablesHelper() { }

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

            GetTable3G2CLookup = new Dictionary<string, decimal>(24);
            GetTable3C2GLookup = new Dictionary<decimal, string>(24);
            GetTable6G2CLookup = new Dictionary<string, decimal>(24);
            GetTable6C2GLookup = new Dictionary<decimal, string>(24);

            while (tracker < 24)
            {
                string letter = alphabet[tracker];
                GetTable3G2CLookup.Add(letter, minsLongitude);
                GetTable3C2GLookup.Add(minsLongitude, letter);
                minsLongitude += 5m;
                GetTable6G2CLookup.Add(letter, minsLattitude);
                GetTable6C2GLookup.Add(minsLattitude, letter);
                minsLattitude += 2.5m;
                tracker++;
            }

            tracker = 0;
            int degreesLongitude = -160;
            int degreesLattitude = -80;

            GetTable1C2GLookupPositive = new Dictionary<decimal, string>(10);
            GetTable4C2GLookupPositive = new Dictionary<decimal, string>(9);
            GetTable1C2GLookupNegative = new Dictionary<decimal, string>(10);
            GetTable4C2GLookupNegative = new Dictionary<decimal, string>(9);
            GetTable1G2CLookup = new Dictionary<string, int>(18);
            GetTable4G2CLookup = new Dictionary<string, int>(18);

            while (tracker < 18)
            {
                string letter = alphabet[tracker];

                if (letter == "J")
                {
                    degreesLongitude -= 20;
                    degreesLattitude -= 10;
                    GetTable1C2GLookupPositive.Add(degreesLongitude, letter);
                    GetTable4C2GLookupPositive.Add(degreesLattitude, letter);
                }

                GetTable1G2CLookup.Add(letter, degreesLongitude);

                if (letter == "I")
                {
                    GetTable1C2GLookupNegative.Add(degreesLongitude, letter);
                    GetTable4C2GLookupNegative.Add(degreesLattitude, letter);
                }

                if (degreesLongitude < 0)
                {
                    GetTable1C2GLookupNegative.Add(degreesLongitude, letter);
                    GetTable4C2GLookupNegative.Add(degreesLattitude, letter);
                }

                if (degreesLongitude > 0)
                {
                    GetTable1C2GLookupPositive.Add(degreesLongitude, letter);
                    GetTable4C2GLookupPositive.Add(degreesLattitude, letter);
                }

                GetTable4G2CLookup.Add(letter, degreesLattitude);

                degreesLongitude += 20;
                degreesLattitude += 10;
                tracker++;
            }

            tracker = 0;
            int degreesNegativeLongitude = -18;
            degreesLongitude = 2;
            int degreesNegativeLattitude = -9;
            degreesLattitude = 1;

            GetTable2C2GLookupPositive = new Dictionary<int, int>(9);
            GetTable2C2GLookupNegative = new Dictionary<int, int>(9);

            while (tracker < 10)
            {
                GetTable2C2GLookupPositive.Add(degreesLongitude, tracker);
                GetTable2C2GLookupNegative.Add(degreesNegativeLongitude, tracker);
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
