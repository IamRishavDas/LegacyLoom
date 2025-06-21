
using AuthenticationManager.Models;
using AuthenticationManager.TokenProviderInterface;
using AuthenticationManager.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationManager
{
    public class AuthenticationTokenProvider: ITokenProvider
    {
        private readonly JwtConfigurationModel _config;
        public AuthenticationTokenProvider()
        {
            _config = JsonConfigurationReader.ReadJwtConfigurationModelFromRoot();
        }

        public (string, DateTime) GenerateJwtToken(Guid userId, string userName, string userRole, bool isTokenGeneratedWhileLogin = true)
        {
            try
            {
                var tokenExpiryTime = isTokenGeneratedWhileLogin ? DateTime.UtcNow.AddMinutes(_config.TokenValidityMins) : DateTime.UtcNow.AddSeconds(0.00000001); ;
                var tokenKey = isTokenGeneratedWhileLogin ? Encoding.ASCII.GetBytes(_config.Key) : Encoding.ASCII.GetBytes(_config.InvalidKey);
                var claimsIdentity = new ClaimsIdentity(
                        new List<Claim>()
                        {
                            new Claim(JwtRegisteredClaimNames.Name, userName),
                            new Claim(ClaimTypes.Role, userRole),
                            new Claim("UserId", userId.ToString()),
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
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to generate the jwt token!, Exception: {ex}");
                return ("", DateTime.Now);
            }
        }
    }
}
