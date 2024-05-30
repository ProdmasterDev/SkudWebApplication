using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SkudWebApplication.Requests.Location;
using SkudWebApplication.Requests.WorkerGroup;

namespace SkudWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationApi : ControllerBase
    {
        private readonly IMediator _mediator;
        public LocationApi(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddLocationRequest request)
        {
            try
            {
                await _mediator.Send(request);
                var refreshAccessesRequest = new RefreshAccessesLocationRequest() { Id = request.Id };
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
        [HttpPut]
        public async Task<IActionResult> Edit(EditLocationRequest request)
        {
            try
            {
                await _mediator.Send(request);
                var refreshAccessesRequest = new RefreshAccessesLocationRequest() { Id = request.Id };
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
    }
}
