namespace nguyenvanlai_2122110481_asp.net.DTO
{
    public class UserUpdateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string UpdatedBy { get; set; } = "Admin";
    }
}
