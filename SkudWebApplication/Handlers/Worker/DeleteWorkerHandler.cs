using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Worker;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Worker
{
    public class DeleteWorkerHandler : IRequestHandler<DeleteWorkerRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public DeleteWorkerHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(DeleteWorkerRequest request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext
                .Set<DB.Worker>()
                .Include(x => x.Cards)
                .Include(x => x.Group)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity is null)
            {
                return;
            }

            entity.Arch = true;
            entity.Group = null;
            var cardNumbers = entity.Cards.Select(x => x.CardNumb16).ToList();
            entity.Cards = new List<DB.Card>();
            _dbContext.Update(entity);

            var quickAccesses = await _dbContext.Set<DB.QuickAccess>().Where(x => cardNumbers.Contains(x.Card)).ToListAsync();
            quickAccesses.ForEach(x => x.Granted = 0);
            _dbContext.UpdateRange(quickAccesses);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
