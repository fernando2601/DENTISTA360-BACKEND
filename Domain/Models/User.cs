namespace DENTISTA360_BACKEND.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? EnderecoId { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public string Senha { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        
        // Navigation property
        public Endereco? Endereco { get; set; }
    }
}
