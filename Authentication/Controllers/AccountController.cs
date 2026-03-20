using System.Security.Claims;
using Authentication.Dto.User;
using Authentication.Model;
using Authentication.Service;
using Authentication.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<Utilizador> _signInManager;
		private readonly IAuthService _authService;
		public AccountController(IAuthService authService, SignInManager<Utilizador> signInManager)
		{
			_authService = authService;
			_signInManager = signInManager; 
		}

		[HttpGet]
		public IActionResult Login()
		{
			if (User.Identity!.IsAuthenticated)
				return View("SessionAlreadyOn");

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			Console.WriteLine($"Email: {model.Email}");
			Console.WriteLine($"Password: {model.Password}");
			Console.WriteLine($"RememberMe: {model.RememberMe}");
			Console.WriteLine($"ModelState válido: {ModelState.IsValid}");

			foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
				Console.WriteLine($"Erro: {error.ErrorMessage}");

			if (!ModelState.IsValid)
				return View(model);

			var result = await _signInManager.PasswordSignInAsync(
				model.Email,
				model.Password,
				isPersistent: model.RememberMe,
				lockoutOnFailure: false);

			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Email ou Senha incorreta");
				return View(model);
			}

			var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);

			if (!model.RememberMe)
			{
				var sessionToken = Guid.NewGuid().ToString();
				user!.SessionToken = sessionToken;
				await _signInManager.UserManager.UpdateAsync(user);

				HttpContext.Session.SetString("SessionToken", sessionToken);
			}
			else
			{
				user!.SessionToken = null;
				await _signInManager.UserManager.UpdateAsync(user);
			}
			 
			return RedirectToAction("PaginaInicial", "Account");
		}

		[HttpGet]
		public IActionResult Register()
		{
			if (User.Identity!.IsAuthenticated)
				return View("SessionAlreadyOn");
			return View();
		}

		[HttpPost]
		public  async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = new Utilizador
			{
				UserName = model.Email,
				Email = model.Email,
				RefreshToken = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32)),
				RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
			};

			var result = await _signInManager.UserManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
					ModelState.AddModelError("", error.Description);
				return View(model);
			}

			await _signInManager.SignInAsync(user, isPersistent: false);
			return RedirectToAction("PaginaInicial", "Account");
		}

		[Authorize]
		[HttpGet]
		public IActionResult PaginaInicial()
		{
			return View();
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Dashboard()
		{
			var user = await _signInManager.UserManager.GetUserAsync(User);
			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login", "Account");
		}
	}
}
