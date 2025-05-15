using System.ComponentModel.DataAnnotations;

namespace nguyenvanlai_2122110481_asp.net.DTO
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}
