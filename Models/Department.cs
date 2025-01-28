namespace back_end.Models
{
    public class Department
    {
        public int departmentId { get; set; }
        public string departmentNameEn { get; set; }
        public string? departmentNameAr { get; set; }

        public ICollection<Employee> employees { get; set; }
    }
}
