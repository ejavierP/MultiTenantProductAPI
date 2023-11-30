using Infraestructura.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces
{
    public interface IOrganizationService
    {
        Task CreateOrganization(Organizations entity);
    }
}
