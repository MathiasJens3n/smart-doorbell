namespace smart_doorbell_api.Models
{
    public class Device
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public required string RegistrationCode { get; set; }
    }
}
