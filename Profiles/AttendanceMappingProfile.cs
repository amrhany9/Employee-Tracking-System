using AutoMapper;
using back_end.DTOs;
using back_end.Models;
using back_end.ViewModels.AttendanceRequest;

namespace back_end.Profiles
{
    public class AttendanceMappingProfile : Profile
    {
        public AttendanceMappingProfile()
        {
            CreateMap<AttendanceRequestCreateViewModel, AttendanceRequest>().ReverseMap();

            CreateMap<AttendanceRequest, Attendance>();
        }
    }
}
