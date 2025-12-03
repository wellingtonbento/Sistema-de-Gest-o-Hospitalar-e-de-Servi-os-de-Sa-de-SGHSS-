using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API_SGHSS.Services.Interfaces
{
    public interface ITokenService
    {
        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);

        public string GenerateRefreshToken();

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
    }
}
