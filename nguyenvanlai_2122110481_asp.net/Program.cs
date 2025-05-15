using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using nguyenvanlai_2122110481_asp.net.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Định nghĩa tên scheme tùy chỉnh
const string CustomScheme = "CustomJwtBearer";

// Thêm dịch vụ vào container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Thêm Authentication với scheme tùy chỉnh
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CustomScheme;
    options.DefaultChallengeScheme = CustomScheme;
})
.AddJwtBearer(CustomScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],  // Thay đổi theo cấu hình của bạn
        ValidAudience = builder.Configuration["Jwt:Audience"],  // Thay đổi theo cấu hình của bạn
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),  // Thay đổi theo cấu hình của bạn
        ClockSkew = TimeSpan.Zero
    };
});

// Cấu hình Authorization (nếu cần)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes(CustomScheme);  // Sử dụng scheme tùy chỉnh
        policy.RequireRole("Admin");
    });
});

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Cho phép tất cả các origin
              .AllowAnyMethod()  // Cho phép tất cả các phương thức (GET, POST, PUT, DELETE, ...)
              .AllowAnyHeader(); // Cho phép tất cả các header
    });
});

// Kết nối với cơ sở dữ liệu
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Thêm Swagger để kiểm tra API
builder.Services.AddSwaggerGen();

// Thêm dòng này để tránh lỗi khi chưa cấu hình Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Cấu hình pipeline cho HTTP requests
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Đảm bảo gọi UseAuthentication trước UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

// Cấu hình CORS
app.UseCors("AllowAll");

// Map các controller
app.MapControllers();

app.UseStaticFiles(); // Cho phép truy cập file trong wwwroot


app.Run();
