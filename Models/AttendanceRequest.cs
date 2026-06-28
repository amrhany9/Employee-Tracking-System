using back_end.Constants.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Models
{
    public class AttendanceRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public CheckType CheckType { get; set; }
        public DateTime CheckDate { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
        public string Description { get; set; }
        public RequestStatus Status { get; set; }

        public Employee Employee { get; set; }
    } 
}
