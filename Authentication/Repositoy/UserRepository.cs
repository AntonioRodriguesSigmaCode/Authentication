using Authentication.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using projetoAPI.Data;
using projetoAPI.Model;

namespace Authentication.Repositoy
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _context;

		public UserRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Permissao> CreatePermissaoAsync(string nomePermissao)
		{
			var existe = await _context.Permissoes.FirstOrDefaultAsync(x => x.Nome == nomePermissao);

			if (existe == null)
				return null;

			var permissao = new Permissao { Nome = nomePermissao };
			_context.Permissoes.Add(permissao);
			await _context.SaveChangesAsync();

			return permissao;
		}
	}
}
