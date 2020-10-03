namespace FacesStorage.Data.Abstractions.SearchOptions
{
    public class UsersSearchOptions
    {
        public UsersSearchTypes SearchType { get; set; }

        public int? From { get; set; }
        public int? Count { get; set; }

        public string Email { get; set; }
        public string Nickname { get; set; }

        public bool WithRole { get; set; }

        public bool WithRequests { get; set; }

        public int? FromRequest { get; set; }
        public int? RequestsCount { get; set; }

        public bool WithRequestImages { get; set; }

        public bool WithRequestResponses { get; set; }

        public bool WithResponseImages { get; set; }

        public bool WithPassword { get; set; }

        public UsersSearchOptions()
        {
            SearchType = UsersSearchTypes.WithoutProperty;
        }
    }

    public enum UsersSearchTypes
    {
        ByEmail,
        ByNickname,
        WithoutProperty,
    }
}
