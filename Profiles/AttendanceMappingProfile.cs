using AutoMapper;

namespace back_end.Profiles
{
    public class AttendanceMappingProfile : Profile
    {
        public AttendanceMappingProfile()
        {
            CreateMap<Models.Attendance, Models.AttendanceArchive>().ReverseMap();
        }
    }
}
