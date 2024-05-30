using AutoMapper;
using ControllerDomain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests;
using SkudWebApplication.Requests.Location;
using SkudWebApplication.Requests.WorkerGroup;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Location
{
    public class EditLocationHandler : IRequestHandler<EditLocationRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public EditLocationHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(EditLocationRequest request, CancellationToken cancellationToken)
        {
            var dbEntity = await _dbContext.Set<DB.ControllerLocation>()
                .Include(x => x.Controller)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (dbEntity == null) { return; }

            var entityToUpdate = _mapper.Map(request, dbEntity);
            if(request.Controller != null)
            {
                entityToUpdate.Controller = await _dbContext.Set<DB.Controller>().FirstOrDefaultAsync(x => x.Id == request.Controller.Id);
            }
            if (entityToUpdate != null)
            {
                _dbContext.Update(entityToUpdate);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
