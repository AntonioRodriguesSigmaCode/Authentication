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
		//public async Task<UtilizadorRole> AtribuirRoleAsync(int userId, int roleId)
		/*{
			var existe = await _context.UtilizadorRole
				.AnyAsync(rp => rp.UtilizadorId == userId && rp.RoleId == roleId);

			if (existe)
				return null;

			var utilizadorRole = new UtilizadorRole
			{
				UtilizadorId = userId,
				RoleId = roleId
			};

			_context.UtilizadorRole.Add(utilizadorRole);
			await _context.SaveChangesAsync();

			return utilizadorRole;
		}*/

		public async Task<Role?> CreateRoleAsync(string nomeRole)
		{
			var existe = await _context.Roles.AnyAsync(x => x.Nome == nomeRole);

			if (existe)
				return null;

			var role = new Role { Nome = nomeRole };
			_context.Roles.Add(role);
			await _context.SaveChangesAsync();

			return role;
		}
	}
}
