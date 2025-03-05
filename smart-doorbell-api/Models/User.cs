namespace smart_doorbell_api.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? RegistrationCode { get; set; }
    }
}
