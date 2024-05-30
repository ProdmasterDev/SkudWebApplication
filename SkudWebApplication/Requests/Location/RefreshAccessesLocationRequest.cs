using MediatR;

namespace SkudWebApplication.Requests.Location
{
    public class RefreshAccessesLocationRequest : IRequest
    {
        public int? Id { get; set; }
    }
}
