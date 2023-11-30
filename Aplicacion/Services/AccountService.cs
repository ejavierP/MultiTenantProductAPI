using Aplicacion.Interfaces;
using Dominio.Configurations;
using Dominio.DTOS.Account;
using Dominio.Interfaces.Common;
using Infraestructura.Models.Organizations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aplicacion.Services
{
    public class AccountService : IAccountService
    {
       
        private readonly ITokenService  _tokenService;
        private readonly IRepositoryOrganizations<Users> _userRepository;
        public AccountService(ITokenService tokenService, IRepositoryOrganizations<Users> userRepository)
        {
           _userRepository = userRepository;
           _tokenService = tokenService;
        }
        public async Task<LoginUserResponseDTO> Authenticate(LoginUserRequestDTO userRequest)
        {

            var user = await _userRepository.Get(user => user.Email == userRequest.Email && user.Password == userRequest.Password, "Organizations");

            if (user == null) 
            {
                throw new Exception("Las credenciales son incorrectas o usuario inexistente.");
            }

            var token = await _tokenService.CreateToken(userRequest.Email);
            

            return new LoginUserResponseDTO
            {
                AccessToken = token,
                Tenant = user.Organizations.SlugTenant
            };
        }

        public async Task CreateUser(Users user)
        {
            var userEmail = await _userRepository.Get(user => user.Email == user.Email);
            Regex passwordValidatioRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            if (userEmail != null)
            {
                throw new Exception("Ya existe un usuario con ese email");
            }
            var validatePassword = passwordValidatioRegex.Match(user.Password);

            if (!validatePassword.Success)
            {
                throw new Exception("Debe un password con un formato valido");
            }

            await _userRepository.Add(user);
        }
    }
}

