using projetoAPI.Model;

namespace Authentication.Interface
{
	public interface IPermissaoRepository
	{
		Task<Permissao> CreatePermissaoAsync(string nomePermissao);
		Task<bool> AtribuirPermissaoAsync(int roleId, int permissaoId);
	}
}
