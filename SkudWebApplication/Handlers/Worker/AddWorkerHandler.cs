using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Worker;
using SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Worker
{
    public class AddWorkerHandler : IRequestHandler<AddWorkerRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public AddWorkerHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(AddWorkerRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<DB.Worker>(request);
            if (entity.AccessMethod != null)
                entity.AccessMethod = await _dbContext.Set<DB.AccessMethod>().FirstAsync(x => x.Id == entity.AccessMethod.Id);
            if (entity.Group != null)
                entity.Group = await _dbContext.Set<DB.WorkerGroup>().FirstAsync(x => x.Id == entity.Group.Id);
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
