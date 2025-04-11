namespace nguyenvanlai_2122110481_asp.net.Model
{
    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        // 🔗 Liên kết 1-nhiều
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
