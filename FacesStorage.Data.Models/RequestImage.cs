namespace FacesStorage.Data.Models
{
    public class RequestImage
    {
        public int ImageId { get; set; }

        public string ImageName { get; set; }

        public Request Request { get; set; }
        public int RequestId { get; set; }
    }
}
