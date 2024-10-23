namespace back_end.DTOs
{
    public class RegisterDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Admin" or "Employee"
    }
}
