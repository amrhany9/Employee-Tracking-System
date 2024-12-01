namespace back_end.Services.Location
{
    public class LocationService : ILocationService
    {
        private double _companyLatitude;
        private double _companyLongitude;
        private double _allowedRadius; // in meters

        public void SetCompanyCoordinates(double companyLatitude, double companyLongitude)
        {
            if (companyLatitude < -90 || companyLatitude > 90)
                throw new ArgumentOutOfRangeException(nameof(companyLatitude), "Latitude must be between -90 and 90.");

            if (companyLongitude < -180 || companyLongitude > 180)
                throw new ArgumentOutOfRangeException(nameof(companyLongitude), "Longitude must be between -180 and 180.");

            _companyLatitude = companyLatitude;
            _companyLongitude = companyLongitude;
        }

        public void SetAllowedRadius(double allowedRadius)
        {
            if (allowedRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(allowedRadius), "Radius must be greater than 0.");
            
            _allowedRadius = allowedRadius;
        }

        public bool IsLocationSet()
        {
            return _companyLatitude != 0 && _companyLongitude != 0 && _allowedRadius > 0;
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
