
using AuthenticationManager.Models;
using AuthenticationManager.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationManager
{
    public class AuthenticationTokenProvider
    {
        private readonly JwtConfigurationModel _config;
        public AuthenticationTokenProvider()
        {
            _config = JsonConfigurationReader.ReadJwtConfigurationModelFromRoot();
        }

        public (string, DateTime) GenerateJwtToken(string userName, string userRole, bool isTokenGeneratedWhileLogin = true)
        {
            var tokenExpiryTime = isTokenGeneratedWhileLogin ? DateTime.Now.AddMinutes(_config.TokenValidityMins) : DateTime.UnixEpoch;
            var tokenKey = Encoding.ASCII.GetBytes(_config.Key);
            var claimsIdentity = new ClaimsIdentity(
                    new List<Claim>()
                    {
                        new Claim(JwtRegisteredClaimNames.Name, userName),
                        new Claim(ClaimTypes.Role, userRole),
                    }
                );

            var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha512
                );

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTime,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return (token, tokenExpiryTime);      
        }
    }
}
