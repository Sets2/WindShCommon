using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models.Auth;

namespace WebApi.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _expireDuration =new TimeSpan(10,30,0);
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string BuildToken(UserAuthDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.SecurityStamp)
            };
            
            if(user.Roles != null)
                foreach (var userRole in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                }
            var securityKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(_configuration["Jwt:Issuer"], 
                _configuration["Jwt:Audience"], claims, expires: DateTime.Now.Add(_expireDuration), 
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
