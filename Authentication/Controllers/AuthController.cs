using Authentication.Interface;
using Authentication.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Authentication.Dto.Token;
using Authentication.Dto.User;
using Authentication.Model;
using Authentication.Service;

namespace Authentication.Controllers
{
	[Route("api/auth-controller")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IUserRepository _repo;

		public AuthController(IAuthService authService, IUserRepository repo)
		{
			_authService = authService;
			_repo = repo;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(UserRegisterDto request)
		{
			var user = await _authService.RegisterAsync(request);

			if (user is null)
				return BadRequest("Username já existe");

			return Ok(user);
		}

		[HttpPost("login")]	
		public async Task<IActionResult> Login(UserLoginDto request)
		{
			var result = await _authService.LoginAsync(request);

			if (result == null)
				return BadRequest("Username ou password errado");

			return Ok(result);
		}

		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
		{
			var result = await _authService.RefreshTokenAsync(request);
			if (result == null || result.AcessToken == null || result.RefreshToken == null)
			{
				return BadRequest("Refresh token invalido.");
			}
			return Ok(result);
		}



		
		
	}
}
