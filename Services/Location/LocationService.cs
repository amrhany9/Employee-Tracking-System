using back_end.Models;

namespace back_end.Services.Location
{
    public class LocationService : ILocationService
    {
        private double _companyLatitude;
        private double _companyLongitude;

        public void setCompanyCoordinates(CompanySetup companySetup)
        {
            if (companySetup.Latitude < -90 || companySetup.Latitude > 90)
                throw new ArgumentOutOfRangeException(nameof(companySetup.Latitude), "Latitude must be between -90 and 90.");

            if (companySetup.Longitude < -180 || companySetup.Longitude > 180)
                throw new ArgumentOutOfRangeException(nameof(companySetup.Longitude), "Longitude must be between -180 and 180.");

            _companyLatitude = (double)companySetup.Latitude;
            _companyLongitude = (double)companySetup.Longitude;
        }

        public bool IsLocationSet()
        {
            return _companyLatitude != 0 && _companyLongitude != 0 ;
        }

        public double CalculateDistance(double UserLatitude, double UserLongitude)
        {
            var R = 6371000;
            var latRad1 = _companyLatitude * Math.PI / 180;
            var latRad2 = UserLatitude * Math.PI / 180;
            var deltaLat = (UserLatitude - _companyLatitude) * Math.PI / 180;
            var deltaLon = (UserLongitude - _companyLongitude) * Math.PI / 180;

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(latRad1) * Math.Cos(latRad2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }
    }
}
