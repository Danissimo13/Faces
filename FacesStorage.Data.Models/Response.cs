namespace FacesStorage.Data.Models
{
    public abstract class Response
    {
        public int ResponseId { get; set; }

        public Request Request { get; set; }
        public int RequestId { get; set; }
    }
}
