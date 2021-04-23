The goals of this project are to explore coordinate systems, conversions between them, and to practice software dev lifecycle processes.

As an amateur radio operator I need to be aware of several different coordinate systems.

As a C# developer I want to gain experience programming applications.

Many hams before me have explored solving coordinate conversion in software, and it seems like a pretty natural fit.

The initial idea for this project came for a QST magazine article titled "Conversion Between Geodetic and Grid Locator Systems" by Edmund T. Tyson, N5JTY. [QST January 1989, pp. 29-30, 43]

REQUIREMENTS
Dot NET 5.0
Keyboard, mouse, and monitor.
CoordinateConversionLibrary.dll: Core utility, must be in PATH for .exe to find and use it.
CoordinateConverterCmd.exe: PowerShell or Command user-interface.

UNDER THE COVERS
The core converting functionality is in CoordinateConversionLibrary.dll.
CoordinateConverterCmd.exe processes user-supplied args, tries to identify commands and coordinate text. It then passes appropriate information to the Library for processing based on the command inputs. The exe includes a built-in Help file.

TO BUILD
Fork MASTER at the desired Tag.
Set: CoordinateConverterCmd as the start-up project.
Run: Build => Clean.
Build Setting: Debug => AnyCPU
Build: Solution
Acquire: CoordinateConverterCmd.exe and CoordinateConversionLibrary.dll in the CoordinateConverterCmd project's bin\Debug\net5.0 output folder.

** NOTE **
Access to QST Magazine and its articles might be restricted to ARRL Members only.
I am a current ARRL Member, and an active member of the amateur community.
