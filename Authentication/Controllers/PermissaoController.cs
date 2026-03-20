using Authentication.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
	[Route("api/permissao-controller")]
	[ApiController]
	public class PermissaoController: ControllerBase
	{
		private readonly IPermissaoRepository _repo;

		public PermissaoController(IPermissaoRepository repo)
		{
			_repo = repo;
		}

		[HttpPost("criar-permissao")]
		public async Task<IActionResult> CreatePermissao(string nomePermissao)
		{
			var permissao = await _repo.CreatePermissaoAsync(nomePermissao);

			if (permissao == null)
				return BadRequest("Já existe uma permissao com esse nome");

			return Ok(permissao);
		}

		[HttpPost("atribuir-role")]
		public async Task<IActionResult> AtribuirRole(int  roleId, int permissaoId)
		{
			var role = await _repo.AtribuirPermissaoAsync(roleId, permissaoId);

			if (role == false)
				return BadRequest("Não existe permissao ou role");

			return Ok(role);
		}

	}
}
