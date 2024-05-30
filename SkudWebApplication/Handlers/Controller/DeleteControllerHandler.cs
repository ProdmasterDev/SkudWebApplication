using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Controller;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Controller
{
    public class DeleteControllerHandler : IRequestHandler<DeleteControllerRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public DeleteControllerHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(DeleteControllerRequest request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Set<DB.Controller>().Include(x => x.ControllerLocation).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if(entity is null)
            {
                return;
            }

            entity.Arch = true;
            entity.ControllerLocation = null;

            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
