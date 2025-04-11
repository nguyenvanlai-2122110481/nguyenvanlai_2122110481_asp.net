using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nguyenvanlai_2122110481_asp.net.Data;
using nguyenvanlai_2122110481_asp.net.DTO;
using nguyenvanlai_2122110481_asp.net.Model;
using System.Linq;

namespace nguyenvanlai_2122110481_asp.net.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả sản phẩm
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // Lấy sản phẩm theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found.");
            return Ok(product);
        }

        // Thêm sản phẩm
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Name))
                return BadRequest("Invalid product data.");

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.Now,
                CreatedBy = "Admin",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "Admin"
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // Cập nhật sản phẩm
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid product data.");

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found.");

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.UpdatedAt = DateTime.Now;
            product.UpdatedBy = string.IsNullOrEmpty(dto.UpdatedBy) ? "Admin" : dto.UpdatedBy;

            await _context.SaveChangesAsync();

            return Ok(product);
        }

        // Xóa sản phẩm
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id, [FromBody] ProductDeleteDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found.");

            product.DeletedAt = DateTime.Now;
            product.DeletedBy = string.IsNullOrEmpty(dto.DeletedBy) ? "Admin" : dto.DeletedBy;

            _context.Products.Remove(product); // Xóa cứng
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Product '{product.Name}' has been deleted successfully.",
                deletedAt = product.DeletedAt,
                deletedBy = product.DeletedBy,
                data = product
            });
        }
    }
}