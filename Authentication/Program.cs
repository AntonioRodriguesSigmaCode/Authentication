using System.Text;
using Authentication.Interface;
using Authentication.Repositoy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using projetoAPI.Data;
using projetoAPI.Service;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// -----------------------
// 1️⃣ DbContext
// -----------------------
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -----------------------
// 2️⃣ JWT Config
// -----------------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),

		ValidateIssuer = true,
		ValidIssuer = jwtSettings["Issuer"],

		ValidateAudience = true,
		ValidAudience = jwtSettings["Audience"],

		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero
	};
});

// -----------------------
// 3️⃣ Services
// -----------------------
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFront",
		policy => policy.WithOrigins("http://127.0.0.1:5500")
						.AllowAnyHeader()
						.AllowAnyMethod());
});

builder.Services.AddControllers();

// -----------------------
// 4️⃣ OpenAPI + Scalar 🚀
// -----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(); // 🔥 substitui Swagger

var app = builder.Build();

app.UseCors("AllowFront");

// -----------------------
// 5️⃣ Middleware
// -----------------------
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();           
	app.MapScalarApiReference(); 
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();