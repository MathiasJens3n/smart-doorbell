namespace smart_doorbell_api.Models
{
    public class Image
    {
        public int? Id { get; set; }
        public required byte[] Data { get; set; }
        public DateTime? Insert_Date { get; set; }
        public required int User_Id { get; set; }
    }
}
