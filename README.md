As an amateur radio operator I need to be aware of several different coordinate systems.

As a C# developer I want to explore coordinate systems and conversions in code.

The goals of this project are to explore coordinate systems, conversions between them, and to practice software dev lifecycle processes.

The initial idea for this project came for a QST magazine article titled "Conversion Between Geodetic and Grid Locator Systems" by Edmund T. Tyson, N5JTY. [QST January 1989, pp. 29-30, 43]

REQUIREMENTS
Windows with .NET Framework 4.7.2.
Keyboard, mouse, and monitor.
CoordinateConversionUtility.dll: Core utility, must be in PATH for .exe to find and use it.
CoordinateConverterCmd.exe: PowerShell or Command user-interface.

UNDER THE COVERS
The primary functionality is in CoordinateConversionUtility.dll. CoordinateConverterCmd.exe just does some cursory searches for commands and sorts information for the DLL to work with. The .exe also has a built-in Help file. Only default .NET Libraries are utilized (and very few of them at that). No special packages or DLL's needed.

TO BUILD
Fork MASTER at the desired Tag.
Set: CoordinateConverterCmd as the start-up project.
Run: Build => Clean.
Build: Release => AnyCPU.
Acquire: CoordinateConverterCmd.exe and CoordianteConversionUtility.dll in Release output and have fun.

** NOTE **
Access to QST Magazine and its articles might be restricted to ARRL Members only.
I am a current ARRL Member, and an active member of the amateur community.
