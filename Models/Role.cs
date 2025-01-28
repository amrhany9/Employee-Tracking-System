namespace back_end.Models
{
    public class Role
    {
        public int roleId { get; set; }
        public string roleNameEn { get; set; }
        public string roleNameAr { get; set; }

        public ICollection<Account> accounts { get; set; }
    }
}
