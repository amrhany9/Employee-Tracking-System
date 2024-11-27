using back_end.Constants.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Models
{
    public class Attendance : BaseModel
    {
        public int UserId { get; set; }
        public VerifyMode VerifyMode { get; set; }
        public CheckType CheckType { get; set; }
        public DateTime CheckDate { get; set; }

        public User User { get; set; }
    } 
}
