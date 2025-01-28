namespace back_end.DTOs
{
    public class CreateAccountDTO
    {
        public int userId { get; set; }
        public int employeeId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int roleId { get; set; }
    }
}
