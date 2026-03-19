using Authentication.Dto.User;

namespace Authentication.Dto
{
	public class RoleDto
	{
		public int Id { get; set; }
		public string Nome { get; set; }

		public List<PermissaoDto> Permissoes { get; set; } = new();
	}
}
