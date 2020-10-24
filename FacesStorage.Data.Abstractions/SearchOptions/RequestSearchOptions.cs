namespace FacesStorage.Data.Abstractions.SearchOptions
{
    public class RequestSearchOptions
    {
        public int RequestId { get; set; }

        public bool WithUser { get; set; }

        public bool WithImages { get; set; }

        public bool WithResponse { get; set; }

        public bool WithResponseImages { get; set; }
    }
}
