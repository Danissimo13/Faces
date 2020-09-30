namespace FacesStorage.Data.Models
{
    public class Request
    {
        public int RequestId { get; set; }

        public RequestImage FromImage { get; set; }
        public int FromImageId { get; set; }

        public User User { get; set; }
        public int? UserId { get; set; }

        public Response Response { get; set; }
        public int? ResponseId { get; set; }
    }
}
