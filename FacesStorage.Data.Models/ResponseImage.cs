namespace FacesStorage.Data.Models
{
    public class ResponseImage
    {
        public int ImageId { get; set; }

        public string ImageName { get; set; }

        public Response Response { get; set; }
        public int ResponseId { get; set; }
    }
}