using Microsoft.IdentityModel.Tokens;

namespace AuthWebapi.Models
{
    public class AppSettings
    {
        public string JWTSecret { get; set; }
        public SymmetricSecurityKey? signInKey { get; set; }

        public string? validIssuer { get; set; }

        public string? validAudience { get; set; }



    }
}
