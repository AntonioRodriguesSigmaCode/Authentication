using System.Security.Claims;
using System.Text;
using Authentication.Data;
using Authentication.Model;
using Authentication.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<Utilizador, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");

var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.ConfigureApplicationCookie(options =>
{
	options.Events = new CookieAuthenticationEvents
	{
		OnValidatePrincipal = async context =>
		{
			var userManager = context.HttpContext.RequestServices
				.GetRequiredService<UserManager<Utilizador>>();

			var user = await userManager.GetUserAsync(context.Principal!);

			if (user?.SessionToken != null)
			{
				var sessionToken = context.HttpContext.Session.GetString("SessionToken");

				if (sessionToken == null || sessionToken != user.SessionToken)
				{
					context.RejectPrincipal();
					await context.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
				}
			}
		}
	};
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFront",
		policy => policy.WithOrigins("http://127.0.0.1:5500")
						.AllowAnyHeader()
						.AllowAnyMethod());
});

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddSession(options =>
{
	options.Cookie.IsEssential = true;
	options.Cookie.MaxAge = null;
});

var app = builder.Build();
app.UseSession(); 

app.UseCors("AllowFront");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllers();

app.Run();