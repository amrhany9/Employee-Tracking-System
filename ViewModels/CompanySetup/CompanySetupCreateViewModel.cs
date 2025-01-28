namespace back_end.ViewModels.CompanySetup
{
    public class CompanySetupCreateViewModel
    {
        public string companyNameEn { get; set; }
        public string? companyNameAr { get; set; }
        public int chairmanId { get; set; }
        public decimal companyLatitude { get; set; }
        public decimal companyLongitude { get; set; }
    }
}
