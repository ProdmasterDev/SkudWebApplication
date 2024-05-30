using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests;
using SkudWebApplication.Requests.Location;
using SkudWebApplication.Requests.WorkerGroup;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Location
{
    public class AddLocationHandler : IRequestHandler<AddLocationRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public AddLocationHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(AddLocationRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<DB.ControllerLocation>(request);
            if (request.Controller != null)
            {
                entity.Controller = await _dbContext.Set<DB.Controller>().FirstOrDefaultAsync(x => x.Id == request.Controller.Id);
            }
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
