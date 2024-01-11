using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Recept.Data
{
    public static class JwtTokenGenerator
    {
        public static string GenerateJwtToken(string username,string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("EzIttEgyNagyonHosszuTitkosKulcsAmiLegalabb128BitHosszu"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
        };

            var token = new JwtSecurityToken(
                "https://localhost:7045",
                "https://localhost:7045",
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
