using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Models
{
    public class Account
    {
        public int accountId { get; set; }
        public int employeeId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string? refreshToken { get; set; }
        public DateTime? refreshTokenExpiry { get; set; }
        public int roleId { get; set; }

        public Role role { get; set; }
        public Employee employee { get; set; }
    }
}
