using System.ComponentModel.DataAnnotations;

namespace Authentication.ViewModel
{
	public class RegisterViewModel
	{
		[Required (ErrorMessage = "O email é obrigatorio")]
		[EmailAddress (ErrorMessage = "Email Inválido")]
		public string Email { get; set; } = string.Empty;

		[Required (ErrorMessage = "A password é obrigatoria")]
		[MinLength(8, ErrorMessage = "A password deve ter no mínimo 8 caracteres")]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "A confirmação da password é obrigatória")]
		[Compare("Password", ErrorMessage = "As passwords não coincidem")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
