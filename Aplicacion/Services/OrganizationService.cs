using Aplicacion.Interfaces;
using Dominio.Interfaces.Common;
using Infraestructura.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IRepositoryOrganizations<Organizations> _organizationsRepository;
        public OrganizationService(IRepositoryOrganizations<Organizations> organizationsRepository)
        {
            _organizationsRepository = organizationsRepository;
        }

        public async Task CreateOrganization(Organizations entity)
        {
            await _organizationsRepository.Add(entity);
        }

        public async Task<Organizations> Get(string slugTenant)
        {
            var organization = await _organizationsRepository.Get(organization => organization.SlugTenant == slugTenant);
            return organization;
        }
    }
}
