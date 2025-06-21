namespace back_end.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string? NameAr { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
