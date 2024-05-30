using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SkudWebApplication.Requests.Worker;
using SkudWebApplication.Requests.WorkerGroup;

namespace SkudWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerGroupApi : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public WorkerGroupApi(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddWorkerGroupRequest request)
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
        public async Task<IActionResult> Edit(EditWorkerGroupRequest request)
        {
            try
            {
                await _mediator.Send(request);
                var refreshAccessesRequest = new RefreshAccessesWorkerGroupRequest() { Id = request.Id };
                await _mediator.Send(refreshAccessesRequest);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, JsonConvert.SerializeObject(ex));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, JsonConvert.SerializeObject(ex));
            }
        }
    }
}
