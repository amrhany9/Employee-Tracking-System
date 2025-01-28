using AutoMapper;
using back_end.Models;
using back_end.ViewModels.CompanySetup;
using back_end.ViewModels.Machine;

namespace back_end.Profiles
{
    public class MachineMappingProfile : Profile
    {
        public MachineMappingProfile()
        {
            CreateMap<MachineViewModel, Machine>();
        }
    }
}
