using back_end.Models;

namespace back_end.Services.Location
{
    public class LocationService : ILocationService
    {
        private double _companyLatitude;
        private double _companyLongitude;

        public void setCompanyCoordinates(CompanySetup companySetup)
        {
            if (companySetup.companyLatitude < -90 || companySetup.companyLatitude > 90)
                throw new ArgumentOutOfRangeException(nameof(companySetup.companyLatitude), "Latitude must be between -90 and 90.");

            if (companySetup.companyLongitude < -180 || companySetup.companyLongitude > 180)
                throw new ArgumentOutOfRangeException(nameof(companySetup.companyLongitude), "Longitude must be between -180 and 180.");

            _companyLatitude = (double)companySetup.companyLatitude;
            _companyLongitude = (double)companySetup.companyLongitude;
        }

        public bool IsLocationSet()
        {
            return _companyLatitude != 0 && _companyLongitude != 0 ;
        }

        private double CalculateDistance(double UserLatitude, double UserLongitude)
        {
            var R = 6371000; // Radius of the Earth in meters
            var latRad1 = _companyLatitude * Math.PI / 180;
            var latRad2 = UserLatitude * Math.PI / 180;
            var deltaLat = (UserLatitude - _companyLatitude) * Math.PI / 180;
            var deltaLon = (UserLongitude - _companyLongitude) * Math.PI / 180;

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(latRad1) * Math.Cos(latRad2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Distance in meters
        }
    }
}
