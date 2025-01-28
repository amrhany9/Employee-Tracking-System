using AutoMapper;
using back_end.Models;
using back_end.ViewModels.CompanySetup;

namespace back_end.Profiles
{
    public class CompanySetupMappingProfile : Profile
    {
        public CompanySetupMappingProfile()
        {
            CreateMap<CompanySetupUpdateViewModel, CompanySetup>();

            CreateMap<CompanySetupCreateViewModel, CompanySetup>();
        }
    }
}
