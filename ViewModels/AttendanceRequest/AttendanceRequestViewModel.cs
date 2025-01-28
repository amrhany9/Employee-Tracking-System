using back_end.Constants.Enums;

namespace back_end.ViewModels.AttendanceRequest
{
    public class AttendanceRequestViewModel
    {
        public int accountId { get; set; }
        public CheckType checkType { get; set; }
        public DateTime checkDate { get; set; }
        public Decimal latitude { get; set; }
        public Decimal longitude { get; set; }
        public string description { get; set; }
    }
}
