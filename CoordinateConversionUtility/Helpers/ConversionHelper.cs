using System;

namespace CoordinateConversionUtility
{
    public static class ConversionHelper
    {
        //  static class requires very few properties
        private static char DegreesSymbol => (char)176;      //  degree symbol
        private static char MinutesSymbol => (char)39;       //  single quote
        private static char SecondsSymbol => (char)34;       //  double quote

        //  RULES
        //      1) Inputs must come in as the appropriate object e.g. DD as DDCoordinateHelper instance
        //      2) Outputs must always be string
        //      3) NSEW format will always be: DD - none; DDM - post; DMS - Pre

        public static decimal ConvertMinsToDD(decimal degrees, decimal minutes)
        {
            //  return the SUM of Longitude DDM Degres and DDM Minutes e.x.: 112*18.25 => 112.3041
            return degrees + (minutes / 60);
        }
        public static decimal ConvertSecondsToDM(decimal minutes, decimal seconds)
        {
            //  generate the SUM of DMS Minutes and DMS Seconds e.x.: 45'30" => 45.50
            return minutes + (seconds / 60);
        }
        //  convert from DDM to DD
        public static string ToDD(DDMCoordinateHelper ddm)
        {
            //  TODO: Deal with signed decimal in calculation
            decimal ddDegreesLat = ddm.DegreesLat + ( ddm.MinutesLat / 60 );
            decimal ddDegreesLon = ddm.DegreesLat + ( ddm.MinutesLon / 60 );
            int latSign = ExtractPolarityNSEW(ddm.ToString());
            int lonSign = ExtractPolarityNSEW(ddm.ToString());
            ddDegreesLat *= latSign;
            ddDegreesLon *= lonSign;
            return $"{ ddDegreesLat }{ DegreesSymbol }, " +
                   $"{ ddDegreesLon }{ DegreesSymbol }";
        }
        //  convert from DMS to DD
        public static string ToDD(DMSCoordinateHelper dms)
        {
            //  TODO: Deal with signed decimal in calculation
            decimal ddmMinutesLat = Math.Truncate(dms.MinutesLat);
            ddmMinutesLat += (dms.MinutesLat - Math.Truncate(dms.MinutesLat)) / 60;
            decimal ddmMinutesLon = Math.Truncate(dms.MinutesLon);
            ddmMinutesLon += (dms.MinutesLon - Math.Truncate(dms.MinutesLon)) / 60;
            decimal ddDegreesLat = dms.DegreesLat + (ddmMinutesLat / 60);
            decimal ddDegreesLon = dms.DegreesLon + (ddmMinutesLon / 60);
            int latSign = ExtractPolarityNSEW(dms.ToString());
            int lonSign = ExtractPolarityNSEW(dms.ToString());
            ddDegreesLat *= latSign;
            ddDegreesLon *= lonSign;
            return $"{ ddDegreesLat }{ DegreesSymbol }, " +
                   $"{ ddDegreesLon }{ DegreesSymbol }";
        }

        //  convert from DMS to DDM
        public static string ToDDM(DMSCoordinateHelper dms)
        {
            //  TODO: Fix invalid minutes output
            //  TODO: Fix inconsistend NESW directionals
            string NS = GetNSEW(Math.Truncate(dms.DegreesLat), 1);
            string EW = GetNSEW(Math.Truncate(dms.DegreesLon), 2);
            decimal ddmMinutesLat = Math.Truncate(dms.MinutesLat);
            ddmMinutesLat += (dms.MinutesLat - Math.Truncate(dms.MinutesLat)) / 60;
            decimal ddmMinutesLon = Math.Truncate(dms.MinutesLon);
            ddmMinutesLon += (dms.MinutesLon - Math.Truncate(dms.MinutesLon)) / 60;

            return $"{ Math.Abs(Math.Truncate(dms.DegreesLat)) }{ DegreesSymbol }" +
                   $"{ ddmMinutesLat:f2}{ MinutesSymbol } { NS }, " +
                   $"{ Math.Abs(Math.Truncate(dms.DegreesLon)) }{ DegreesSymbol }" +
                   $"{ ddmMinutesLon:f2}{ MinutesSymbol } { EW }";
        }

        //  convert from DD to DDM
        public static string ToDDM(DDCoordindateHelper dd)
        {
            string NS = GetNSEW(Math.Truncate(dd.DegreesLat), 1);
            string EW = GetNSEW(Math.Truncate(dd.DegreesLon), 2);
            return $"{ Math.Abs(Math.Truncate(dd.DegreesLat)) }{ DegreesSymbol }" +
                   $"{ GetMinutesLat(dd.DegreesLat):f2}{ MinutesSymbol } { NS }, " +
                   $"{ Math.Abs(Math.Truncate(dd.DegreesLon)) }{ DegreesSymbol }" +
                   $"{ GetMinutesLon(dd.DegreesLon):f2}{ MinutesSymbol } { EW }";
        }

        //  convert from DDM to DMS
        public static string ToDMS(DDMCoordinateHelper ddm)
        {
            //  TODO: Fix invalid Minutes and Seconds output
            //  DDM properties are both in Degrees and Minutes
            string NS = GetNSEW(ddm.DegreesLat, 1);
            string EW = GetNSEW(ddm.DegreesLon, 1);
            return $"{ NS } { Math.Abs(Math.Truncate(ddm.DegreesLat)) }{ DegreesSymbol }" +
                   $"{ Math.Truncate(ddm.MinutesLat) }{ MinutesSymbol }" +
                   $"{ Math.Truncate(GetSecondsLat(ddm.MinutesLat)) }{ SecondsSymbol }, " +
                   $"{ EW } { Math.Abs(Math.Truncate(ddm.DegreesLon)) }{ DegreesSymbol }" +
                   $"{ Math.Truncate(ddm.MinutesLon) }{ MinutesSymbol }" +
                   $"{ Math.Truncate(GetSecondsLon(ddm.MinutesLon)) }{ SecondsSymbol }";
        }
        //  convert from DD to DMS
        public static string ToDMS(DDCoordindateHelper dd)
        {
            //  DD properties are only in signed Degrees
            string NS = GetNSEW(dd.DegreesLat, 1);
            string EW = GetNSEW(dd.DegreesLon, 2);
            decimal ddmMinutesLattitude = GetMinutes(dd.DegreesLat);
            decimal ddmMinutesLongitude = GetMinutes(dd.DegreesLon);
            return $"{ NS } { Math.Abs(Math.Truncate(dd.DegreesLat)) }{ DegreesSymbol }" +
                   $"{ Math.Truncate(ddmMinutesLattitude) }{ MinutesSymbol }" +
                   $"{ Math.Truncate(GetSecondsLat(ddmMinutesLattitude)) }{ SecondsSymbol }, " +
                   $"{ EW } { Math.Abs(Math.Truncate(dd.DegreesLon)) }{ DegreesSymbol }" +
                   $"{ Math.Truncate(ddmMinutesLongitude) }{ MinutesSymbol }" +
                   $"{ Math.Truncate(GetSecondsLon(ddmMinutesLongitude)) }{ SecondsSymbol }";
        }

        //  used by some or all output methods
        private static int ExtractPolarityNSEW(string strDdmLatOrLon)
        {
            int nsew = 1;
            if (strDdmLatOrLon.IndexOf('S') > -1)
            {
                nsew = -1;
            }
            else if (strDdmLatOrLon.IndexOf('W') > -1)
            {
                nsew = -1;
            }
            return nsew;
        }
        private static decimal GetMinutes(decimal degreesLatOrLon)
        {
            return (Math.Abs(degreesLatOrLon) - Math.Truncate(Math.Abs(degreesLatOrLon))) * 60;
        }
        public static decimal GetMinutesLat(decimal ddDegreesLattitude)
        {
            return (Math.Abs(ddDegreesLattitude) - Math.Truncate(Math.Abs(ddDegreesLattitude))) * 60;
        }
        public static decimal GetMinutesLon(decimal ddDegreesLongitude)
        {
            return (Math.Abs(ddDegreesLongitude) - Math.Truncate(Math.Abs(ddDegreesLongitude))) * 60;
        }
        private static decimal GetSeconds(decimal minutesLatOrLon)
        {
            decimal truncMinutes = Math.Abs(Math.Truncate(minutesLatOrLon));
            decimal absMinutes = Math.Abs(minutesLatOrLon);
            decimal fractionalMinutes = absMinutes - truncMinutes;
            decimal result = 60 * fractionalMinutes;
            return result;
        }
        public static decimal GetSecondsLat(decimal ddMinutesLattitude)
        {
            decimal truncMinutes = Math.Abs(Math.Truncate(ddMinutesLattitude));
            decimal absMinutes = Math.Abs(ddMinutesLattitude);
            decimal fractionalMinutes = absMinutes - truncMinutes;
            decimal result = 60 * fractionalMinutes;
            return result;
        }
        public static decimal GetSecondsLon(decimal ddMinutesLongitude)
        {
            decimal truncMinutes = Math.Abs(Math.Truncate(ddMinutesLongitude));
            decimal absMinutes = Math.Abs(ddMinutesLongitude);
            decimal fractionalMinutes = absMinutes - truncMinutes;
            decimal result = 60 * fractionalMinutes;
            return result;
        }
        private static string GetNSEW(decimal degreesLatOrLon, int LatOrLon)
        {   //  lat = 1; lon = 2
            switch (LatOrLon)
            {
                case 1:
                    {
                        string NS = "N";
                        if (degreesLatOrLon < 0)
                        {
                            NS = "S";
                        }
                        return NS;
                    }
                case 2:
                    {
                        string EW = "E";
                        if (degreesLatOrLon < 0)
                        {
                            EW = "W";
                        }
                        return EW;
                    }
                default:
                    return string.Empty;
            }
        }
    }
}
