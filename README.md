# Coordinate Converter

Library of coordinate system conversions and an executable for handy terminal level conversion commands.

## About

As an amateur radio operator I need to be aware of several different coordinate systems.
As a C# developer I want to gain experience programming applications.
Many hams before me have explored solving coordinate conversion in software, and it seems like a pretty natural fit.
The initial idea for this project came for a QST magazine article titled "Conversion Between Geodetic and Grid Locator Systems" by Edmund T. Tyson, N5JTY. [QST January 1989, pp. 29-30, 43]

## Requirements

- Dot NET 6.0 SDK
- Terminal interaction requires a keyboard, mouse, and monitor.

## Usage

There is a built-in help file. Activate it by running `CoordinateConverterCmd.exe` to get output to the screen. Take a look at the [Usage Examples](#usage-examples) for a good start.

Basic Commands:

- `-in_cmd`: If this looks like a DDM the converter will return a Gridsquare. If it looks like a Gridsquare it will return a DDM coordinate.
- `coordinate`: A degrees decimal (DD), decimal degree minutes (DDM), or degrees minutes seconds (DMS) coordinate input text. Degrees symbol is not needed. North, South, East, and West indicators might be necessary depending on the coordinate system used.
- `-grid`: Convert the preceeding coordinate to a Grid Square format.
- `-direwolf`: (FUTURE) This option formats the output suitable to use as an input to a running Direwolf instance.

Note: DIREWOLF is an Automatic Packet Reporting System (APRS :copyright: Bob Bruninga) software package that knows how to transmit and receive messages and coordinates in AX.25 on-air protocol.

### Usage Examples

Display a DDM-like input (ddm-ish) in a "pretty" DD output with degree symbols:

```text
C:\> .\CoordinateConverterCmd.exe -ddm "43 32.21N,120 19.18W"
43°32.21'N, 120°19.18'W
C:\>
```

Convert a DDM-like input into a DD output with degree symbol and minus sign for South or West hemispheres:

```text
C:\> .\CoordinateConverterCmd.exe -ddm "43 32.21N,120 19.18W" -dd
43.53683°, -120.31967°
C:\>
```

Display a DD-like input (dd-ish) in a "pretty" DDM output with degree symbols:

```text
C:\> .\CoordinateConverterCmd.exe -dd "43.53683,-120.31967" -ddm
43°32.21'N, 120°19.18'W
```

Note that the Eastern hemisphere is identified without a minus sign:

```text
C:\> .\CoordinateConverterCmd.exe -dd "43.53683,120.31967" -ddm
43°32.21'N, 120°19.18'E
C:\>
```

There is also DMS:

```text
C:\> .\CoordinateConverterCmd.exe -dd "43.53683,120.31967" -dms
N 43°32'12.6", E 120°19'10.8"
C:\>
```

Convert a DDM coordinate to a Maidenhead/Gread Square format:

```text
C:\> .\CoordinateConverterCmd.exe -ddm "43 32.21N,120 19.18W" -grid
CN93um
C:\>
```

Convert DMS to Grid Square format:

```text
C:\> .\CoordinateConverterCmd.exe -dms "N43 43 12.6, E120 19 10.8" -grid
PN03dr
C:\>
```

## Components

- CoordinateConverterCmd.exe: Terminal/cmd user interface.
- CoordinateConversionLibrary.dll: Library of conversion functions and objects.
- CoordianteConverterCmd.runtimeconfig.json: Required for DotNet 5.0+ stand-alone packaging.

## Installation

There is no installer. Download the release file 'CoordinateConversionUtility*v*._._-\_.zip' from GitHub Releases page and then unzip the package. Eensure these files are all in the current Path so the executable can find them:

- CoordinateConversionLibrary.dll
- CoordinateConverterCmd.dll
- CoordinateConverterCmd.runtimeconfig.json

## Build Requirements

Visual Studio:

- Set CoordinateConverterCmd as the start-up project.
- Build the project

VSCode:

- Install .NET Install Tool for Extension Authors
- Install C# for VS Code (powered by OmniSharp)

## Clone and Build

If you want to use the Coordinate Conversion Library in your project you can either download the Release files and copy the DLLs, importing them into your project to use as an API, or you can clone the source code and import the CoordinateConversionLibrary.cs files into your own project. See LICENSE for details before doing this.

1. Clone this repository.
2. Follow requirements from previous section.
3. VSCode: Execute `dotnet restore`.
4. VSCode: Execute `dotnet build`.

Check the build output location for DLLs and executable: `CoordinateConverterCmd5\bin\Debug\net6.0` and `CoordinateConversionLibrary\bin\Debug\net6.0`

### Visual Studio 2022 (and similar)

1. Run: Build => Clean.
2. Build Setting: Debug => AnyCPU
3. Build: Solution
4. Acquire: CoordinateConverterCmd.exe and CoordinateConversionLibrary.dll in the CoordinateConverterCmd project's bin\Debug\net6.0 output folder.

## Solution Component Interaction

The core converting functionality is in CoordinateConversionLibrary.dll.
CoordinateConverterCmd.exe processes user-supplied args, tries to identify commands text-inputs as coordinates.
The executable then passes appropriate information to the Library for processing based on the command inputs.
The exe includes a built-in Help file.

## Bugs

Unfortunately there are bugs. See the [Issues list on GitHub](https://github.com/nojronatron/CoordinateConversionUtility/issues) for known bugs and other work items.

## About QST Magazine

Access to QST Magazine and its articles might be restricted to ARRL Members only. To gain access, become an ARRL member and keep your amateur license up-to-date. I am a current ARRL Member, and an active member of the amateur community.
