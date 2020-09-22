namespace FacesStorage.Data.Models
{
    public abstract class Request
    {
        public int RequestId { get; set; }

        public User User { get; set; }
        public int? UserId { get; set; }

        public Response Response { get; set; }
        public int? ResponseId { get; set; }
    }
}
