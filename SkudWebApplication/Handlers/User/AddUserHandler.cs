using AutoMapper;
using MediatR;
using SkudWebApplication.Db;
using SkudWebApplication.Requests;
using SkudWebApplication.Requests.User;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.User
{
    public class AddUserHandler : IRequestHandler<AddUserRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public AddUserHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(AddUserRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<DB.User>(request);
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
