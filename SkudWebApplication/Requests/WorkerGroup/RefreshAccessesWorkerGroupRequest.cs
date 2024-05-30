using MediatR;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Requests.WorkerGroup
{
    public class RefreshAccessesWorkerGroupRequest : IRequest
    {
        public int? Id {  get; set; }
    }
}
