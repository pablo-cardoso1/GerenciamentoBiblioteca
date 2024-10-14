using System.ComponentModel.DataAnnotations;

public class CadastroModel
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    public string Email { get; set; }

    [Required(ErrorMessage ="O telefone é obrigatório")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(100, ErrorMessage = "A senha deve ter entre {2} e {1} caracteres.", MinimumLength = 6)]
    public string Senha { get; set; }
    
}
