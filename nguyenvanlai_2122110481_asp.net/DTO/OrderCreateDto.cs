namespace nguyenvanlai_2122110481_asp.net.DTO
{
    public class OrderCreateDto
    {
        public int UserId { get; set; }
        public string Status { get; set; } = "Pending"; // Mặc định là "Pending"
        public DateTime OrderDate { get; set; } = DateTime.Now; // Mặc định ngày tạo là hiện tại
        public List<OrderDetailDto> OrderDetails { get; set; }
    }

    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
