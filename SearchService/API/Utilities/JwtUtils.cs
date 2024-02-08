using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Utilities
{
    public class JwtUtils
    {
        public JwtUtils()
        {
            
        }

        public string? GetSidFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            //return the sid claim from the token
            return jsonToken?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value;
        }
    }
}
