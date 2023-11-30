using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.DTOS.Account
{
    public class LoginUserResponseDTO
    {
        public string AccesToken { get; set; }
        public string Tenant { get; set; }
    }
}
