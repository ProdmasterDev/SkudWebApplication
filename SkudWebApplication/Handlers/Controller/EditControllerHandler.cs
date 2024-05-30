using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Controller;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Controller
{
    public class EditControllerHandler : IRequestHandler<EditControllerRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public EditControllerHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(EditControllerRequest request, CancellationToken cancellationToken)
        {
            var dbEntity = await _dbContext.Set<DB.Controller>()
                .Include(x => x.ControllerLocation)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (dbEntity == null) { return; }

            dbEntity = _mapper.Map(request, dbEntity);
            dbEntity.ControllerLocation = _mapper.Map<DB.ControllerLocation>(request.Location);

            if (dbEntity != null)
            {
                _dbContext.Update(dbEntity);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
