using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
    public class User : BaseModel
    {
        public string FullNameEn { get; set; }
        public string FullNameAr { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserPhotoPath { get; set; }

        public virtual Account UserAccount { get; set; }
        public virtual ICollection<Attendance> UserAttendances { get; set; } = new HashSet<Attendance>();
    }
}
