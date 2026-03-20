using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Model
{
    public class Utilizador: IdentityUser
	{
        //public int Id { get; set; }
        //public string Username { get; set; } = string.Empty;
        //public string PasswordHash { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
		public string? SessionToken { get; set; }

		public ICollection<Role> Roles { get; set; }

	}
}