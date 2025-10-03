namespace DENTISTA360_BACKEND.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        public string DescricaoGrupo { get; set; } = string.Empty;
    }

    public static class GrupoTypes
    {
        public const string SuperAdmin = "SUPER ADMIN";
        public const string Admin = "ADMIN";
        public const string Gerente = "GERENTE";
        public const string Financeiro = "FINANCEIRO";
        public const string Recepcao = "RECEPCAO";
        public const string Estoque = "ESTOQUE";
        public const string Medico = "MEDICO";
    }
}
