using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class UserInfoResponse
    {
        public List<ClinicaInfo> Clinicas { get; set; } = new List<ClinicaInfo>();
        public UserInfo User { get; set; } = new UserInfo();
    }

    public class ClinicaInfo
    {
        public int Id { get; set; }
        public string NomeFantasia { get; set; } = string.Empty;
    }

    public class UserInfo
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;
    }
}
