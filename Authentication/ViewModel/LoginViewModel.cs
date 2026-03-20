using System.ComponentModel.DataAnnotations;

namespace Authentication.ViewModel
{
	public class LoginViewModel
	{
		[Required (ErrorMessage = "O email é obrigátorio")]
		[EmailAddress (ErrorMessage = "Email Inválido")]

		public string Email { get; set; } = string.Empty;
		[Required (ErrorMessage = "A Password é Obrigátorio")]
		public string Password { get; set; } = string.Empty;
		public bool RememberMe { get; set; }	
	}
}
