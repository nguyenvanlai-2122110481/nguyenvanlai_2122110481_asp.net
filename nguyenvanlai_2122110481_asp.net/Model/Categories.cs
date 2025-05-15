using System.Text.Json.Serialization;

namespace nguyenvanlai_2122110481_asp.net.Model
{
    public class Categories
    {
        public int Id { get; set; }                     // Khóa chính
        public string Name { get; set; }                // Tên danh mục
        public string Description { get; set; }         // Mô tả

        public string? ImageUrl { get; set; }           // Ảnh minh họa (nếu có)

        public bool IsActive { get; set; } = true;      // Trạng thái hoạt động

        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Thời gian tạo
        public string CreatedBy { get; set; }           // Người tạo

        public DateTime UpdatedAt { get; set; } = DateTime.Now;  // Thời gian cập nhật gần nhất
        public string UpdatedBy { get; set; }           // Người cập nhật gần nhất

        public DateTime? DeletedAt { get; set; }        // Thời gian xoá mềm (nếu có)
        public string? DeletedBy { get; set; }          // Người xoá (nếu có)

        // 🔗 Quan hệ 1 - nhiều: 1 Category có nhiều Product
        [JsonIgnore]
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
