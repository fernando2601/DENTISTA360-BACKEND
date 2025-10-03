namespace DENTISTA360_BACKEND.Models
{
    public class UserClinic
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClinicId { get; set; }
        public string GroupType { get; set; } = string.Empty;
        
        // Navigation properties
        public User? User { get; set; }
        public Clinica? Clinic { get; set; }
    }

    public static class UserClinicGroupTypes
    {
        public const string Employee = "employee";
        public const string Director = "director";
        public const string Doctor = "doctor";
    }
}
