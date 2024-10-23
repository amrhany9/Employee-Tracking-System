namespace back_end.Interfaces
{
    public interface ILocationService
    {
        void SetCompanyCoordinates(double companyLatitude, double companyLongitude);
        void SetAllowedRadius(double allowedRadius);
        bool IsWithinCompanyArea(double UserLatitude, double UserLongitude);
    }
}
