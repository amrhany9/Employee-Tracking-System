using AutoMapper;
using back_end.DTOs;
using back_end.Models;

namespace back_end.Profiles
{
    public class AttendanceMappingProfile : Profile
    {
        public AttendanceMappingProfile()
        {
            CreateMap<Attendance, AttArchive>().ReverseMap();
        }
    }
}
