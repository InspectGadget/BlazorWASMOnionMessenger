using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorWASMOnionMessenger.Client.Features.Helpers
{
    public static class JwtTokenParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwt) as JwtSecurityToken;

            if (jsonToken == null)
                return null;

            return jsonToken.Claims;
        }
    }
}
