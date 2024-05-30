using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests;
using SkudWebApplication.Requests.WorkerGroup;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.WorkerGroup
{
    public class AddWorkerGroupHandler : IRequestHandler<AddWorkerGroupRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public AddWorkerGroupHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(AddWorkerGroupRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<DB.WorkerGroup>(request);
            var accesses = _mapper.Map<IEnumerable<AccessRequest>, IEnumerable<DB.GroupAccess >> (request.Accesses);
            foreach(var access in accesses)
            {
                access.Group = entity;
                await _dbContext.AddAsync(access, cancellationToken);
            }
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
