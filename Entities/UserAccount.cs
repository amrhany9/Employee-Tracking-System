using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Entities
{
    public class UserAccount
    {
        public int UserAccountId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Admin" or "Employee"
        public bool IsCheckedIn { get; set; }

        public virtual User User { get; set; }
    }
}
