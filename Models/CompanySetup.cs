namespace back_end.Models
{
    public class CompanySetup
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string? NameAr { get; set; }
        public int ChairmanId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public Employee chairman { get; set; }
    }
}
