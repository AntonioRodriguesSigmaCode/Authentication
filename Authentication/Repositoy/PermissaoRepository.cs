using Authentication.Interface;
using Microsoft.EntityFrameworkCore;
using projetoAPI.Data;
using projetoAPI.Model;

namespace Authentication.Repositoy
{
	public class PermissaoRepository: IPermissaoRepository
	{
		private readonly AppDbContext _context;

		public PermissaoRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Permissao> CreatePermissaoAsync(string nomePermissao)
		{
			var existe = await _context.Permissoes.FirstOrDefaultAsync(x => x.Nome == nomePermissao);

			if (existe != null)
				return null;

			var permissao = new Permissao { Nome = nomePermissao };
			_context.Permissoes.Add(permissao);
			await _context.SaveChangesAsync();

			return permissao;
		}

		public async Task<bool> AtribuirPermissaoAsync(int roleId, int permissaoId)
		{
			// 1️⃣ Buscar a role e a permissão
			var role = await _context.Roles
				.Include(r => r.Permissoes) // Inclui a lista de permissões da role
				.FirstOrDefaultAsync(r => r.Id == roleId);

			if (role == null) return false; // Role não existe

			var permissao = await _context.Permissoes.FindAsync(permissaoId);
			if (permissao == null) return false; // Permissão não existe

			// 2️⃣ Verificar se a permissão já pertence à role
			if (role.Permissoes.Any(p => p.Id == permissaoId))
				return false; // Já atribuída

			// 3️⃣ Atribuir a permissão à role
			permissao.Role = role;   // Atualiza a FK
			role.Permissoes.Add(permissao); // Atualiza a coleção

			// 4️⃣ Guardar alterações
			await _context.SaveChangesAsync();

			return true;
		}
	}
}
