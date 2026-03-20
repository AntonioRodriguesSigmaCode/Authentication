using Authentication.Interface;
using Authentication.Service;

namespace Authentication.Controllers
{
	public class UserController
	{
		private readonly IUserRepository _repo;

		public UserController(IAuthService authService, IUserRepository repo)
		{
			_repo = repo;
		}

	}
}
