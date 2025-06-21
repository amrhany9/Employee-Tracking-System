namespace back_end.Models
{
    public class Machine
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }

        public ICollection<Attendance> Attendances { get; set; }
    }
}
