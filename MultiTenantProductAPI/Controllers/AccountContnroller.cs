﻿using Dominio.DTOS;
using Dominio.DTOS.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountContnroller : ControllerBase
    {
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
                    Expires = DateTime.UtcNow.AddHours(1)
                };


                Response.Cookies.Append("access_token", result.AccessToken, cookieOptions);
                return Ok(new APIResponse<LoginUserResponseDTO>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(400, new APIResponse<LoginUserResponseDTO>(null, false));
            }

        }
    }
}
