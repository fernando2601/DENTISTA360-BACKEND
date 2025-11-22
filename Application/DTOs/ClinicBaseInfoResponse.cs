using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class ClinicBaseInfoResponse
    {
        [JsonPropertyName("clinicBaseInfo")]
        public ClinicBasicInfo ClinicBaseInfo { get; set; } = new ClinicBasicInfo();

        [JsonPropertyName("user")]
        public UserInfo User { get; set; } = new UserInfo();
    }

    public class ClinicBasicInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("nomeFantasia")]
        public string NomeFantasia { get; set; } = string.Empty;
    }
}

