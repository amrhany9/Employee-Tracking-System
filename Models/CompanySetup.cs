namespace back_end.Models
{
    public class CompanySetup
    {
        public int companyId { get; set; }
        public string companyNameEn { get; set; }
        public string? companyNameAr { get; set; }
        public int chairmanId { get; set; }
        public decimal companyLatitude { get; set; }
        public decimal companyLongitude { get; set; }

        public Employee chairman { get; set; }
    }
}
