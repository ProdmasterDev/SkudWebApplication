using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.WorkerGroup;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.WorkerGroup
{
    public class DeleteWorkerGroupHandler : IRequestHandler<DeleteWorkerGroupRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public DeleteWorkerGroupHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(DeleteWorkerGroupRequest request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Set<DB.WorkerGroup>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
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
