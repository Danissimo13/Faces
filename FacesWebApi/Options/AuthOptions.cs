using FacesWebApi.Services.Implemetations;
using Microsoft.IdentityModel.Tokens;

namespace FacesWebApi.Options
{
    public static class AuthOptions
    {
        public const string ISSUER = "FacesCorporation";
        public const string AUDIENCE = "FacesApp";
        public const int LIFETIME = 60 * 24;

        private static readonly byte[] KEY;

        static AuthOptions()
        {
            KEY = new HashService().Key;
        }

        public static SymmetricSecurityKey GetSymmetricSecurity()
        {
            return new SymmetricSecurityKey(KEY);
        }
    }
}
