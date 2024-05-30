using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SkudWebApplication.Repos;
using SkudWebApplication.Requests.Card;
using LoginRequest = SkudWebApplication.Requests.Auth.LoginRequest;

namespace SkudWebApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginApiController : ControllerBase
    {
        private readonly IAccount _account;
        public LoginApiController(IAccount account)
        {
             _account = account;
        }
        [HttpGet]
        public async Task<IActionResult> Add([FromQuery] LoginRequest request)
        {
            try
            {
                return Ok(await _account.LoginAsync(request));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, JsonConvert.SerializeObject(ex));
            }
        }
    }
}
