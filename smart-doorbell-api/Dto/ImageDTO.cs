namespace smart_doorbell_api.Dto
{
    public class ImageDTO
    {
        public int? Id { get; set; }
        public required string Data { get; set; }
        public required int UserId { get; set; }
        public DateTime? Insert_Date { get; set; }

    }
}
