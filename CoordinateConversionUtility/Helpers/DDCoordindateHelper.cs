using System;

namespace CoordinateConversionUtility
{
    public class DDCoordindateHelper
    {   //  helper class to validate and instantiate proper DecimalDegree coordinates
        internal decimal DegreesLat { get; private set; }
        internal decimal DegreesLon { get; private set; }
        private static char DegreesSymbol => (char)176;      //  degree symbol
        public DDCoordindateHelper() { }
        public DDCoordindateHelper(decimal lat, decimal lon)
        {
            DegreesLat = lat;
            DegreesLon = lon;
        }
        public decimal GetLatDegrees()
        {
            return DegreesLat;
        }
        public decimal GetLonDegrees()
        {
            return DegreesLon;
        }
        public static bool IsValid(string DDLatAndLon, out decimal ddlat, out decimal ddlon)
        {   //  e.g. CoordinateConverter.IsValid("47.8058,-122.2516")
            decimal latDeciTemp = -91m, lonDeciTemp = -181m;
            if (DDLatAndLon != null)
            {
                string lat = DDLatAndLon.Split(',')[0];
                string lon = DDLatAndLon.Split(',')[1];

                if (decimal.TryParse(lat, out decimal latDeci))
                {
                    latDeciTemp = latDeci;
                    if (!LatDecimalIsValid(latDeciTemp))
                    {
                        ddlat = latDeciTemp;
                        ddlon = lonDeciTemp;
                        return false;
                    }
                }
                if (decimal.TryParse(lon, out decimal lonDeci))
                {
                    lonDeciTemp = lonDeci;
                    if (!LonDecimalIsValid(lonDeciTemp))
                    {
                        ddlat = latDeciTemp;
                        ddlon = lonDeciTemp;
                        return false;
                    }
                }
            }
            ddlat = latDeciTemp;
            ddlon = lonDeciTemp;
            return false;
        }
        public static bool IsValid(decimal lattitude, decimal longitude)
        {   //  e.g.: CoordinateConverter.IsValid(47.8058m, -122.2516m)

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
        {   //  return boolean true unless lattitude is out of bounds then return false
            if (-90 <= lattitudeDecimal && lattitudeDecimal <= 90)
            {
                return true;
            }
            return false;
        }
        private static bool LonDecimalIsValid(decimal longitudeDecimal)
        {   //  return boolean true unless longitude is out of bounds then return false
            if (-180 <= longitudeDecimal && longitudeDecimal <= 180)
            {
                return true;
            }
            return false;
        }
        public decimal GetShortDegreeslat()
        {
            return Math.Truncate(this.DegreesLat);
        }
        public decimal GetShortDegreesLon()
        {
            return Math.Truncate(this.DegreesLon);
        }
        public override string ToString()
        {
            return $"{ DegreesLat:f4}{ DegreesSymbol },{ DegreesLon:f4}{ DegreesSymbol }";
        }
    }
}
