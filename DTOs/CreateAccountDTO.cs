namespace back_end.DTOs
{
    public class CreateAccountDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public String Role { get; set; }
    }
}
