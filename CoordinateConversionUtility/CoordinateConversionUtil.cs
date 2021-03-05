using CoordinateConversionUtility.Helpers;
using CoordinateConversionUtility.Models;
using System.Text;

namespace CoordinateConversionUtility
{
    /// <summary>
    /// Provides a means to convert back and forth between a gridsquare and DDM style coordinate.
    /// Calling App or Service must decide how to present results to user.
    /// </summary>
    public class CoordinateConverter
    {
        private GridSquareHelper GridsquareHelper { get; set; }
        private LookupTablesHelper LookuptablesHelper { get; set; }

        public CoordinateConverter()
        {
            GridsquareHelper = new GridSquareHelper();
            LookuptablesHelper = new LookupTablesHelper();
        }

        /// <summary>
        /// Takes a 6-character string gridsquare and processes it using LookupTablesHelper and GridsquareHelper to return a DDMCoordinate object.
        /// If solution cannot be found a null DDMCoordinate type is returned.
        /// </summary>
        /// <param name="gridsquare"></param>
        /// <returns></returns>
        public DDMCoordinate ConvertGridsquareToDDM(string gridsquare)
        {
            DDMCoordinate DdmResult = null;

            if (!string.IsNullOrEmpty(gridsquare) && !string.IsNullOrWhiteSpace(gridsquare))
            {
                if (LookuptablesHelper.GenerateTableLookups() && GridsquareHelper.ValidateGridsquareInput(gridsquare, out string validGridsquare))
                {
                    decimal tempLatDegrees = ConversionHelper.GetLatDegrees(LookuptablesHelper, validGridsquare, out short latDirection);
                    decimal latDegreesWithRemainder = ConversionHelper.AddLatDegreesRemainder(tempLatDegrees, latDirection, validGridsquare);
                    decimal DDMlatMinutes = ConversionHelper.GetLatMinutes(
                        LookuptablesHelper, latDegreesWithRemainder, latDirection, validGridsquare, out decimal adjustedLatDegrees);

                    decimal tempLonDegrees = ConversionHelper.GetLonDegrees(LookuptablesHelper, validGridsquare, out short lonDirection);
                    decimal lonDegreesWithRemainder = ConversionHelper.AddLonDegreesRemainder(tempLonDegrees, lonDirection, validGridsquare);
                    decimal DDMlonMinutes = ConversionHelper.GetLonMinutes(
                        LookuptablesHelper, lonDegreesWithRemainder, lonDirection, validGridsquare, out decimal adjustedLonDegrees);

                    DdmResult = new DDMCoordinate(
                        adjustedLatDegrees, DDMlatMinutes,
                        adjustedLonDegrees, DDMlonMinutes);
                }
            }

            return DdmResult;
        }

        /// <summary>
        /// Takes a DDMCoordinate objects and processes it using ConversionHelper an GridSquareHelper to return a Gridsquare string.
        /// If solution cannot be found a string with encoded error condition is returned and should be handled by the caller.
        /// </summary>
        /// <param name="ddmCoordinates"></param>
        /// <returns></returns>
        public string ConvertDDMtoGridsquare(DDMCoordinate ddmCoordinates)
        {
            if (ddmCoordinates == null)
            {
                return "BadInp";
            }

            if (false == LookuptablesHelper.GenerateTableLookups())
            {
                return "NoTbls";
            }

            var GridsquareResult = new StringBuilder();

            decimal DDMlatDegrees = ddmCoordinates.GetShortDegreesLat();
            decimal DDMlonDegrees = ddmCoordinates.GetShortDegreesLon();
            decimal DDMlatMinutes = ddmCoordinates.MinutesLattitude;
            decimal DDMlonMinutes = ddmCoordinates.MinutesLongitude;

            short LatDirection = ConversionHelper.ExtractPolarityNS(ddmCoordinates.ToString());
            short LonDirection = ConversionHelper.ExtractPolarityEW(ddmCoordinates.ToString());

            GridsquareResult.Append(GridsquareHelper.GetFirstGridsquareCharacter(DDMlonDegrees, LonDirection, out decimal remainderLon));
            GridsquareResult.Append(GridsquareHelper.GetSecondGridsquareCharacter(DDMlatDegrees, LatDirection, out decimal remainderLat));

            GridsquareResult.Append(GridSquareHelper.GetThirdGridsquareCharacter(remainderLon, LonDirection, out decimal minsRemainderLon));
            GridsquareResult.Append(GridSquareHelper.GetFourthGridsquareCharacter(remainderLat, LatDirection));

            var ddmLonMinsWithRemainder = LonDirection * (minsRemainderLon + DDMlonMinutes);
            decimal nearestEvenMultipleLon = ConversionHelper.GetNearestEvenMultiple(ddmLonMinsWithRemainder, 2);
            GridsquareResult.Append(GridsquareHelper.GetFifthGridsquareCharacter(LonDirection, nearestEvenMultipleLon));

            var ddmLatMinsWithRemainder = LatDirection * DDMlatMinutes;
            decimal nearestEvenMultipleLat = ConversionHelper.GetNearestEvenMultiple(ddmLatMinsWithRemainder, 1);
            GridsquareResult.Append(GridsquareHelper.GetSixthGridsquareCharacter(LatDirection, nearestEvenMultipleLat));

            if (GridsquareResult.Length != 6)
            {
                return "NoCalc";
            }

            return GridsquareResult.ToString();
        }

    }
}
