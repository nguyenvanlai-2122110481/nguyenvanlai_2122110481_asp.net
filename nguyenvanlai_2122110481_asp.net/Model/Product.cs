
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace nguyenvanlai_2122110481_asp.net.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        [BindNever]
        [JsonIgnore] // Thêm cái này để không serialize ngược lại khi trả JSON
        public Categories Category { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
