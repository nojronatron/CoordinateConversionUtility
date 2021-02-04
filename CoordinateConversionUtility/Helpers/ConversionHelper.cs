using System;
using System.Text;

namespace CoordinateConversionUtility
{
    /// <summary>
    /// Static Helper class.
    /// Responsible for providing basic validation and state information for Coordinate objects and portions thereof.
    /// </summary>
    public static class ConversionHelper
    {
        public static int ExtractPolarity(decimal number)
        {
            if (number > 0)
            {
                return 1;
            }

            if (number < 0)
            {
                return -1;
            }

            return 0;
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
            else if (strDdmLatOrLon.IndexOf('S') > -1)
            {
                nsew.Append("S");
            }

            if (strDdmLatOrLon.IndexOf('E') > -1)
            {
                nsew.Append("E");
            }
            else if (strDdmLatOrLon.IndexOf('W') > -1)
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

        public static bool IsValid(decimal lattitude, decimal longitude)
        {

            if (!LatDecimalIsValid(lattitude))
            {
                return false;
            }

            if (!LonDecimalIsValid(longitude))
            {
                return false;
            }

            return true;
        }

        private static bool LatDecimalIsValid(decimal lattitudeDecimal)
        {
            if (-90 <= lattitudeDecimal && lattitudeDecimal <= 90)
            {
                return true;
            }

            return false;
        }

        private static bool LonDecimalIsValid(decimal longitudeDecimal)
        {
            if (-180 <= longitudeDecimal && longitudeDecimal <= 180)
            {
                return true;
            }

            return false;
        }

    }
}
