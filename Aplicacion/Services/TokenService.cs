using Aplicacion.Interfaces;
using Dominio.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<JWTOptions> _jwtOptions;
        public TokenService(IOptions<JWTOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }
        public async Task<string> CreateToken(string email)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken createToken;
                string accessToken;
                string? secretKey = _jwtOptions.Value.Secret;
                var key = Encoding.ASCII.GetBytes(secretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {

                    Subject = new ClaimsIdentity(new Claim[]
                    {
                      new Claim(ClaimTypes.Name,email),
                    }),
                    Expires = DateTime.UtcNow.AddHours(_jwtOptions.Value.ExpiryHours),
                    Issuer = _jwtOptions.Value.Issuer,
                    Audience = _jwtOptions.Value.Audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                createToken = tokenHandler.CreateToken(tokenDescriptor);
                accessToken = tokenHandler.WriteToken(createToken);
                return accessToken;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
    }
}
