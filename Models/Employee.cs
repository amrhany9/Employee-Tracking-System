using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullNameEn { get; set; }
        public string? FullNameAr { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserPhotoPath { get; set; }
        public bool IsCheckedIn { get; set; }
        public bool IsActive { get; set; }

        public Department Department { get; set; }
        public Account Account { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<AttendanceRequest> AttendanceRequests { get; set; }
    }
}
