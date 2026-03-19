using Authentication.Dto;
using Authentication.Dto.User;
using projetoAPI.Model;

namespace Authentication.Mappers
{
	public static class Mapper
	{
		public static UserDto ToUserDto(this Utilizador user)
		{
			return new UserDto
			{
				Id = user.Id,
				Username = user.Username,
				Roles = user.Roles.Select(r => new RoleDto
				{
					Id = r.Id,
					Nome = r.Nome,
				}).ToList()
			};
		}

		public static PermissaoDto MapToDto(this Permissao permissao)
		{
			return new PermissaoDto
			{
				Id = permissao.Id,
				Nome = permissao.Nome
			};
		}

		public static RoleDto MapToDto(this Role role)
		{
			return new RoleDto
			{
				Id = role.Id,
				Nome = role.Nome,
				Permissoes = role.Permissoes.Select(MapToDto).ToList()
			};
		}
	}
}
