using Authentication.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Authentication.Service;
using Authentication.Dto.User;
using Microsoft.AspNetCore.Identity;
using Authentication.Model;

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
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

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
			return View();
		}

		[HttpPost]
		public  async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = new Utilizador { UserName = model.Email, Email = model.Email };
			var result = await _signInManager.UserManager.CreateAsync(user, model.Password);

			if(!result.Succeeded)
			{
				foreach (var error in result.Errors)
					ModelState.AddModelError("", error.Description);
				return View(model);
			}

			await _signInManager.SignInAsync(user, isPersistent: false);
			return RedirectToAction("PaginaInicial", "Account");
		}

		[HttpGet]
		public IActionResult PaginaInicial()
		{
			return View();
		}
	}
}
