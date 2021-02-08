using System;
using System.Text;
using CoordinateConversionUtility.Models;
using CoordinateConversionUtility.Helpers;

namespace CoordinateConversionUtility
{
    /// <summary>
    /// Coordinate Converter core library. Responsibilities:
    ///     Provide a means for another service or app to get conversions between different coordinate systems.
    ///     Orchestrate the conversion process(es).
    ///     Arrange any other required objects/instances (TBD).
    /// Calling App or Service must decide how to present results to user or other dependent system,
    ///     although ToString() is customized on coordinate objects.
    /// </summary>
    public class CoordinateConverter
    {
        private GridSquareHelper GridSquareHelper { get; set; }
        private LookupTablesHelper LookupTablesHelper { get; set; }
        public StringBuilder GridsquareResult { get; set; }
        public DDMCoordinate DdmResult { get; set; }

        public CoordinateConverter()
        {
            GridSquareHelper = new GridSquareHelper();
            LookupTablesHelper = new LookupTablesHelper();
            DdmResult = null;
            GridsquareResult = null;
        }

        /// <summary>
        /// Takes a 6-character string gridsquare, verifies the formatting, and returns a DDMCoordinate object.
        /// </summary>
        /// <param name="gridsquare"></param>
        /// <returns></returns>
        public DDMCoordinate ConvertGridsquareToDDM(string gridsquare)
        {
            DdmResult = null;

            if (!string.IsNullOrEmpty(gridsquare) && !string.IsNullOrWhiteSpace(gridsquare))
            {

                if (LookupTablesHelper.GenerateTableLookups())
                {
                    if (GridSquareHelper.ValidateGridsquareInput(gridsquare, out string validGridsquare))
                    {
                        decimal tempLatDegrees = ConversionHelper.GetLatDegrees(LookupTablesHelper, validGridsquare, out short latDirection);
                        decimal latDegreesWithRemainder = ConversionHelper.AddLatDegreesRemainder(tempLatDegrees, latDirection, validGridsquare);
                        decimal DDMlatMinutes = ConversionHelper.GetLatMinutes(
                            LookupTablesHelper, latDegreesWithRemainder, latDirection, validGridsquare, out decimal adjustedLatDegrees);

                        decimal tempLonDegrees = ConversionHelper.GetLonDegrees(LookupTablesHelper, validGridsquare, out short lonDirection);
                        decimal lonDegreesWithRemainder = ConversionHelper.AddLonDegreesRemainder(tempLonDegrees, lonDirection, validGridsquare);
                        decimal DDMlonMinutes = ConversionHelper.GetLonMinutes(
                            LookupTablesHelper, lonDegreesWithRemainder, lonDirection, validGridsquare, out decimal adjustedLonDegrees);

                        DdmResult = new DDMCoordinate(
                            adjustedLatDegrees, DDMlatMinutes,
                            adjustedLonDegrees, DDMlonMinutes);
                    }
                }
            }

            return DdmResult;
        }

        /// <summary>
        /// Returns a string representation of the calculated GridSquare from a DDM Coordinate object.
        /// </summary>
        /// <param name="ddmCoordinates"></param>
        /// <returns></returns>
        public string ConvertDDMtoGridsquare(DDMCoordinate ddmCoordinates)
        {
            if (ddmCoordinates == null)
            {
                return "NOcrds";
            }

            GridsquareResult = new StringBuilder();

            if (LookupTablesHelper.GenerateTableLookups())
            {
                decimal DDMlatDegrees = ddmCoordinates.GetShortDegreesLat();
                decimal DDMlonDegrees = ddmCoordinates.GetShortDegreesLon();
                decimal DDMlatMinutes = ddmCoordinates.MinutesLattitude;
                decimal DDMlonMinutes = ddmCoordinates.MinutesLongitude;

                int LatDirection = ConversionHelper.ExtractPolarityNS(ddmCoordinates.ToString());
                int LonDirection = ConversionHelper.ExtractPolarityEW(ddmCoordinates.ToString());

                GridsquareResult.Append(GridSquareHelper.GetFirstGridsquareCharacter(DDMlonDegrees, LonDirection, out decimal remainderLon));
                GridsquareResult.Append(GridSquareHelper.GetSecondGridsquareCharacter(DDMlatDegrees, LatDirection, out decimal remainderLat));

                GridsquareResult.Append(GridSquareHelper.GetThirdGridsquareCharacter(remainderLon, LonDirection, out decimal minsRemainderLon));
                GridsquareResult.Append(GridSquareHelper.GetFourthGridsquareCharacter(remainderLat, LatDirection, out decimal minsRemainderLat));

                var ddmLonMinsWithRemainder = LonDirection * (minsRemainderLon + DDMlonMinutes);
                decimal nearestEvenMultipleLon = ConversionHelper.GetNearestEvenMultiple(ddmLonMinsWithRemainder, 2);
                GridsquareResult.Append(GridSquareHelper.GetFifthGridsquareCharacter(LonDirection, nearestEvenMultipleLon));

                var ddmLatMinsWithRemainder = LatDirection * (minsRemainderLat + DDMlatMinutes);
                decimal nearestEvenMultipleLat = ConversionHelper.GetNearestEvenMultiple(ddmLatMinsWithRemainder, 1);
                GridsquareResult.Append(GridSquareHelper.GetSixthGridsquareCharacter(LatDirection, nearestEvenMultipleLat));
            }
            else
            {
                return "NoTbls";
            }

            return GridsquareResult.ToString();
        }

    }
}
