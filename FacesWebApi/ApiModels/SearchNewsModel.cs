namespace FacesWebApi.ApiModels
{
    public class SearchNewsModel
    {
        public int From { get; set; }

        public int Count { get; set; }

        public bool WithBody { get; set; }
    }
}
