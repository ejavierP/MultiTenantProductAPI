using Dominio.DTOS.Account;
using Dominio.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.Interfaces;
using Dominio.DTOS.Organization;
using Infraestructura.Models.Organizations;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService  _organizationService;
        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;

        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequestDTO createOrganizationRequest)
        {
            try
            {
                await _organizationService.CreateOrganization(new Organizations
                {
                    Name = createOrganizationRequest.Name,
                    SlugTenant = createOrganizationRequest.SlugTenant,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                });

               
                return Ok(new APIResponse<string>("Organizacion creada con sastifactoriamente",true, null));
            }
            catch (Exception ex)
            {
                return StatusCode(400, new APIResponse<string>(null, false, ex.Message));
            }

        }
    }
}
