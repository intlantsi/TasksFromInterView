using System;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CSBI.Models;
using CSBI.Interfaces;

namespace CSBI.Services
{
    public class UserService: IUserService
    {
        public UserService(IConfiguration configuration)
        {
            Key = configuration["Token:Key"];
        }

        string Key { get; }

        public string GenerateJwtToken(Guid userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Id", userId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool CheckUserCredential(Credentials cred)
        {
            if (cred!=null &&
                cred.Login !=null &&
                cred.Password != null)
            {
                return true;
            }
            return false;
        }

        SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
