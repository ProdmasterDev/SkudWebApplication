using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests;
using SkudWebApplication.Requests.AccessGroup;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.AccessGroup
{
    public class EditAccessGroupHandler : IRequestHandler<EditAccessGroupRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public EditAccessGroupHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(EditAccessGroupRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<DB.AccessGroup>(request);
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
