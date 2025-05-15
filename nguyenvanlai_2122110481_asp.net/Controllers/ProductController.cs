using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nguyenvanlai_2122110481_asp.net.Data;
using nguyenvanlai_2122110481_asp.net.DTO;
using nguyenvanlai_2122110481_asp.net.Model;

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
            var products = await _context.Products
                .Include(p => p.Category)  // Bao gồm thông tin Category
                .Include(p => p.User)      // Bao gồm thông tin User
                .ToListAsync();
            return Ok(products);
        }

        // Lấy sản phẩm theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)  // Bao gồm thông tin Category
                .Include(p => p.User)      // Bao gồm thông tin User
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound("Product not found.");
            return Ok(product);
        }

        // Thêm sản phẩm
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ⚠️ Xử lý lưu ảnh
            string imagePath = null;
            if (dto.ImageUrl != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ImageUrl.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageUrl.CopyToAsync(stream);
                }

                imagePath = $"/uploads/{fileName}";
            }

            // ⚙️ Tạo đối tượng Product để lưu DB
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                IsAvailable = dto.IsAvailable,
                CategoryId = dto.CategoryId,
                ImageUrl = imagePath,
                CreatedBy="k",
                UserId=1,
                UpdatedBy="k",
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }
        // Cập nhật sản phẩm
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ⚠️ Kiểm tra tồn tại sản phẩm
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Sản phẩm không tồn tại.");

            // ⚠️ Xử lý lưu ảnh mới (nếu có)
            string imagePath = product.ImageUrl; // Giữ ảnh cũ nếu không có ảnh mới
            if (dto.ImageUrl != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.ImageUrl.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageUrl.CopyToAsync(stream);
                }

                imagePath = $"/uploads/{fileName}"; // Cập nhật ảnh mới
            }

            // ⚙️ Cập nhật thông tin sản phẩm
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.IsAvailable = dto.IsAvailable;
            product.CategoryId = dto.CategoryId;
            product.ImageUrl = imagePath;
            product.UpdatedBy = "k";  // Hoặc lấy từ session/user thực tế

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        // Xóa sản phẩm
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found.");

            product.DeletedAt = DateTime.Now;
            product.DeletedBy = "Admin"; // Cải thiện nếu muốn lấy từ HttpContext.User

            _context.Products.Remove(product);  // Xóa cứng
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
