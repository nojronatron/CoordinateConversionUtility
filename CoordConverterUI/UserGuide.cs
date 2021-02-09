using System;
using System.Collections.Generic;

namespace CoordConverterUI
{
    internal class UserGuide
    {
        //  CoordinateConverter.exe -grid GG11gg | ddm NS DD MM.dd, EW DDD MM.dd | dd +-DD.ddddd,+=DDD.ddddd | dms DD mm ss.s NS, DDD mm ss.s EW] -out []
        private string[] text =
        {
            @"Coordinate Converter Utility takes a 6-character gridsquare (maidenhead) and returns the approximate center in decimal degrees and minutes (DDM).
    Optional commands will convert DDM to gridsquare and other conversions.
    Accurracy is limited to within a couple decimal minutes, or 3 decimal digits (DD 44.123__)",
            
            @"Usage:
    CoordinateConverter.exe [-grid | -dd | -ddm | -dms] (input option) -out (output option)",
            
            @"Input Options:
    -in\t-grid, -dd, -ddm, -dms
    -out\t-grid, -dd, -ddm, -dms",

            @"\nOutput Options:
    -in\t-grid, -dd, -ddm, -dms
    -out\t-grid, -dd, -ddm, -dms",

            @"Defaults:
    CoordinateConverter.exe gridsquare => stringified formatted DDM coordinate
    CoordinateConverter.exe -ddm ddm-ish => stringified formatted DDM coordinate
    CoordinateConverter.exe -dd ddm-ish => stringified formatted DD coordinate
    CoordinateConverter.exe -dms dms-ish => stringified formatted DMS coordinate",
            
            @"Other Commands:
    CoordinateConverter.exe [-dd | -ddm | -dms] input -grid => 'gridsquare'
    CoordinateConverter.exe -gridsquare gridsquare [-dd | -ddm | -dms] => stringified formatted coordinate"
        };

        internal List<string> UsageInstructions { get; set; }
        
        public UserGuide()
        {
            UsageInstructions = new List<string>(text);
        }

    }
}
