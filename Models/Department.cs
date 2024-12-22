namespace back_end.Models
{
    public class Department : BaseModel
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
