using AutoMapper;
using MediatR;
using SkudWebApplication.Db;
using SkudWebApplication.Requests;
using SkudWebApplication.Requests.WorkerGroup;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.WorkerGroup
{
    public class EditWorkerGroupHandler : IRequestHandler<EditWorkerGroupRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public EditWorkerGroupHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(EditWorkerGroupRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<DB.WorkerGroup>(request);
            var accesses = _mapper.Map<IEnumerable<AccessRequest>, IEnumerable<DB.GroupAccess>>(request.Accesses);
            foreach (var access in accesses)
            {
                access.Group = entity;
                if(access.Id == 0)
                    await _dbContext.AddAsync(access, cancellationToken);
                else
                    _dbContext.Update(access);
            }
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
