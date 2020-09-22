namespace FacesStorage.Data.Models
{
    public class SwapRequest : Request
    {
        public RequestImage FromImage { get; set; }
        public int FromImageId { get; set; }

        public RequestImage ToImage { get; set; }
        public int ToImageId { get; set; }
    }
}
