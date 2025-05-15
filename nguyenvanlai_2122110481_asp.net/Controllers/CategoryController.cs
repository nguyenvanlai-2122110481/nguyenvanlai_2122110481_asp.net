using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nguyenvanlai_2122110481_asp.net.Data;
using nguyenvanlai_2122110481_asp.net.Model;
using nguyenvanlai_2122110481_asp.net.DTO;

namespace nguyenvanlai_2122110481_asp.net.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả category (chỉ hiển thị các mục đang hoạt động)
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive && c.DeletedAt == null)
                .ToListAsync();
            return Ok(categories);
        }

        // Lấy category theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null || !category.IsActive || category.DeletedAt != null)
                return NotFound("Category not found or has been deleted.");
            return Ok(category);
        }

        // Thêm mới category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Name))
                return BadRequest("Invalid data");

            var category = new Categories
            {
                Name = dto.Name,
                Description ="k",
                ImageUrl = "k",
                IsActive = true, // Sử dụng giá trị IsActive từ DTO
                CreatedAt = DateTime.Now,
                CreatedBy = "Admin",
                UpdatedAt = DateTime.Now,
                UpdatedBy ="Admin" ,
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // Cập nhật category
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null || !category.IsActive || category.DeletedAt != null)
                return NotFound("Category not found or has been deleted.");

            category.Name= dto.Name;    
            category.UpdatedAt = DateTime.Now;
            category.UpdatedBy = "Admin";

            await _context.SaveChangesAsync();

            return Ok(category);
        }

        // Xoá mềm category
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Danh mục không tồn tại.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok("Danh mục đã được xoá khỏi cơ sở dữ liệu.");
        }
    }
}
