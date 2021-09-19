using Microsoft.IdentityModel.Tokens;
using NoeliaStorytellerAPI.Services.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoeliaStorytellerAPI.Services
{
    public class AuthService : IAuthService
    {         

        public string GenerateToken(DateTime date, string user, TimeSpan validDate)
        {

            var expire = date.Add(validDate);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(
                    JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64
                ),
                new Claim("roles","Client"),
                new Claim("roles","Administrator")
            };

            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes("noeliastoryteller2021messageapirest")),
                SecurityAlgorithms.HmacSha256Signature
            );

            var jwt = new JwtSecurityToken(
            issuer: "Example",
            audience: "Public",
            claims: claims,
            notBefore: date,
            expires: expire,
            signingCredentials: signingCredentials
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }
}
