using MediatR;

namespace SkudWebApplication.Requests.Controller
{
    public class RefreshAccessesControllerRequest : IRequest
    {
        public int? Id { get; set; }
    }
}
