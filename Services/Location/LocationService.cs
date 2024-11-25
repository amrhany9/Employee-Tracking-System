namespace back_end.Services.Location
{
    public class LocationService : ILocationService
    {
        private double _companyLatitude;
        private double _companyLongitude;
        private double _allowedRadius; // in meters

        public void SetCompanyCoordinates(double companyLatitude, double companyLongitude)
        {
            _companyLatitude = companyLatitude;
            _companyLongitude = companyLongitude;
        }

        public void SetAllowedRadius(double allowedRadius)
        {
            _allowedRadius = allowedRadius;
        }

        public bool IsWithinCompanyArea(double UserLatitude, double UserLongitude)
        {
            var distance = CalculateDistance(UserLatitude, UserLongitude);
            return distance <= _allowedRadius;
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
