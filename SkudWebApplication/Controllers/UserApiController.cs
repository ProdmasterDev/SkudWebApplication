using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SkudWebApplication.Requests.User;

namespace SkudWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserApiController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserApiController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddUserRequest request)
        {
            try
            {
                await _mediator.Send(request);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, JsonConvert.SerializeObject(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, JsonConvert.SerializeObject(ex));
            }
        }
        [HttpPut]
        public async Task<IActionResult> Edit(EditUserRequest request)
        {
            try
            {
                await _mediator.Send(request);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, JsonConvert.SerializeObject(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, JsonConvert.SerializeObject(ex));
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteUserRequest request)
        {
            try
            {
                await _mediator.Send(request);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, JsonConvert.SerializeObject(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, JsonConvert.SerializeObject(ex));
            }
        }
    }
}
