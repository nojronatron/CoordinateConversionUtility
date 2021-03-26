using System;
using System.Collections.Generic;

namespace CoordinateConverterCmd
{
    internal class UserGuide
    {
        private readonly string[] text =
        {
            @"Coordinate Converter Utility by Jon Rumsey",

            @"*** WARNING! DO NOT USE FOR NAVIGATIONAL PURPOSES! ***
    Accuracy is limited. Gridsquares to within 1.0 Degree Lat or Lon. Coords are within ~5 mins or 0.0012 degree, Lat or Lon.",

            @"Coordinate Converter Utility has a few conversion capabilties:
    1. Accepts a valid 6-character gridsquare and returns the approximate center point, in DDM.
    2. Accepts a valid DDM within quotes and returns a DDM in 'pretty' format with degree and minute markers.
    3. Accepts an input command followed by a quoted DD, DDM, or DMS coordinate and returns as pretty text.
    4. Accepts a valid DIREWOLF program output coordinate and returns a pretty Grid, DD, DDM, or DMS, per the output command.
    5. Accepts a -grid command followed by a gridsquare and returns a pretty DD, DDM, or DMS coordinate, per the output command.
    6. Accepts a -dd, -ddm, -dms, or -direwolf comand followed by a quoted coordinate and returns a pretty DD, DMS, or DDM coordinate, per the output command.",

            @"Usage:
    CoordinateConverter.exe -in_cmd 'coordinate'|gridsquare [-out_cmd]",

            @"Argument Formatting:
    Input coordinates must be surrounded by double-quotes if there are any spaces.
    Degree, Minute, and Second markers are assumed. A DMS could be like: 'N22 34 17.5, E46 18 59.8'.
    Commands, gridsquare, and N, S, E and W indicators are NOT case-sensitive.
    Negative signs required for DD; N, S, E and W indicators required for DDM, DMS, and Direwolf coordinates.",

            @"Input Command Options:
    CoordConverterCmd.exe [-in_cmd] [user_input]
    CoordConverterCmd.exe -grid gridsquare | -dd 'ddish' | -ddm 'ddmish' | -dms 'dmsish' | -direwolf 'DW1.6'
    Invalid commands will launch this help page.",

            @"Output Options:
    CoordConverterCmd.exe -in_cmd user_input -out_cmd
    CoordConverterCmd.exe -in_cmd user_input -grid => user_input converted to a Gridsquare.
    CoordConverterCmd.exe -in_cmd user_input -dd => user_input is converted to a DD coordinate.
    CoordConverterCmd.exe -in_cmd user_input -ddm => user_input is converted to a DDM coordinate.
    CoordConverterCmd.exe -in_cmd user_input -dms => user_input is converted to a DMS coordinate.",

            @"Default Behaviors:
    CoordConverterCmd.exe => Displays this Help file.
    CoordConverterCmd.exe ddm-ish => Well-formated DDM coordinate.
    CoordConverterCmd.exe gridsquare => Well-formatted DDM coordinate at center of the input gridsquare.
    CoordConverterCmd.exe -dd dd-ish => Well-formatted DD coordinate.
    CoordConverterCmd.exe -ddm ddm-ish => Well-formatted DDM coordinate.
    CoordConverterCmd.exe -dms dms-ish => Well-formatted DMS coordinate."

        };

        internal List<string> UsageInstructions { get; set; }

        public UserGuide()
        {
            UsageInstructions = new List<string>(text);
        }

    }
}
