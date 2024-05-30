using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Card;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Card
{
    public class EditCardHandler : IRequestHandler<EditCardRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public EditCardHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(EditCardRequest request, CancellationToken cancellationToken)
        {
            var dbEntity = await _dbContext.Set<DB.Card>()
                .Include(x => x.Worker)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (dbEntity == null) { return; }

            var entityToUpdate = _mapper.Map(request, dbEntity);

            if (entityToUpdate != null)
            {
                _dbContext.Update(entityToUpdate);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
