using System.ComponentModel.DataAnnotations;

namespace nguyenvanlai_2122110481_asp.net.DTO
{
    public class ProductUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string UpdatedBy { get; set; }
    }
}
