using System.ComponentModel.DataAnnotations;

namespace Authentication.ViewModel
{
	public class RegisterViewModel
	{
		[Required]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;

		[Required]
		[Compare("Password", ErrorMessage = "As passwords não coincidem")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
