namespace back_end.Services.Location
{
    public interface ILocationService
    {
        void SetCompanyCoordinates(double companyLatitude, double companyLongitude);
        void SetAllowedRadius(double allowedRadius);
        bool IsWithinCompanyArea(double UserLatitude, double UserLongitude);
        bool IsLocationSet();
    }
}
