using Aplicacion.Interfaces;
using Dominio.DTOS;
using Dominio.DTOS.Account;
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
    }
}
