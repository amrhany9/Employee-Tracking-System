namespace back_end.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }

        public ICollection<Account> Accounts { get; set; }
        public ICollection<RolePermission> Permissions { get; set; }
    }
}
