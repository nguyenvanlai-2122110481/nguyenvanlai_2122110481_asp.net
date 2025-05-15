
using System.ComponentModel.DataAnnotations;

namespace nguyenvanlai_2122110481_asp.net.DTO
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; } = true;
        public IFormFile? ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }

        // Có thể có thêm các trường khác nếu cần
    }
}
