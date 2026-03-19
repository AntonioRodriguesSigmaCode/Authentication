using Authentication.Interface;
using Microsoft.EntityFrameworkCore;
using projetoAPI.Data;
using projetoAPI.Model;

namespace Authentication.Repositoy
{
	public class RoleRepository: IRoleRepository
	{
		private readonly AppDbContext _context;
		public RoleRepository(AppDbContext context)
		{
			_context = context;
		}
		public async Task<Utilizador> AtribuirRoleAsync(int userId, int roleId)
		{
			// buscar utilizador com as roles
			var user = await _context.Utilizadores.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
				return null;

			// buscar a role
			var role = await _context.Roles.FindAsync(roleId);
			if (role == null)
				return null;

			// verificar se já tem a role
			if (!user.Roles.Contains(role))
			{
				// guardar role no utilizador
				user.Roles.Add(role);
				await _context.SaveChangesAsync();
			}

			return user;
		}

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
	
		public async Task<List<Role>> GetAllAsync()
		{
			return await _context.Roles
				.Include(p => p.Utilizadores)
				.Include(p => p.Permissoes)
				.ToListAsync();
		}

		public async Task<Role> GetByIdAsync(int id)
		{
			return await _context.Roles
				.Include(p => p.Utilizadores)
				.Include(p => p.Permissoes)
				.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
