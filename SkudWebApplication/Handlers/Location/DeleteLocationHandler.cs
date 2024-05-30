using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Location;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Location
{
    public class DeleteLocationHandler : IRequestHandler<DeleteLocationRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public DeleteLocationHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(DeleteLocationRequest request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Set<DB.ControllerLocation>().Include(x => x.Controller).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity is null)
            {
                return;
            }

            entity.Arch = true;
            entity.Controller = null;

            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
