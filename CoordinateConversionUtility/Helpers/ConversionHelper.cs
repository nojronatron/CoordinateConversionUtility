using CoordinateConversionUtility.Models;
using System;
using System.Text;

namespace CoordinateConversionUtility
{
    public static class ConversionHelper
    {
        internal static char CommaSymbol => (char)44;    //  comma symbol
        internal static char DegreesSymbol => (char)176; //  degree symbol
        internal static char MinutesSymbol => (char)39;      //  single quote
        internal static char SecondsSymbol => (char)34;      //  double quote

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
        public static string ToDD(DDMCoordinate ddm)
        {
            if (ddm == null)
            {
                return string.Empty;
            }

            //  TODO: Deal with signed decimal in calculation
            decimal ddDegreesLat = ddm.DegreesLattitude + ( ddm.MinutesLattitude / 60 );
            decimal ddDegreesLon = ddm.DegreesLattitude + ( ddm.MinutesLongitude / 60 );
            int latSign = ExtractPolarityNS(ddm.ToString());
            int lonSign = ExtractPolarityEW(ddm.ToString());
            ddDegreesLat *= latSign;
            ddDegreesLon *= lonSign;
            return $"{ ddDegreesLat }{ DegreesSymbol }, " +
                   $"{ ddDegreesLon }{ DegreesSymbol }";
        }

        //  convert from DMS to DD
        public static string ToDD(DMSCoordinate dms)
        {
            if (dms == null)
            {
                return string.Empty;
            }

            //  TODO: Deal with signed decimal in calculation
            decimal ddmMinutesLat = Math.Truncate(dms.MinutesLattitude);
            ddmMinutesLat += (dms.MinutesLattitude - Math.Truncate(dms.MinutesLattitude)) / 60;
            decimal ddmMinutesLon = Math.Truncate(dms.MinutesLongitude);
            ddmMinutesLon += (dms.MinutesLongitude - Math.Truncate(dms.MinutesLongitude)) / 60;
            decimal ddDegreesLat = dms.DegreesLattitude + (ddmMinutesLat / 60);
            decimal ddDegreesLon = dms.DegreesLongitude + (ddmMinutesLon / 60);
            int latSign = ExtractPolarityNS(dms.ToString());
            int lonSign = ExtractPolarityEW(dms.ToString());
            ddDegreesLat *= latSign;
            ddDegreesLon *= lonSign;
            return $"{ ddDegreesLat }{ DegreesSymbol }, " +
                   $"{ ddDegreesLon }{ DegreesSymbol }";
        }

        //  convert from DMS to DDM
        public static string ToDDM(DMSCoordinate dms)
        {
            if (dms == null)
            {
                return string.Empty;
            }

            //  TODO: Fix invalid minutes output
            //  TODO: Fix inconsistend NESW directionals
            string NS = GetNSEW(Math.Truncate(dms.DegreesLattitude), 1);
            string EW = GetNSEW(Math.Truncate(dms.DegreesLongitude), 2);
            decimal ddmMinutesLat = Math.Truncate(dms.MinutesLattitude);
            ddmMinutesLat += (dms.MinutesLattitude - Math.Truncate(dms.MinutesLattitude)) / 60;
            decimal ddmMinutesLon = Math.Truncate(dms.MinutesLongitude);
            ddmMinutesLon += (dms.MinutesLongitude - Math.Truncate(dms.MinutesLongitude)) / 60;

            return $"{ Math.Abs(Math.Truncate(dms.DegreesLattitude)) }{ DegreesSymbol }" +
                   $"{ ddmMinutesLat:f2}{ MinutesSymbol } { NS }, " +
                   $"{ Math.Abs(Math.Truncate(dms.DegreesLongitude)) }{ DegreesSymbol }" +
                   $"{ ddmMinutesLon:f2}{ MinutesSymbol } { EW }";
        }

        //  convert from DD to DDM
        public static string ToDDM(DDCoordinate dd)
        {
            if (dd == null)
            {
                return string.Empty;
            }

            string NS = GetNSEW(Math.Truncate(dd.DegreesLattitude), 1);
            string EW = GetNSEW(Math.Truncate(dd.DegreesLongitude), 2);
            return $"{ Math.Abs(Math.Truncate(dd.DegreesLattitude)) }{ DegreesSymbol }" +
                   $"{ GetMinutesLat(dd.DegreesLattitude):f2}{ MinutesSymbol } { NS }, " +
                   $"{ Math.Abs(Math.Truncate(dd.DegreesLongitude)) }{ DegreesSymbol }" +
                   $"{ GetMinutesLon(dd.DegreesLongitude):f2}{ MinutesSymbol } { EW }";
        }

        //  convert from DDM to DMS
        public static string ToDMS(DDMCoordinate ddm)
        {
            if (ddm == null)
            {
                return string.Empty;
            }
            //  TODO: Fix invalid Minutes and Seconds output
            //  DDM properties are both in Degrees and Minutes
            string NS = GetNSEW(ddm.DegreesLattitude, 1);
            string EW = GetNSEW(ddm.DegreesLongitude, 1);
            return $"{ NS } { Math.Abs(Math.Truncate(ddm.DegreesLattitude)) }{ DegreesSymbol }" +
                   $"{ Math.Truncate(ddm.MinutesLattitude) }{ MinutesSymbol }" +
                   $"{ Math.Truncate(GetSecondsLat(ddm.MinutesLattitude)) }{ SecondsSymbol }, " +
                   $"{ EW } { Math.Abs(Math.Truncate(ddm.DegreesLongitude)) }{ DegreesSymbol }" +
                   $"{ Math.Truncate(ddm.MinutesLongitude) }{ MinutesSymbol }" +
                   $"{ Math.Truncate(GetSecondsLon(ddm.MinutesLongitude)) }{ SecondsSymbol }";
        }
        //  convert from DD to DMS
        public static string ToDMS(DDCoordinate dd)
        {
            if (dd == null)
            {
                return string.Empty;
            }

            //  DD properties are only in signed Degrees
            string NS = GetNSEW(dd.DegreesLattitude, 1);
            string EW = GetNSEW(dd.DegreesLongitude, 2);
            decimal ddmMinutesLattitude = GetMinutes(dd.DegreesLattitude);
            decimal ddmMinutesLongitude = GetMinutes(dd.DegreesLongitude);
            return $"{ NS } { Math.Abs(Math.Truncate(dd.DegreesLattitude)) }{ DegreesSymbol }" +
                   $"{ Math.Truncate(ddmMinutesLattitude) }{ MinutesSymbol }" +
                   $"{ Math.Truncate(GetSecondsLat(ddmMinutesLattitude)) }{ SecondsSymbol }, " +
                   $"{ EW } { Math.Abs(Math.Truncate(dd.DegreesLongitude)) }{ DegreesSymbol }" +
                   $"{ Math.Truncate(ddmMinutesLongitude) }{ MinutesSymbol }" +
                   $"{ Math.Truncate(GetSecondsLon(ddmMinutesLongitude)) }{ SecondsSymbol }";
        }

        //  used by some or all output methods
        public static int ExtractPolarityNS(string ddmLattitude)
        {
            if (string.IsNullOrEmpty(ddmLattitude) != true)
            {

                if (ddmLattitude.IndexOf('S') > -1)
                {
                    return -1;
                }

                if (ddmLattitude.IndexOf('N') > -1)
                {
                    return 1;
                }
            }

            return 0;
        }

        public static int ExtractPolarityEW(string ddmLongitude)
        {
            if (string.IsNullOrEmpty(ddmLongitude) != true)
            {

                if (ddmLongitude.IndexOf('W') > -1)
                {
                    return -1;
                }

                if (ddmLongitude.IndexOf('E') > -1)
                {
                    return 1;
                }
            }

            return 0;
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

        public static string GetNSEW(string strDdmLatOrLon)
        {
            if (string.IsNullOrEmpty(strDdmLatOrLon))
            {
                return string.Empty;
            }

            StringBuilder nsew = new StringBuilder(2);

            if (strDdmLatOrLon.IndexOf('N') > -1)
            {
                nsew.Append("N");
            }
            else
            {
                nsew.Append("S");
            }

            if (strDdmLatOrLon.IndexOf('E') > -1)
            {
                nsew.Append("E");
            }
            else
            {
                nsew.Append("W");
            }

            return nsew.ToString();
        }

        public static string GetNSEW(decimal degreesLatOrLon, int LatOrLon)
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
                    {
                        return string.Empty;
                    }
            }
        }
        
        public static DDCoordinate StringToDD(string ddLatAndLon)
        {
            if (ddLatAndLon is null)   //  check for null
            {
                throw new ArgumentNullException(nameof(ddLatAndLon));
            }
            //  Split string into DegreesLat and DegreesLon
            string[] strDdmLatAndLon = ddLatAndLon.Replace(DegreesSymbol, ' ').Split(CommaSymbol);
            decimal degreesLatTemp = -91m;
            decimal degreesLonTemp = -181m;

            //  TryParse degrees into decimal format
            if (decimal.TryParse(strDdmLatAndLon[0], out decimal decLatDegrees))
            {
                degreesLatTemp = decLatDegrees;
            }
            if (decimal.TryParse(strDdmLatAndLon[1], out decimal decLonDegrees))
            {
                degreesLonTemp = decLonDegrees;
            }

            //  Store as a DDCoordinate if both are set within specified ranges
            Decimal degreesLat = 0m;
            Decimal degreesLon = 0m;

            if (-90m <= degreesLatTemp && degreesLatTemp <= 90m)
            {
                degreesLat = degreesLatTemp;
            }

            if (-180m <= degreesLonTemp && degreesLonTemp <= 180m)
            {
                degreesLon = degreesLonTemp;
            }

            return new DDCoordinate(degreesLat, degreesLon);
        }
    }
}
