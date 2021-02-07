using System;
using System.Globalization;
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
        //  TODO: CoordinateConverter 1) Refactor this to hide all detailed methods that a calling app/service doesn't need to know/care about.
        //  TODO: CoordinateConverter 2) Move any unnecessary methods out to the Helper classes (remember ConversionHelper is STATIC).
        private readonly CultureInfo currentCulture = CultureInfo.CurrentCulture;

        private DDCoordinate DecimalDegreesCoordinate { get; set; } = (DDCoordinate)default;
        private DDMCoordinate DdmResult { get; set; } = (DDMCoordinate)default;
        private DMSCoordinate DegreesMinutesSecondsCoordinate { get; set; } = (DMSCoordinate)default;
        private GridSquareHelper GridSquareHelper { get; set; }
        private LookupTablesHelper LookupTablesHelper { get; set; }


        protected short LatDirection { get; private set; }   // 1 or -1: represents N or S, respectively
        protected short LonDirection { get; private set; }   // 1 or -1: represents E or W, respectively
        protected decimal DDMlatDegrees { get; private set; }  //  ever negative?  // degree portion of DDM e.g.: 47 in 47*48.75"N
        protected decimal DDMlatMinutes { get; private set; }  //  ever negative?  // decimal minutes portion of DDM e.g.: 48.75 in 47*48.75"N
        protected decimal DDMlonDegrees { get; private set; }  //  evern egative?  // degree portion of DDM e.g.: -122 in 122*17.5"W
        protected decimal DDMlonMinutes { get; private set; }  //  ever negative?  //  decimal minutes portion of DDM e.g.: 17.5 in 122*17.5"W
        //protected decimal RemainderLat { get; private set; }      // stores carry-over remainder value for Lattitude calculations
        //protected decimal RemainderLon { get; private set; }      // stores carry-over remainder value for Longitude calculations

        public CoordinateConverter()
        {
            DecimalDegreesCoordinate = new DDCoordinate();
            DdmResult = new DDMCoordinate();
            DegreesMinutesSecondsCoordinate = new DMSCoordinate();
            GridSquareHelper = new GridSquareHelper();
            LookupTablesHelper = new LookupTablesHelper();
        }

        /// <summary>
        /// Takes a 6-character string gridsquare, verifies the formatting, and returns a DDMCoordinate object.
        /// </summary>
        /// <param name="gridsquare"></param>
        /// <returns></returns>
        public DDMCoordinate ConvertGridsquareToDDM(string gridsquare)
        {
            if (!string.IsNullOrEmpty(gridsquare) && !string.IsNullOrWhiteSpace(gridsquare))
            {

                if (LookupTablesHelper.GenerateTableLookups())
                {
                    var tempLatDegrees = 0.0m;
                    decimal adjustedLatDegrees = 0;
                    var tempLatMinutes = 0.0m;

                    var tempLonDegrees = 0.0m;
                    decimal adjustedLonDegrees = 0;
                    var tempLonMinutes = 0.0m;

                    if (GridSquareHelper.ValidateGridsquareInput(gridsquare, out string validGridsquare))
                    {
                        tempLatDegrees = ConversionHelper.GetLatDegrees(LookupTablesHelper, validGridsquare, out short latDirection);
                        LatDirection = latDirection;
                        //LatDirection = ConversionHelper.ExtractPolarity(DDMlatDegrees);
                        
                        decimal latDegreesWithRemainder = ConversionHelper.AddLatDegreesRemainder(tempLatDegrees, LatDirection, validGridsquare);
                        DDMlatMinutes = ConversionHelper.GetLatMinutes(LookupTablesHelper, latDegreesWithRemainder, LatDirection, validGridsquare, out adjustedLatDegrees);

                        DDMlonDegrees = ConversionHelper.GetLonDegrees(LookupTablesHelper, validGridsquare, out short lonDirection);
                        LonDirection = lonDirection;
                        //LonDirection = ConversionHelper.ExtractPolarity(DDMlonDegrees);
                        decimal lonDegreesWithRemainder = ConversionHelper.AddLonDegreesRemainder(DDMlonDegrees, LonDirection, validGridsquare);
                        DDMlonMinutes = ConversionHelper.GetLonMinutes(LookupTablesHelper, lonDegreesWithRemainder, LonDirection, validGridsquare, out adjustedLonDegrees);

                        //var nearestEvenLatMinute = ConversionHelper.GetNearestEvenMultiple(DDMlatMinutes, 1);
                        //var nearestEvenLonMinute = ConversionHelper.GetNearestEvenMultiple(DDMlonMinutes, 2);
                    }

                    DdmResult = new DDMCoordinate(
                        adjustedLatDegrees, DDMlatMinutes,
                        adjustedLonDegrees, DDMlonMinutes);

                    return DdmResult;
                }
            }

            return new DDMCoordinate();
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
                return string.Empty;
            }

            var gridsquare = new StringBuilder();

            if (LookupTablesHelper.GenerateTableLookups())
            {
                DDMlatDegrees = ddmCoordinates.GetShortDegreesLat();
                DDMlonDegrees = ddmCoordinates.GetShortDegreesLon();
                DDMlatMinutes = ddmCoordinates.MinutesLattitude;
                DDMlonMinutes = ddmCoordinates.MinutesLongitude;
                LatDirection = ConversionHelper.ExtractPolarityNS(ddmCoordinates.ToString());
                LonDirection = ConversionHelper.ExtractPolarityEW(ddmCoordinates.ToString());

                gridsquare.Append(GridSquareHelper.GetFirstGridsquareCharacter(DDMlonDegrees, LonDirection, out decimal remainderLon));
                gridsquare.Append(GridSquareHelper.GetSecondGridsquareCharacter(DDMlatDegrees, LatDirection, out decimal remainderLat));

                gridsquare.Append(GridSquareHelper.GetThirdGridsquareCharacter(remainderLon, LonDirection, out decimal minsRemainderLon));
                gridsquare.Append(GridSquareHelper.GetFourthGridsquareCharacter(remainderLat, LatDirection, out decimal minsRemainderLat));

                var ddmLonMinsWithRemainder = LonDirection * (minsRemainderLon + DDMlonMinutes);
                decimal nearestEvenMultipleLon = ConversionHelper.GetNearestEvenMultiple(ddmLonMinsWithRemainder, 2);
                //gridsquare.Append(GridSquareHelper.GetFifthGridsquareCharacter(ddmLonMinsWithRemainder, LonDirection, nearestEvenMultipleLon));
                //decimal nearestEvenMultipleLon = ConversionHelper.GetNearestEvenMultiple(DDMlonMinutes, 2);
                gridsquare.Append(GridSquareHelper.GetFifthGridsquareCharacter(minsRemainderLon, LonDirection, nearestEvenMultipleLon));

                var ddmLatMinsWithRemainder = LatDirection * (minsRemainderLat + DDMlatMinutes);
                decimal nearestEvenMultipleLat = ConversionHelper.GetNearestEvenMultiple(ddmLatMinsWithRemainder, 1);
                //gridsquare.Append(GridSquareHelper.GetSixthGridsquareCharacter(ddmLatMinsWithRemainder, LatDirection, nearestEvenMultipleLat));
                //decimal nearestEvenMultipleLat = ConversionHelper.GetNearestEvenMultiple(DDMlatMinutes, 1);
                gridsquare.Append(GridSquareHelper.GetSixthGridsquareCharacter(minsRemainderLat, LatDirection, nearestEvenMultipleLat));
            }
            else
            {
                return "??????";
            }

            return gridsquare.ToString();
        }
        //public string GetGridsquare() => Gridsquare.ToString();

        //public void SetGridsquare(string gridsquare)
        //{   // use for testing. Externally setting this value can produce unexpected results
        //    Contract.Requires(gridsquare != null);
        //    if (0 < gridsquare.Length && gridsquare.Length < 7)
        //    {
        //        if (GridSquareHelper.ValidateGridsquareInput(gridsquare, out string formattedGridsquare))
        //        {
        //            Gridsquare.Clear();
        //            Gridsquare.Append(formattedGridsquare);
        //        }
        //    }
        //    else
        //    {
        //        throw new ArgumentOutOfRangeException($"Argument out of range in { System.Reflection.MethodInfo.GetCurrentMethod().Name }");
        //    }
        //}

        //public void GetLatDegrees()
        //{   // Table4 Lookup primary Degrees Lattitude from a GridSquare character
        //    if (LookupTablesHelper.GetTable4G2CLookup.TryGetValue(Gridsquare[1].ToString(currentCulture).ToUpper(culture: currentCulture), out int latDegreesLookupResult))
        //    {
        //        if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "I" || latDegreesLookupResult < 0)
        //        {
        //            LatDirection = -1;
        //        }
        //        else if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "J" || latDegreesLookupResult > 0)
        //        {
        //            LatDirection = 1;
        //        }
        //        else
        //        {
        //            throw new KeyNotFoundException($"{Gridsquare[1].ToString(currentCulture)} of {Gridsquare} not found in Table4");
        //        }
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException($"Gridsquare member {Gridsquare[0]} (index zero of {Gridsquare}) or LonDegrees {DDMlatDegrees} did not match the program logic.");
        //    }

        //    DDMlatDegrees = Math.Abs(latDegreesLookupResult);
        //}

        //public void AddLatDegreesRemainder()
        //{   // Table5 is calculated and will add to Degrees Lattitude, MUST BE ZERO_BIASED
        //    int lat_MinsAdjustment = 0; // -9;

        //    if (LatDirection == 1)
        //    {
        //        lat_MinsAdjustment = 0;
        //    }
        //    else if (LatDirection == -1)
        //    {
        //        lat_MinsAdjustment = -9;
        //    }

        //    DDMlatDegrees += ((int.Parse(Gridsquare[3].ToString(currentCulture), currentCulture) + lat_MinsAdjustment) * LatDirection);
        //}

        //public void GetLatMinutes()
        //{   // Table6 Lookup Lattitude Minutes including Round and increment/decrement Lat Degrees with carry-over
        //    // NOTE Dependency on LatDirection
        //    if (LookupTablesHelper.GetTable6G2CLookup.TryGetValue(Gridsquare[5].ToString(currentCulture).ToUpper(currentCulture), out decimal latMinsLookupResult))
        //    {
        //        if (LatDirection > 0)
        //        {   // the positive side (0 thru 60) of the Table but ZERO_BIASED so add 2.5 less than 60
        //            latMinsLookupResult += 57.5m + LatMinsRound;
        //        }
        //        else
        //        {   // the negative side (-60 thru 0) of the Table
        //            latMinsLookupResult += (LatMinsRound * LatDirection);
        //        }

        //        while (Math.Abs(latMinsLookupResult) >= 60)
        //        {
        //            DDMlatDegrees += (1 * LatDirection);
        //            latMinsLookupResult -= 60;
        //        }
        //    }
        //    else
        //    {
        //        throw new KeyNotFoundException($"{Gridsquare[5]} of {Gridsquare} not found in Table6");
        //    }

        //    DDMlatMinutes = Math.Abs(latMinsLookupResult);
        //}

        //public void GetLonDegrees()
        //{   // the 1st portion of Degrees Longitude IS the successfull lookup of first_lonChar in Table1
        //    if (LookupTablesHelper.GetTable1G2CLookup.TryGetValue(Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture), out int lonDegreesLookupResult))
        //    {
        //        //  DDMlonDegrees will be set to LonDirection MUST also be set
        //        if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "I" || lonDegreesLookupResult < 0)
        //        {   // if Gridsquare is I result should be between -20 and 0
        //            LonDirection = -1;
        //        }
        //        else if (Gridsquare[0].ToString(currentCulture).ToUpper(currentCulture) == "J" || lonDegreesLookupResult >= 0)
        //        {   // if Gridsquare is J result should be between 0 and 20
        //            LonDirection = 1;
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException($"Gridsquare member {Gridsquare[0]} (index zero of {Gridsquare}) or LonDegrees {lonDegreesLookupResult} did not match the program logic.");
        //        }
        //    }
        //    else
        //    {
        //        throw new KeyNotFoundException($"{Gridsquare[0]} of {Gridsquare} not found in Table1!");
        //    }

        //    DDMlonDegrees = Math.Abs(lonDegreesLookupResult);
        //}

        //public void AddLonDegreesRemainder()
        //{   // Table2 lookup is calculated, then added to Degrees Longitude, MUST BE ZERO_BIASED
        //    //  LonDirection must be checked (should have been set PRIOR to reaching this method)
        //    if (LonDirection == 1)
        //    {   //  calc gridchar Number to Positive side of table is (num * 2)
        //        //lon_MinsAdjustment = 0;
        //        DDMlonDegrees += (int.Parse(Gridsquare[2].ToString(currentCulture), currentCulture) * 2);
        //    }
        //    else if (LonDirection == -1)
        //    {
        //        //  calc gridchar Number to Negative side of table: (num * 2) - 18 
        //        int lon_MinsAdjustment = -18;
        //        DDMlonDegrees += Math.Abs((lon_MinsAdjustment + (int.Parse(Gridsquare[2].ToString(currentCulture), currentCulture) * 2)));
        //    }
        //}
        //public void GetLonMinutes()
        //{   // Table3 is Minutes Longitude Lookup table will also Round-up/down and in/decrement Lon Degrees with carry-over
        //    int lonMinsReducer;

        //    if (LookupTablesHelper.GetTable3G2CLookup.TryGetValue(Gridsquare[4].ToString(currentCulture).ToUpper(currentCulture), out decimal lonMinsLookupResult))
        //    {
        //        if (LonDirection > 0)
        //        {
        //            lonMinsLookupResult += 115 + LonMinsRound;
        //            lonMinsReducer = 60;
        //        }
        //        else
        //        {
        //            lonMinsLookupResult += (LonDirection * LonMinsRound);
        //            lonMinsReducer = -60;
        //        }

        //        while (Math.Abs(lonMinsLookupResult) >= 60) //   120)
        //        {
        //            DDMlonDegrees += 1;
        //            lonMinsLookupResult -= lonMinsReducer;  //   120;
        //        }
        //    }
        //    else
        //    {
        //        throw new KeyNotFoundException($"{Gridsquare[4].ToString(currentCulture)} of {Gridsquare} not found in Table3!");
        //    }

        //    DDMlonMinutes = Math.Abs(lonMinsLookupResult);
        //}

        ///// <summary>
        ///// Returns decimal value that maps to nearest even multiple of Lat (1) or Lon (2) coordinates, or 0.0m if solution not found.
        ///// </summary>
        ///// <param name="minutesInput"></param>
        ///// <param name="latOrLon"></param>
        ///// <returns></returns>
        //private static decimal GetNearestEvenMultiple(decimal minutesInput, int latOrLon)
        //{   
        //    decimal interval = 0.0m;

        //    if (latOrLon == 1)  //  1 = lattitude
        //    {   //  incremental steps mapping to gridsquare north and south edges (Lattitude)
        //        interval = 2.5m;
        //    }
        //    else
        //    {   //  incremental steps mapping to gridsquare east and west edges (Longitude)
        //        interval = 5.0m; 
        //    }

        //    if (minutesInput % interval == 0)
        //    {   //  interval is an even divisor of minutesInput so return it
        //        return minutesInput;
        //    }

        //    decimal LowEndMultiple = (Math.Truncate(minutesInput / interval)) * interval;
        //    decimal HighEndMultiple = LowEndMultiple + interval;
        //    decimal LowEndDifference = Math.Abs(minutesInput - LowEndMultiple);
        //    decimal HighEndDifference = Math.Abs(minutesInput - HighEndMultiple);

        //    if (LowEndDifference < HighEndDifference)
        //    {
        //        if (minutesInput < 0)
        //        {
        //            return LowEndMultiple;
        //        }

        //        if (minutesInput > 0)
        //        {
        //            return  HighEndMultiple;
        //        }
        //    }
        //    else if (LowEndDifference > HighEndDifference)
        //    {
        //        if (minutesInput > 0)
        //        {
        //            return HighEndMultiple;
        //        }

        //        if (minutesInput < 0)
        //        {
        //            return LowEndMultiple;
        //        }
        //    }
        //    else // LowEndDifference == HighEndDifference
        //    {
        //        if (minutesInput > 0)
        //        {
        //            return HighEndMultiple;
        //        }

        //        if (minutesInput < 0)
        //        {
        //            return LowEndMultiple;
        //        }
        //    }

        //    return 0.0m;    //  a solution was not found
        //}

        ///// <summary>
        ///// Sets 6th character of Gridsquare based on DDM_LatMins and Remainder_Lat
        ///// </summary>
        //private void GetSixthGridsquareCharacter()
        //{   
        //    decimal latMinsLookupValue;
        //    // check remainder and zero it out if in 2-degree increments otherwise...
        //    //   ...remove all but the remaining single-degree increment
        //    if (LatDirection > 0)
        //    {
        //        latMinsLookupValue = -60m + DDMlatMinutes;
        //        latMinsLookupValue = GetNearestEvenMultiple(latMinsLookupValue, 1);

        //        if (LookupTablesHelper.GetTable6C2GLookup.TryGetValue(latMinsLookupValue, out string table6LookupResult))
        //        {
        //            Gridsquare.Append(table6LookupResult.ToLower(currentCulture));
        //        }
        //    }
        //    else if (LatDirection < 0)
        //    {
        //        latMinsLookupValue = LatDirection * DDMlatMinutes;

        //        if (latMinsLookupValue % 2.5m != 0)
        //        {
        //            latMinsLookupValue -= (latMinsLookupValue % 2.5m);
        //        }
        //        if (LookupTablesHelper.GetTable6C2GLookup.TryGetValue(latMinsLookupValue, out string table6LookupResult))
        //        {
        //            Gridsquare.Append(table6LookupResult.ToLower(currentCulture));
        //        }
        //    }
        //    else
        //    {
        //        Gridsquare.Append("?");
        //        throw new ArgumentOutOfRangeException($"Unable to determine character in method { System.Reflection.MethodBase.GetCurrentMethod().Name }");
        //    }
        //}

        ///// <summary>
        ///// Returns 5th Gridsquare character based on Remainder_Lon.
        ///// </summary>
        //private void GetFifthGridsquareCharacter()
        //{   
        //    decimal lonMinutesLookupValue;
        //    decimal remainderCorrectionValue = 0;

        //    if (RemainderLon > 1)
        //    {
        //        throw new ArgumentOutOfRangeException($"RemainderLon was > 1 in { System.Reflection.MethodBase.GetCurrentMethod().Name }");
        //    }
        //    else if (RemainderLon == 1)
        //    {
        //        remainderCorrectionValue = 60;    //  convert the remainder degrees to minutes
        //        RemainderLon = 0;               //  zero-out the remainder now it has been used
        //    }

        //    // use remainder from Table1Lookup and Table2Lookup, express it as Minutes and Add to actual Minutes
        //    if (LonDirection > 0)
        //    {
        //        lonMinutesLookupValue = 0 - 120 + DDMlonMinutes + remainderCorrectionValue;
        //        lonMinutesLookupValue = GetNearestEvenMultiple(lonMinutesLookupValue, 2);

        //        if (LookupTablesHelper.GetTable3C2GLookup.TryGetValue(lonMinutesLookupValue, out string table3LookupResult))
        //        {
        //            Gridsquare.Append(table3LookupResult.ToLower(currentCulture));
        //        }
        //    }
        //    else if (LonDirection < 0)
        //    {
        //        lonMinutesLookupValue = 0 - DDMlonMinutes - remainderCorrectionValue;

        //        if (lonMinutesLookupValue % 5 != 0)
        //        {
        //            lonMinutesLookupValue -= (lonMinutesLookupValue % 5);   //  move the pointer down the table to the next multiple of 5
        //        }
        //        if (LookupTablesHelper.GetTable3C2GLookup.TryGetValue((int)lonMinutesLookupValue, out string table3LookupResult))
        //        {
        //            Gridsquare.Append(table3LookupResult.ToLower(currentCulture));
        //        }
        //    }
        //    else
        //    {
        //        Gridsquare.Append("?");
        //    }
        //}

        ///// <summary>
        ///// Returns 4th GridSquare character based on Remainder_Lat and LatDirection.
        ///// </summary>
        //private void GetFourthGridsquareCharacter()
        //{   
        //    //  NOTE: Fourth Gridsquare character is arranged in single-digit increments therefore no remainder
        //    decimal latLookupValue; // = RemainderLat;

        //    if (LatDirection < 0)
        //    {
        //        latLookupValue = RemainderLat + 9;
        //    }
        //    else if (LatDirection > 0)
        //    {
        //        latLookupValue = RemainderLat;
        //    }
        //    else
        //    {
        //        throw new IndexOutOfRangeException($"The lookup value {RemainderLat} is not within the range of -10 to 9.");
        //    }

        //    RemainderLat = 0;
        //    Gridsquare.Append(latLookupValue.ToString(currentCulture));
        //}

        ///// <summary>
        ///// Returns 3rd GridSquare character based on LonDirection and Remainder_Lon.
        ///// </summary>
        //private void GetThirdGridsquareCharacter()
        //{   
        //    decimal calculationNumber;

        //    if (LonDirection < 0)
        //    {

        //        if (RemainderLon % 2 != 0)
        //        {
        //            calculationNumber = ((RemainderLon) + 21) / 2 - 1;
        //            RemainderLon = 1;   // used up max even portion of RemainderLon so odd single is left and must be accounted for in last grid character
        //        }
        //        else
        //        {
        //            calculationNumber = (18 + RemainderLon) / 2;
        //        }
        //    }
        //    else
        //    {
        //        calculationNumber = Math.Abs(RemainderLon) / 2;
        //    }

        //    if (RemainderLon % 2 != 0)  //  third character lookup is in 2-degree increments so an odd-number will result in a remainder of 1
        //    {
        //        RemainderLon = 1;
        //    }
        //    else
        //    {
        //        RemainderLon = 0;   //  decrement remainder
        //    }

        //    decimal lonDegreesCalculatedResult = Math.Truncate(calculationNumber);
        //    Gridsquare.Append(Math.Abs(lonDegreesCalculatedResult).ToString(currentCulture));
        //}

        ///// <summary>
        ///// Returns 2nd GridSquare character based on DDM_LatDegrees (whole number).
        ///// </summary>
        //private void GetSecondGridsquareCharacter()
        //{   
        //    decimal latDegreesLookupValue = DDMlatDegrees;

        //    if (DDMlatDegrees < 0)
        //    {
        //        LatDirection = -1;
        //    }
        //    else if (DDMlatDegrees > 0)
        //    {
        //        LatDirection = 1;
        //    }
        //    else
        //    {
        //        throw new IndexOutOfRangeException($"LatDirection ({ LatDirection }) could not be set for latDegreesLookupValue { latDegreesLookupValue }.");
        //    }

        //    RemainderLat = latDegreesLookupValue % 10;

        //    if (RemainderLat != 0)
        //    {
        //        latDegreesLookupValue -= RemainderLat;
        //    }

        //    if (LatDirection > 0)
        //    {

        //        if (LookupTablesHelper.GetTable4C2GLookupPositive.TryGetValue(latDegreesLookupValue, out string table4LookupResult))
        //        {
        //            Gridsquare.Append(table4LookupResult);
        //        }
        //    }
        //    else if (LatDirection < 0)
        //    {

        //        if (LookupTablesHelper.GetTable4C2GLookupNegative.TryGetValue(Math.Abs(latDegreesLookupValue) * LatDirection, out string table4LookupResult))
        //        {
        //            Gridsquare.Append(table4LookupResult);
        //        }
        //        else
        //        {
        //            Gridsquare.Append("?");
        //        }
        //    }
        //    else
        //    {
        //        Gridsquare.Append("?");
        //    }
        //}

        ///// <summary>
        ///// Returns the 1st GridSquare character based on DDM_LonDegrees (whole number).
        ///// </summary>
        //private void GetFirstGridsquareCharacter()
        //{
        //    // LonDirection MUST be set prior to entering this method
        //    decimal lonDegreesLookupValue;
        //    if (LonDirection != -1 && LonDirection != 1)
        //    {
        //        throw new IndexOutOfRangeException($"LonDirection ({LonDirection}) could not be set for lonDegreesLookupValue.");
        //    }

        //    Gridsquare.Clear();
        //    RemainderLon = DDMlonDegrees % 20;

        //    if (RemainderLon != 0)
        //    {
        //        lonDegreesLookupValue = DDMlonDegrees - RemainderLon;
        //    }
        //    else
        //    {
        //        lonDegreesLookupValue = DDMlonDegrees;
        //    }

        //    if (LonDirection < 0)
        //    {
        //        if (LookupTablesHelper.GetTable1C2GLookupNegative.TryGetValue(Math.Abs(lonDegreesLookupValue) * LonDirection, out string table1LookupResult))
        //        {
        //            Gridsquare.Append(table1LookupResult);
        //        }
        //    }
        //    else if (LonDirection > 0)
        //    {
        //        if (LookupTablesHelper.GetTable1C2GLookupPositive.TryGetValue(lonDegreesLookupValue, out string table1LookupResult))
        //        {
        //            Gridsquare.Append(table1LookupResult);
        //        }
        //        else
        //        {
        //            Gridsquare.Append("?");
        //        }
        //    }
        //    else
        //    {
        //        Gridsquare.Append("?");
        //    }
        //}

        ///// <summary>
        ///// Returns a string representation of the calculated GridSquare from a DMS Coordinate object.
        ///// </summary>
        ///// <param name="dmsLatAndLongCoordinates"></param>
        ///// <returns></returns>
        //public string ConvertDMStoGridsquare(DMSCoordinate dmsLatAndLongCoordinates)
        //{
        //    Contract.Requires(dmsLatAndLongCoordinates != null);

        //    if (dmsLatAndLongCoordinates != null)
        //    {
        //        var ddm = new DDMCoordinate(dmsLatAndLongCoordinates.DegreesLattitude, dmsLatAndLongCoordinates.MinutesLattitude,
        //            dmsLatAndLongCoordinates.SecondsLattitude, dmsLatAndLongCoordinates.DegreesLongitude,
        //            dmsLatAndLongCoordinates.MinutesLongitude, dmsLatAndLongCoordinates.SecondsLongitude);

        //        LatDirection = ConversionHelper.ExtractPolarityNS(ddm.ToString());
        //        LonDirection = ConversionHelper.ExtractPolarityEW(ddm.ToString());
        //        ConvertDDMtoGridsquare(DecimalDegreesMinutesCoordinate);
        //        return $"{ Gridsquare.ToString() }";
        //    }
        //    else
        //    {
        //        return string.Empty;    //  if input is empty output is empty
        //    }
        //}

    }
}
