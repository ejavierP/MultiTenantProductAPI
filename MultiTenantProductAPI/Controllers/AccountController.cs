using Aplicacion.Interfaces;
using Aplicacion.Services;
using Dominio.DTOS;
using Dominio.DTOS.Account;
using Dominio.DTOS.Organization;
using Infraestructura.Models.Organizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {

            _accountService = accountService;
        }


        [HttpPost]
        [Route("Authenticate")]
        [ProducesResponseType(typeof(APIResponse<LoginUserResponseDTO>), 200)]
        public async Task<IActionResult> Authenticate([FromBody] LoginUserRequestDTO userRequest)
        {
            try
            {
                var result = await _accountService.Authenticate(userRequest);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(2)
                };


                Response.Cookies.Append("access_token", result.AccessToken, cookieOptions);
                return Ok(new APIResponse<LoginUserResponseDTO>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(400, new APIResponse<LoginUserResponseDTO>(null, false,ex.Message));
            }

        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO createUserRequest)
        {
            try
            {
                await _accountService.CreateUser(new Users
                {
                    Email = createUserRequest.Email,
                    Password = createUserRequest.Password,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    OrganizationId =  createUserRequest.OrganizationId,
                });


                return Ok(new APIResponse<string>("Usuario creado con exito", true, null));
            }
            catch (Exception ex)
            {
                return StatusCode(400, new APIResponse<string>(null, false, ex.Message));
            }

        }
    }
}
