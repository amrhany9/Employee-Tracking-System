using back_end.Constants.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int MachineCode { get; set; }
        public int EmployeeId { get; set; }
        public VerifyMode VerifyMode { get; set; }
        public CheckType CheckType { get; set; }
        public DateTime CheckDate { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public Machine Machine { get; set; }
        public Employee Employee { get; set; }
    } 
}
