namespace back_end.Services.Location
{
    public class LocationService : ILocationService
    {
        public double companyLatitude;
        public double companyLongitude;
        public double allowedRadius; // in meters

        public void SetCompanyCoordinates(double companyLatitude, double companyLongitude)
        {
            this.companyLatitude = companyLatitude;
            this.companyLongitude = companyLongitude;
        }

        public void SetAllowedRadius(double allowedRadius)
        {
            this.allowedRadius = allowedRadius;
        }

        public bool IsWithinCompanyArea(double UserLatitude, double UserLongitude)
        {
            var distance = CalculateDistance(UserLatitude, UserLongitude);
            return distance <= allowedRadius;
        }

        private double CalculateDistance(double UserLatitude, double UserLongitude)
        {
            var R = 6371000; // Radius of the Earth in meters
            var latRad1 = companyLatitude * Math.PI / 180;
            var latRad2 = UserLatitude * Math.PI / 180;
            var deltaLat = (UserLatitude - companyLatitude) * Math.PI / 180;
            var deltaLon = (UserLongitude - companyLongitude) * Math.PI / 180;

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(latRad1) * Math.Cos(latRad2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Distance in meters
        }
    }
}
