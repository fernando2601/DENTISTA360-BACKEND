using System.ComponentModel.DataAnnotations;

namespace DENTISTA360_BACKEND.DTOs
{
    public class SignupRequest
    {
        [Required]
        public PersonalData PersonalData { get; set; } = new PersonalData();

        [Required]
        public CompanyData CompanyData { get; set; } = new CompanyData();
    }

    public class PersonalData
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        public string CPF { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public string Telefone { get; set; } = string.Empty;
    }

    public class CompanyData
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string RazaoSocial { get; set; } = string.Empty;

        [Required]
        public string CNPJ { get; set; } = string.Empty;

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string NomeFantasia { get; set; } = string.Empty;

        [Required]
        public string CEP { get; set; } = string.Empty;

        [Required]
        public string Logradouro { get; set; } = string.Empty;

        [Required]
        public string Numero { get; set; } = string.Empty;

        [Required]
        public string Bairro { get; set; } = string.Empty;

        [Required]
        public string Cidade { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        [MaxLength(2)]
        public string Estado { get; set; } = string.Empty;
    }
}

