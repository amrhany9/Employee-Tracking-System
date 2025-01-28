using back_end.Models;

namespace back_end.Services.Location
{
    public interface ILocationService
    {
        void setCompanyCoordinates(CompanySetup companySetup);
        bool IsLocationSet();
    }
}
