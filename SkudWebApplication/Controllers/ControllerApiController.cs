using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SkudWebApplication.Requests.Controller;
using SkudWebApplication.Requests.Location;

namespace SkudWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ControllerApi : ControllerBase
    {
        private readonly IMediator _mediator;
        public ControllerApi(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPut]
        public async Task<IActionResult> Edit(EditControllerRequest request)
        {
            try
            {
                await _mediator.Send(request);
                var refreshAccessesRequest = new RefreshAccessesControllerRequest() { Id = request.Id };
                await _mediator.Send(refreshAccessesRequest);
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
        public async Task<IActionResult> Delete([FromBody] DeleteControllerRequest request)
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
