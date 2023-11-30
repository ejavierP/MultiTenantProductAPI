using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.DTOS.Organization
{
    public class CreateOrganizationRequestDTO
    {
        public string Name { get; set; }
        public string SlugTenant { get; set; }
    }
}
