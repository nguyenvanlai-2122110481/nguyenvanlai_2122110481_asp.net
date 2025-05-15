using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using nguyenvanlai_2122110481_asp.net.Data;
using nguyenvanlai_2122110481_asp.net.DTO;
using nguyenvanlai_2122110481_asp.net.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace nguyenvanlai_2122110481_asp.net.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Where(u => u.IsActive && u.DeletedAt == null)
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive && u.DeletedAt == null);

            if (user == null)
                return NotFound("User not found or deleted.");

            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
                Phone = dto.Phone,
                Address = dto.Address,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = dto.CreatedBy,
                UpdatedAt = DateTime.Now,
                UpdatedBy = dto.CreatedBy
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FindAsync(id);
            if (user == null || user.DeletedAt != null)
                return NotFound("User not found or deleted.");

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.Address = dto.Address;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = dto.UpdatedBy;

            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, [FromBody] UserDeleteDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.DeletedAt != null)
                return NotFound("User not found or already deleted.");

            user.DeletedAt = DateTime.Now;
            user.DeletedBy = dto.DeletedBy;
            user.IsActive = false;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"User '{user.FullName}' has been soft-deleted successfully.",
                deletedAt = user.DeletedAt,
                deletedBy = user.DeletedBy
            });
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password && u.DeletedAt == null);

            if (user == null)
                return Unauthorized("Invalid credentials.");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = jwt,
                user = new { user.Id, user.FullName, user.Email }
            });
        }
    }
}
