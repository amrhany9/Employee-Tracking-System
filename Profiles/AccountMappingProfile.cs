using AutoMapper;
using back_end.DTOs;
using back_end.Models;

namespace back_end.Profiles
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<CreateAccountDTO, Account>().ReverseMap();
        }
    }
}
