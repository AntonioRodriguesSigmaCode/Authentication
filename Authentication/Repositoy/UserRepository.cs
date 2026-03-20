using Authentication.Data;
using Authentication.Interface;
using Authentication.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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
			//var existe = await _context.Permissoes.FirstOrDefaultAsync(x => x.Nome == nomePermissao);

			//if (existe == null)
				return null;

			var permissao = new Permissao { Nome = nomePermissao };
			//_context.Permissoes.Add(permissao);
			//await _context.SaveChangesAsync();

			return permissao;
		}
	}
}
