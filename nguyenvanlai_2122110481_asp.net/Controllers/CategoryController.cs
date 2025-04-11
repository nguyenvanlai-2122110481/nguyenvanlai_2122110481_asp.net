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

        // Lấy tất cả category
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        // Lấy category theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound("Category not found.");
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
                Description = dto.Description,
                CreatedAt = DateTime.Now,
                CreatedBy = "Admin",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "Admin"
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // Cập nhật category
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryCreateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound("Category not found.");

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.UpdatedAt = DateTime.Now;
            category.UpdatedBy = "Admin";

            await _context.SaveChangesAsync();

            return Ok(category);
        }

        // Xóa category
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id, [FromBody] CategoryDeleteDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound("Category not found.");

            category.DeletedAt = DateTime.Now;
            category.DeletedBy = string.IsNullOrEmpty(dto.DeletedBy) ? "Admin" : dto.DeletedBy;

            _context.Categories.Remove(category); // Hoặc soft-delete
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Category '{category.Name}' has been deleted successfully.",
                deletedAt = category.DeletedAt,
                deletedBy = category.DeletedBy,
                data = category
            });
        }

    }
}
