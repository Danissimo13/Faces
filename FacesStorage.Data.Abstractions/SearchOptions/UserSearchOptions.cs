namespace FacesStorage.Data.Abstractions.SearchOptions
{
    public class UserSearchOptions
    {
        public UserSearchTypes SearchType { get; set; }

        public int? UserId { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }

        public bool WithRole { get; set; }

        public bool WithRequests { get; set; }

        public int? FromRequest { get; set; }
        public int? RequestsCount { get; set; }

        public bool WithRequestImages { get; set; }

        public bool WithRequestResponses { get; set; }

        public bool WithResponseImages { get; set; }

        public bool WithPassword { get; set; }

        public UserSearchOptions()
        {
            SearchType = UserSearchTypes.WithoutProperty;
        }
    }

    public enum UserSearchTypes
    {
        ById,
        ByEmail,
        ByNickname,
        WithoutProperty
    }
}
