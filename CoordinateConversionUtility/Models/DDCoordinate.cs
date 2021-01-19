using System;

namespace CoordinateConversionUtility.Models
{
    /// <summary>
    /// Decimal Degrees coordinate class
    /// </summary>
    public class DDCoordinate : CoordinateBase
    {
        public DDCoordinate() { }

        public DDCoordinate(decimal lat, decimal lon)
        {
            if (-90m <= lat && lat <= 90m)
            {
                DegreesLattitude = lat;
            }
            else
            {
                DegreesLattitude = -91m;
            }

            if (-180m <= lon && lon <= 180m)
            {
                DegreesLongitude = lon;
            }
            else
            {
                DegreesLongitude = -181m;
            }
        }

        public int GetLatDegrees()
        {
            Decimal lat = Math.Truncate(DegreesLattitude);

            if (int.TryParse(lat.ToString(), out int result))
                {
                return result;
            }

            return -91;
        }

        public decimal GetLonDegrees()
        {
            Decimal lon = Math.Truncate(DegreesLongitude);

            if (int.TryParse(lon.ToString(), out int result))
            {
                return result;
            }

            return -181;
        }

        public decimal GetFractionalLattitude()
        {
            return DegreesLattitude - Math.Truncate(DegreesLattitude);
        }

        public decimal GetFractionalLongitude()
        {
            return DegreesLongitude - Math.Truncate(DegreesLongitude);
        }

        public virtual bool IsValid(string DDLatAndLon, out decimal ddlat, out decimal ddlon)
        { 
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

        public virtual bool IsValid(decimal lattitude, decimal longitude)
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

        public decimal GetShortDegreeslat()
        {
            return Math.Truncate(this.DegreesLattitude);
        }

        public decimal GetShortDegreesLon()
        {
            return Math.Truncate(this.DegreesLongitude);
        }

        public override string ToString()
        {
            return $"{ DegreesLattitude:f4}{ DegreesSymbol }, { DegreesLongitude:f4}{ DegreesSymbol }";
        }

    }
}
