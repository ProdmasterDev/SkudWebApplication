using MediatR;

namespace SkudWebApplication.Requests.Card
{
    public class RefreshAccessesCardRequest : IRequest
    {
        public int? Id { get; set; }
    }
}
