namespace nguyenvanlai_2122110481_asp.net.DTO
{
    public class UserCreateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string CreatedBy { get; set; } = "Admin";
    }
}
