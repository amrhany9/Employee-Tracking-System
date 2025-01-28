using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
    public class Employee
    {
        public int employeeId { get; set; }
        public string fullNameEn { get; set; }
        public string? fullNameAr { get; set; }
        public int departmentId { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string userPhotoPath { get; set; }
        public bool isCheckedIn { get; set; }
        public bool isActive { get; set; }

        public Department department { get; set; }
        public Account account { get; set; }
        public ICollection<Attendance> attendances { get; set; }
        public ICollection<AttendanceRequest> attendanceRequests { get; set; }
    }
}
