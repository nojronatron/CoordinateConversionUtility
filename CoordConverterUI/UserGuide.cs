using System.Collections.Generic;

namespace CoordConverterUI
{
    internal class UserGuide
    {
        private readonly string[] text =
        {
            @"Coordinate Converter Utility by Jon Rumsey",

            @"*** WARNING! DO NOT USE FOR NAVIGATIONAL PURPOSES! ***
    Accuracy is limited. Gridsquares to within 1.0 Degree Lat or Lon. Coords are within 5 mins and 0.0001 degree, Lat or Lon.",

            @"Coordinate Converter Utility has a few conversion capabilties:
    1. Accepts a valid 6-character gridsquare and returns the approximate center point, in DDM.
    2. Accepts a valid DDM within quotes and returns the 6-character gridsquare.
    3. Accepts an input DD, DDM, or DMS coordinate into a well-formatted text per output_cmd.
    4. Accepts a valid DIREWOLF program output coordinate and returns a Grid, DD, DDM, or DMS, well-formatted.
    5. Accepts a gridsquare and returns a DD, DDM, or DMS coordinate.
    6. Accepts a DD, DDM, DMS, or DIREWOLF-formatted DDM and returns the encompassing Gridsquare.",

            @"Usage:
    CoordinateConverter.exe -in_cmd 'coordinate'|gridsquare [-out_cmd]",

            @"Argument Formatting:
    Input coordinates MUST must be surrounded by double-quotes.
    Commands, gridsquare, and N, S, E and W indicators are case INsensitive.
    Negative signs OR N, S, E and W indicators are required.",

            @"Input Command Options:
    CoordinateConverter.exe [-in_cmd] [user_input]
    CoordinateConverter.exe -grid gridsquare | -dd 'ddish' | -ddm 'ddmish' | -dms 'dmsish' | -direwolf 'DW1.6' | -h | --help
        => Describes the user input format (gridsquare, ddish, etc) or launches this help.",

            @"Output Options:
    CoordinateConverter.exe -in_cmd user_input -out_cmd
    CoordinateConverter.exe -in_cmd user_input -grid => user_input converted to a Gridsquare.
    CoordinateConverter.exe -in_cmd user_input -dd => user_input is converted to a DD coordinate.
    CoordinateConverter.exe -in_cmd user_input -ddm => user_input is converted to a DDM coordinate.
    CoordinateConverter.exe -in_cmd user_input -dms => user_input is converted to a DMS coordinate.",

            @"Default Behaviors:
    CoordinateConverter.exe => Displays this Help file.
    CoordinateConverter.exe 'ddm-ish' => Well-formated Gridsquare containing coordinate point.
    CoordinateConverter.exe 'gridsquare' => Well-formatted DDM coordinate at center of the input gridsquare.
    CoordinateConverter.exe -dd dd-ish => Well-formatted DD coordinate.
    CoordinateConverter.exe -ddm ddm-ish => Well-formatted DDM coordinate.
    CoordinateConverter.exe -dms dms-ish => Well-formatted DMS coordinate."

        };

        internal List<string> UsageInstructions { get; set; }

        public UserGuide()
        {
            UsageInstructions = new List<string>(text);
        }

    }
}
