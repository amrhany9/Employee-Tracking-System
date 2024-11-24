using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Models
{
    public class Attendance : BaseModel
    {
        public int UserId { get; set; }
        public int VerifyMode { get; set; } // 0: "Finger Print" or 1: "Password" or 2: "Tracker App"
        public int CheckType { get; set; } // 0: "CheckIn" or 1: "CheckOut"
        public DateTime CheckDate { get; set; }

        public User User { get; set; }
    } 
}
