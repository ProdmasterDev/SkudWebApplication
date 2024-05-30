using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.AccessGroup;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.AccessGroup
{
    public class DeleteAccessGroupHandler : IRequestHandler<DeleteAccessGroupRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public DeleteAccessGroupHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(DeleteAccessGroupRequest request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Set<DB.AccessGroup>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity is null)
            {
                return;
            }

            entity.Arch = true;

            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
