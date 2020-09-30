namespace FacesStorage.Data.Models
{
    public class SwapRequest : Request
    {
        public RequestImage ToImage { get; set; }
        public int ToImageId { get; set; }
    }
}
