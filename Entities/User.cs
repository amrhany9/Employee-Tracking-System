using System.ComponentModel.DataAnnotations;

namespace back_end.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameAr { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserPhotoPath { get; set; }

        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<UserAttendanceRecord> AttendanceRecords { get; set; } = new HashSet<UserAttendanceRecord>();
    }
}
