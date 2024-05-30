using AutoMapper;
using MediatR;
using SkudWebApplication.Db;
using SkudWebApplication.Requests;
using SkudWebApplication.Requests.AccessGroup;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.AccessGroup
{
    public class AddAccessGroupHandler : IRequestHandler<AddAccessGroupRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public AddAccessGroupHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(AddAccessGroupRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<DB.AccessGroup>(request);
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
