namespace back_end.Models
{
    public class Machine
    {
        public int machineCode { get; set; }
        public string machineName { get; set; }
        public string machineIp { get; set; }
        public int machinePort { get; set; }

        public ICollection<Attendance> attendances { get; set; }
    }
}
