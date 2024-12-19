using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SkudWebApplication.Requests.Worker;

namespace SkudWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerApiController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public WorkerApiController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddWorkerRequest request)
        {
            try
            {
                await _mediator.Send(request);
                var refreshAccessesRequest = _mapper.Map<RefreshAccessesWorkerRequest>(request);
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
        public async Task<IActionResult> Edit(EditWorkerRequest request)
        {
            try
            {
                await _mediator.Send(request);
                //var WorkerAccessGRequest = _mapper.Map<RefreshAccessGroupWorkerRequest>(request);
                //await _mediator.Send(WorkerAccessGRequest);
                var refreshAccessesRequest = _mapper.Map<RefreshAccessesWorkerRequest>(request);
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
        public async Task<IActionResult> Delete([FromBody] DeleteWorkerRequest request)
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
