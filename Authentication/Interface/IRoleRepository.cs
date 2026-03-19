using projetoAPI.Model;

namespace Authentication.Interface
{
	public interface IRoleRepository
	{
		Task<Role?> CreateRoleAsync(string nomeRole);
		Task<Utilizador> AtribuirRoleAsync(int userId, int roleId);
		Task<List<Role>> GetAllAsync();
		Task<Role> GetByIdAsync(int id);
	}
}
