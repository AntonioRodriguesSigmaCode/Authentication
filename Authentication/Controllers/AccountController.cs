using Authentication.Models;
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
		private readonly SignInManager<ApplicationUser> _signInManager;
		public AccountController(SignInManager<ApplicationUser> signInManager)
		{
			_signInManager = signInManager; 
		}

		[HttpGet]
		public IActionResult Login()
		{
			if (User.Identity!.IsAuthenticated)
			{
				TempData["AlreadyLoggedIn"] = "Já tens uma sessão iniciada.";
				return RedirectToAction("PaginaInicial");
			}

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var result = await _signInManager.PasswordSignInAsync(
				model.Email,
				model.Password,
				isPersistent: model.RememberMe,
				lockoutOnFailure: true);

			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Email ou Senha incorreta");
				return View(model);
			}

			return RedirectToAction("PaginaInicial", "Account");
		}

		[HttpGet]
		public IActionResult Register()
		{
			if (User.Identity!.IsAuthenticated)
			{
				TempData["AlreadyLoggedIn"] = "Já tens uma sessão iniciada.";
				return RedirectToAction("PaginaInicial");
			}
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email
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
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login", "Account");
		}

	}
}
