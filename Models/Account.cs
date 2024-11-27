using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Models
{
    public class Account : BaseModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public String Role { get; set; } // "Admin" or "Employee"
        public bool IsCheckedIn { get; set; }

        public User User { get; set; }
    }
}
