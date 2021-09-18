using Microsoft.IdentityModel.Tokens;
using NoeliaStorytellerAPI.Data;
using NoeliaStorytellerAPI.Models;
using NoeliaStorytellerAPI.Services.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NoeliaStorytellerAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly NoeliaStorytellerAPIContext _context;

     

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
