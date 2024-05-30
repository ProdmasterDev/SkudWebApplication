using MediatR;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Requests
{
    public class RefreshAccessesRequest : IRequest
    {
        public IEnumerable<DB.Worker> Workers { get; set; } = new List<DB.Worker>();
    }
}
