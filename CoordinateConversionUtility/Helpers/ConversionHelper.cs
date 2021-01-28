using CoordinateConversionUtility.Models;
using System;
using System.Text;

namespace CoordinateConversionUtility
{
    public static class ConversionHelper
    {
        internal static int PrecisionLevel => 5;        //  Math.Round( deciman, PrecisionLevel )
        internal static char CommaSymbol => (char)44;    //  comma symbol
        internal static char DegreesSymbol => (char)176; //  degree symbol
        internal static char MinutesSymbol => (char)39;      //  single quote
        internal static char SecondsSymbol => (char)34;      //  double quote

        //  RULES
        //      1) Inputs must come in as the appropriate object e.g. DD as DDCoordinateHelper instance
        //      2) Outputs must always be one of DD, DDM, or DMS
        //      3) NSEW format will always be: DD - none; DDM - post; DMS - Pre

        public static DDCoordinate ToDD(DDMCoordinate ddm)
        {
            if (ddm == null)
            {
                return new DDCoordinate();
            }

            decimal ddDegreesLat = Math.Round( Math.Abs( ddm.DegreesLattitude) + ( ddm.MinutesLattitude / 60 ), PrecisionLevel );
            decimal ddDegreesLon = Math.Round( Math.Abs( ddm.DegreesLongitude) + ( ddm.MinutesLongitude / 60 ), PrecisionLevel );
            int latSign = ExtractPolarityNS(ddm.ToString());
            int lonSign = ExtractPolarityEW(ddm.ToString());
            ddDegreesLat *= latSign;
            ddDegreesLon *= lonSign;
            return new DDCoordinate(ddDegreesLat, ddDegreesLon);
        }

        public static DDCoordinate ToDD(DMSCoordinate dms)
        {
            if (dms == null)
            {
                return new DDCoordinate();
            }

            decimal ddDegreesLat = Math.Abs(dms.GetShortDegreesLat());
            decimal ddFractionLat = dms.GetShortMinutesLattitude() / 60;
            ddFractionLat += dms.GetSecondsLattitude() / 3600;
            ddDegreesLat += ddFractionLat;

            decimal ddDegreesLon = Math.Abs(dms.GetShortDegreesLon());
            decimal ddFractionLon = dms.GetShortMinutesLongitude() / 60;
            ddFractionLon += dms.GetSecondsLongitude() / 3600;
            ddDegreesLon += ddFractionLon;

            int latSign = ExtractPolarityNS(dms.ToString());
            int lonSign = ExtractPolarityEW(dms.ToString());
            ddDegreesLat *= latSign;
            ddDegreesLon *= lonSign;

            return new DDCoordinate(ddDegreesLat, ddDegreesLon);
        }

        public static DDMCoordinate ToDDM(DMSCoordinate dms)
        {
            if (dms == null)
            {
                return new DDMCoordinate();
            }

            decimal ddmMinutesLat = Math.Truncate(dms.MinutesLattitude);
            decimal ddmMinutesLon = Math.Truncate(dms.MinutesLongitude);

            ddmMinutesLat += dms.SecondsLattitude / 60;
            ddmMinutesLon += dms.SecondsLongitude / 60;

            decimal ddmDegreesLat = dms.GetShortDegreesLat();
            decimal ddmDegreesLon = dms.GetShortDegreesLon();

            return new DDMCoordinate(ddmDegreesLat, ddmMinutesLat, ddmDegreesLon, ddmMinutesLon);
        }

        public static DDMCoordinate ToDDM(DDCoordinate dd)
        {
            if (dd == null)
            {
                return new DDMCoordinate();
            }

            decimal dmLat = dd.GetFractionalLattitude() * 60;
            decimal dmLon = dd.GetFractionalLattitude() * 60;

            return new DDMCoordinate();
        }

        public static DMSCoordinate ToDMS(DDMCoordinate ddm)
        {
            if (ddm == null)
            {
                return new DMSCoordinate();
            }

            string NS = GetNSEW(ddm.DegreesLattitude, 1);
            string EW = GetNSEW(ddm.DegreesLongitude, 2);

            int latPolarity = ExtractPolarityNS(NS);
            int lonPolarity = ExtractPolarityEW(EW);

            decimal latDegrees = Math.Abs( ddm.GetShortDegreesLat() );
            decimal lonDegrees = Math.Abs( ddm.GetShortDegreesLon() );

            decimal latMinutes = Math.Round( ddm.MinutesLattitude / 60 , PrecisionLevel);
            decimal lonMinutes = Math.Round( ddm.MinutesLongitude / 60 , PrecisionLevel);

            decimal ddLattitude = (latDegrees + latMinutes) * latPolarity;
            decimal ddLongitude = (lonDegrees + lonMinutes) * lonPolarity;

            return new DMSCoordinate(ddLattitude, ddLongitude);
        }

        public static DMSCoordinate ToDMS(DDCoordinate dd)
        {
            if (dd == null)
            {
                return new DMSCoordinate(); ;
            }

            //string NS = GetNSEW(dd.DegreesLattitude, 1);
            //string EW = GetNSEW(dd.DegreesLongitude, 2);

            //decimal dmsDegreesLattitude = dd.GetShortDegreesLat();
            //decimal dmsDegreesLongitude = dd.GetShortDegreesLon();
            //decimal dmsMinutesLattitude = dd.GetFractionalLattitude();
            //decimal dmsMinutesLongitude = dd.GetFractionalLongitude();
            //decimal dmsSecondsLattitude = (dmsMinutesLattitude - Math.Truncate(dmsMinutesLattitude)) / 60;
            //decimal dmsSecondsLongitude = (dmsMinutesLongitude - Math.Truncate(dmsMinutesLongitude)) / 60;

            return new DMSCoordinate(dd.DegreesLattitude, dd.DegreesLongitude);
            //return $"{ NS } { Math.Abs(Math.Truncate(dd.DegreesLattitude)) }{ DegreesSymbol }" +
            //       $"{ Math.Truncate(ddmMinutesLattitude) }{ MinutesSymbol }" +
            //       $"{ Math.Truncate(GetSecondsLat(ddmMinutesLattitude)) }{ SecondsSymbol }, " +
            //       $"{ EW } { Math.Abs(Math.Truncate(dd.DegreesLongitude)) }{ DegreesSymbol }" +
            //       $"{ Math.Truncate(ddmMinutesLongitude) }{ MinutesSymbol }" +
            //       $"{ Math.Truncate(GetSecondsLon(ddmMinutesLongitude)) }{ SecondsSymbol }";
        }

        /// <summary>
        /// Returns a signed integer representing direction: 1 for N; -1 for S; 0 for bad input.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int ExtractPolarity(decimal number)
        {
            int result;

            try
            {
                result = (int)(number / number);
            }
            catch
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// Returns integer representing Decimal direction: 1 for N; -1 for S; 0 for bad input.
        /// Apply to absolute Longitude to transform from unsigned to signed value.
        /// </summary>
        /// <param name="ddmLattitude"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns integer representing Decimal direction: 1 for E; -1 for W; 0 for bad input.
        /// Apply to Absolute Lattitude to transform from unsigned to signed value.
        /// </summary>
        /// <param name="ddmLongitude"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns string N, S, E, or W. Use LatOrLon=1 for Lattitudes; LatOrLon=2 for Longitudes.
        /// </summary>
        /// <param name="degreesLatOrLon"></param>
        /// <param name="LatOrLon"></param>
        /// <returns></returns>
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
