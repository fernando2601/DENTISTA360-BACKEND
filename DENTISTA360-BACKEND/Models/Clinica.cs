namespace DENTISTA360_BACKEND.Models
{
    public class Clinica
    {
        public int Id { get; set; }
        public string NomeFantasia { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public int EnderecoId { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string NomeResponsavel { get; set; } = string.Empty;
        
        // Navigation property
        public Endereco? Endereco { get; set; }
    }
}
