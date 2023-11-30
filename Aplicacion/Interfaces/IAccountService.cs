using Dominio.DTOS.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces
{
    public interface IAccountService
    {
        Task<LoginUserResponseDTO> Authenticate(LoginUserRequestDTO loginRequest);

        Task CreateUser(CreateUserRequestDTO createUserRequest);
    }
}
