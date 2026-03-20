namespace Authentication.Dto.User
{
	public class UserDto
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public List<RoleDto> Roles { get; set; }
	}
}
