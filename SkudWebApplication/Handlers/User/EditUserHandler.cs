using AutoMapper;
using MediatR;
using SkudWebApplication.Db;
using SkudWebApplication.Requests;
using SkudWebApplication.Requests.User;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.User
{
    public class EditUserHandler : IRequestHandler<EditUserRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public EditUserHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(EditUserRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<DB.User>(request);
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
