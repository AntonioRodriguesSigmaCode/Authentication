using projetoAPI.Dto.User;
using projetoAPI.Model;

namespace Authentication.Interface
{
	public interface IUserRepository
	{
		Task<Role?> CreateRoleAsync(string nomeRole);
		//Task<UtilizadorRole> AtribuirRoleAsync(int userId, int roleId);


	}
}
