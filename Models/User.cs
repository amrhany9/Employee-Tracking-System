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

        public Account UserAccount { get; set; }
        public IEnumerable<Attendance> UserAttendances { get; set; }
    }
}
