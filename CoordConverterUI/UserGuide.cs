using System;
using System.Collections.Generic;

namespace CoordConverterUI
{
    internal class UserGuide
    {
        private string[] text =
        {
            @"Coordinate Converter Utility has a few conversion capabilties:
    1. Accepts a valid 6-character gridsquare and returns the approximate center in decimal degrees and minutes (DDM).
    2. Accepts a valid DD like 47 49.52N,122 17.60W and returns the 6-character gridsquare that contains the DD coordinates.
    3. Accepts commands to convert a DD, DDM, or DMS coordinate to a well-formatted text.
    4. Accepts a valid DIREWOLF program output DD and returns a DDM in well-formatted text.
    5. Future: Accept a gridsquare and returns a DD, DMS, or DIREWOLF-formatted DDM.
    6. Future: Accept a DD, DDM, DMS, or DIREWOLF-formatted DDM and returns the encompassing Gridsquare.

    DO NOT USE FOR NAVIGATIONAL PURPOSES!
    Accurracy is limited to within Lat 1.0 secs or Lon 1.0 secs, or Lat 1.25 mins or Lon 2.5 mins, or one ten-thousands of a decimal degree (ex: DD 44.123__)",
            
            @"Usage:
    CoordinateConverter.exe [command] 'coordinate|gridsquare' [-out 'output option']",

            @"Argument Formatting:
    Input coordinates and gridsquares must be surrounded by double-quotes.
    Commands, gridsquare, and nsew indicators are case insensitive.
    Negative signs OR nsew indicators are required else all coordinates are assumed to be in the Northern and Eastern hemisphere quadrant.",
            
            @"Command Input Options:
    -grid, -dd, -ddm, -dms, -direwolf, -h, --help",

            @"\nOutput Options:
    -grid, -dd, -ddm, -dms, -direwolf",

            @"Defaults:
    Default operations without input or output commands:
        A gridsquare input will return a well-formatted DDM text output.
        A DDM input will return a well-formatted DDM text output.
    Default output options are:
        (not yet implemented)",

            @"Defaults Examples:
    CoordinateConverter.exe grid => stringified formatted DDM coordinate
    CoordinateConverter.exe ddm-ish => stringified formatted DDM coordinate
    CoordinateConverter.exe -ddm ddm-ish => stringified formatted DDM coordinate
    CoordinateConverter.exe -dd dd-ish => stringified formatted DD coordinate
    CoordinateConverter.exe -dms dms-ish => stringified formatted DMS coordinate",
            
            @"Output Command Examples:
    CoordinateConverter.exe [-dd | -ddm | -dms] input_coordinate -grid => 'gridsquare'
    CoordinateConverter.exe -grid input_gridsquare [-dd | -ddm | -dms] => stringified formatted coordinate"
        };

        internal List<string> UsageInstructions { get; set; }
        
        public UserGuide()
        {
            UsageInstructions = new List<string>(text);
        }

    }
}
