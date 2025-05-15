using Microsoft.AspNetCore.Mvc.ModelBinding;
using nguyenvanlai_2122110481_asp.net.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    public decimal? Discount { get; set; }  // Giảm giá (%)

    public int StockQuantity { get; set; } = 0;

    public bool IsAvailable { get; set; } = true;

    [StringLength(100)]
    public string? Brand { get; set; }

    public int? WarrantyMonths { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public Categories Category { get; set; }

    // Quan hệ với User
    [Required]
    [JsonIgnore]   // Không trả về trường UserId khi serialize thành JSON
    [BindNever]    // Không cho phép bind từ request
    public int UserId { get; set; }
    public User User { get; set; }

    // Audit fields
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}
