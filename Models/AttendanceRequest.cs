using back_end.Constants.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Models
{
    public class AttendanceRequest
    {
        public int requestId { get; set; }
        public int employeeId { get; set; }
        public CheckType checkType { get; set; }
        public DateTime checkDate { get; set; }
        public Decimal latitude { get; set; }
        public Decimal longitude { get; set; }
        public string description { get; set; }
        public RequestStatus status { get; set; }

        public Employee employee { get; set; }
    } 
}
