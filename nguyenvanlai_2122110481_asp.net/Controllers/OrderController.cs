using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nguyenvanlai_2122110481_asp.net.Data;
using nguyenvanlai_2122110481_asp.net.DTO;
using nguyenvanlai_2122110481_asp.net.Model;

namespace nguyenvanlai_2122110481_asp.net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.Now,
                Status = dto.Status ?? "Pending", // Mặc định là "Pending" nếu không truyền
                OrderDetails = dto.OrderDetails.Select(od => new OrderDetail
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Lưu order và orderdetails vào cơ sở dữ liệu

            return Ok(new { message = "Order created successfully", orderId = order.Id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User) // Bao gồm thông tin User
                .Include(o => o.OrderDetails) // Bao gồm thông tin OrderDetails
                .ThenInclude(od => od.Product) // Bao gồm thông tin Product của từng OrderDetail
                .FirstOrDefaultAsync(o => o.Id == id); // Tìm kiếm order theo ID

            if (order == null) return NotFound(); // Nếu không tìm thấy thì trả về lỗi 404

            return Ok(order); // Trả về thông tin đơn hàng
        }
    }
}
