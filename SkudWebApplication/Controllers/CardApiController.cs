using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SkudWebApplication.Requests.Card;
using SkudWebApplication.Requests.Controller;

namespace SkudWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardApiController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CardApiController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddCardRequest request)
        {
            try
            {
                await _mediator.Send(request);
                var refreshAccessesRequest = new RefreshAccessesCardRequest() { Id = request.Id };
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
        public async Task<IActionResult> Edit(EditCardRequest request)
        {
            try
            {
                await _mediator.Send(request);
                var refreshAccessesRequest = new RefreshAccessesCardRequest() { Id = request.Id };
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
        public async Task<IActionResult> Delete([FromBody] DeleteCardRequest request)
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
