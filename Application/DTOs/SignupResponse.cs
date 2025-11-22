namespace Application.DTOs
{
    public class SignupResponse
    {
        public int UserId { get; set; }
        public int ClinicId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

