using System.ComponentModel.DataAnnotations;

namespace GerenciamentoBiblioteca.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, ErrorMessage = "A senha não pode exceder 100 caracteres.")]
        public string Senha { get; set; }
                
        public string Tipo { get; set; } // Ex: "Admin" ou "Usuario"

        // Outras propriedades relevantes para o seu sistema
        [StringLength(200, ErrorMessage = "O e-mail não pode exceder 200 caracteres.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Número de telefone inválido.")]
        public string Telefone { get; set; }

        // Propriedade para armazenar a data de criação do usuário
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
