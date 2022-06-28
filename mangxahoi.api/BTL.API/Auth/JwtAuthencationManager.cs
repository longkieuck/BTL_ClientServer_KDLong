using BTL.API.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BTL.API.Auth
{
    public class JwtAuthencationManager  : IJwtAuthencationManager
    {
        private const string TAG = "JwtAuthencationManager";
        private readonly string _key;
        private readonly IConfiguration _configuration;
        public JwtAuthencationManager(string key, IConfiguration configuration)
        {
            _key = key;
            _configuration = configuration;
        }


        public async Task<string> Autheticate(UserTb user)
        {
            return GetToken(user);
        }

        public string GetToken(UserTb user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes(_key);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Role, $"{RoleType.User}"),
                    }),
                    Expires = DateTime.UtcNow.AddHours(double.Parse(_configuration["Jwt:ExpireDate"])),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature),
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

    }
}
