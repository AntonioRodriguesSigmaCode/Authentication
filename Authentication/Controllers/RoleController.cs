using Authentication.Interface;
using Authentication.Mappers;
using Microsoft.AspNetCore.Mvc;
using projetoAPI.Service;



namespace Authentication.Controllers
{
	[Route("api/role-controller")]
	[ApiController]
	public class RoleController: ControllerBase
	{
		private readonly IRoleRepository _repo;

		public RoleController( IRoleRepository repo)
		{
			_repo = repo;
		}

		[HttpPost("create-role")]
		public async Task<IActionResult> CreateRole(string nome)
		{
			var role = await _repo.CreateRoleAsync(nome);

			if (role == null)
				return BadRequest("Já existe uma role com esse nome");

			return Ok(role);
		}
		[HttpPost("atribuir-role")]
		public async Task<IActionResult> AtribuirRole(int userId, int roleId)
		{
			var user = await _repo.AtribuirRoleAsync(userId, roleId);

			if (user == null)
				return BadRequest("Não existe utilizador ou role");

			return Ok(user.ToUserDto());
		}

		[HttpPost("getAll")]
		public async Task<IActionResult> GetAll()
		{
			var roles = await _repo.GetAllAsync();
			var rolesDto = roles.Select(s => s.MapToDto());

			return Ok(rolesDto);
		}
	}
}
