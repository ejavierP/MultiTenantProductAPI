using Aplicacion.Interfaces;
using Dominio.Interfaces.Common;
using Infraestructura.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IRepository<Organizations> _organizationsRepository;
        public OrganizationService(IRepository<Organizations> organizationsRepository)
        {
            _organizationsRepository = organizationsRepository;
        }

        public async Task CreateOrganization(Organizations entity)
        {
            await _organizationsRepository.Add(entity);
        }
    }
}
