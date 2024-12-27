namespace back_end.Models
{
    public class AttMachine : BaseModel
    {
        public int MachineCode { get; set; }
        public string MachineName { get; set; }
        public string MachineIP { get; set; }
        public int MachinePort { get; set; }
    }
}
