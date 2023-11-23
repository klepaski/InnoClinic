using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GatewayAPI
{
    public class ValidateToken
    {
        private readonly SymmetricSecurityKey _secret;

        public ValidateToken()
        {
            string secret = "JWTRefreshTokenHIGHsecuredPasswordVVVp1OH7Xzyr";
            _secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }


        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _secret,
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            if (securityToken.ValidTo < DateTime.UtcNow)
            {
                throw new SecurityTokenException("Token expired.");
            }
            var validToken = securityToken as JwtSecurityToken;
            if (validToken == null || !validToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token.");
            return principal;
        }

        //var principal = _tokenService.GetPrincipalFromToken(req.AccessToken);
        //var userId = principal.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

    }
}
