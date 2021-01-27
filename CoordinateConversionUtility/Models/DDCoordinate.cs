using System;

namespace CoordinateConversionUtility.Models
{
    /// <summary>
    /// Decimal Degrees coordinate class
    /// </summary>
    public class DDCoordinate : CoordinateBase
    {
        public DDCoordinate() { }

        public DDCoordinate(decimal lattitude, decimal longitude)
        {
            if (-90m <= lattitude && lattitude <= 90m)
            {
                DegreesLattitude = lattitude;
            }
            else
            {
                DegreesLattitude = -91m;
            }

            if (-180m <= longitude && longitude <= 180m)
            {
                DegreesLongitude = longitude;
            }
            else
            {
                DegreesLongitude = -181m;
            }
        }

        public DDCoordinate(decimal ddmLatDegrees, decimal ddmLatMins, decimal ddmLonDegrees, decimal ddmLonMins)
        {
            DegreesLattitude = ddmLatDegrees + (ddmLatMins / 60);
            DegreesLongitude = ddmLonDegrees + (ddmLonMins / 60);
        }

        public DDCoordinate(decimal dmsLatDegrees, decimal dmsLatMinutes, decimal dmsLatSeconds,
            decimal dmsLonDegrees, decimal dmsLonMinutes, decimal dmsLonSeconds)
        {
            DegreesLattitude = dmsLatDegrees + (dmsLatMinutes / 60) + (dmsLatSeconds / 3600);
            DegreesLongitude = dmsLonDegrees + (dmsLonMinutes / 60) + (dmsLonSeconds / 3600);
        }

        public DDCoordinate(string ddLatAndLon)
        {
            if (string.IsNullOrEmpty(ddLatAndLon) || string.IsNullOrWhiteSpace(ddLatAndLon))   //  check for null
            {
                DegreesLattitude = 0.0m;
                DegreesLongitude = 0.0m;

                throw new ArgumentNullException(nameof(ddLatAndLon));
            }

            string[] splitLatAndLon = ddLatAndLon.Split(CommaSymbol);
            string ddLat = splitLatAndLon[0];
            string ddLon = splitLatAndLon[1];

            ddLat = ddLat.Trim();
            int degreeIDX = ddLat.IndexOf(DegreesSymbol);

            string temp = ddLat.Substring(0, degreeIDX).Trim(trimChars).Trim();

            if (decimal.TryParse(temp, out decimal decLatDegrees))
            {
                DegreesLattitude = decLatDegrees;
            }

            int north = 1;

            if (temp.IndexOf((char)45) > -1)
            {
                north = -1;
            }

            degreeIDX = ddLon.IndexOf(DegreesSymbol);
            temp = ddLon.Substring(0, degreeIDX);
            temp = temp.Trim(trimChars);

            if (decimal.TryParse(temp, out decimal decLonDegrees))
            {
                DegreesLongitude = decLonDegrees;
            }

            int east = 1;

            if (temp.IndexOf((char)45) > -1)
            {
                east = -1;
            }

            DegreesLattitude *= north;
            DegreesLongitude *= east;
        }

        public decimal GetLattitudeDD()
        {
            return DegreesLattitude;
        }

        public decimal GetLongitudeDD()
        {
            return DegreesLongitude;
        }

        public decimal GetShortDegreesLat()
        {
            return Math.Truncate(this.DegreesLattitude);
        }

        public decimal GetShortDegreesLon()
        {
            return Math.Truncate(this.DegreesLongitude);
        }

        public decimal GetFractionalLattitude()
        {
            return DegreesLattitude - Math.Truncate(DegreesLattitude);
        }

        public decimal GetFractionalLongitude()
        {
            return DegreesLongitude - Math.Truncate(DegreesLongitude);
        }

        //public virtual bool IsValid(string DDLatAndLon, out decimal ddlat, out decimal ddlon)
        //{ 
        //    decimal latDeciTemp = -91m, lonDeciTemp = -181m;

        //    if (DDLatAndLon != null)
        //    {
        //        string lat = DDLatAndLon.Split(',')[0];
        //        string lon = DDLatAndLon.Split(',')[1];

        //        if (decimal.TryParse(lat, out decimal latDeci))
        //        {
        //            latDeciTemp = latDeci;

        //            if (!LatDecimalIsValid(latDeciTemp))
        //            {
        //                ddlat = latDeciTemp;
        //                ddlon = lonDeciTemp;
        //                return false;
        //            }
        //        }

        //        if (decimal.TryParse(lon, out decimal lonDeci))
        //        {
        //            lonDeciTemp = lonDeci;

        //            if (!LonDecimalIsValid(lonDeciTemp))
        //            {
        //                ddlat = latDeciTemp;
        //                ddlon = lonDeciTemp;
        //                return false;
        //            }
        //        }
        //    }

        //    ddlat = latDeciTemp;
        //    ddlon = lonDeciTemp;
        //    return false;
        //}

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

        public override string ToString()
        {
            return $"{ DegreesLattitude:f4}{ DegreesSymbol }, { DegreesLongitude:f4}{ DegreesSymbol }";
        }

    }
}
