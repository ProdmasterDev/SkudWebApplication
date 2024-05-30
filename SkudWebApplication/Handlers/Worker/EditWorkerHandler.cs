using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Components.Pages;
using SkudWebApplication.Db;
using SkudWebApplication.Requests;
using SkudWebApplication.Requests.Worker;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Worker
{
    public class EditWorkerHandler : IRequestHandler<EditWorkerRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public EditWorkerHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(EditWorkerRequest request, CancellationToken cancellationToken)
        {
            var dbEntity = await _dbContext
                .Set<DB.Worker>()
                .Include(x => x.Cards)
                .Include(x => x.Accesses)
                    .ThenInclude(x => x.ControllerLocation)
                .Include(x => x.Group)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (dbEntity == null) { return; }

            dbEntity = _mapper.Map(request, dbEntity);
            foreach (var newCard in request.Cards.Where(x => x.Id == default))
            {
                var newCardDb = _mapper.Map<DB.Card>(newCard);
                newCardDb.WorkerId = dbEntity.Id;
                await _dbContext.AddAsync(newCardDb, cancellationToken);
            }
            foreach(var card in dbEntity.Cards)
            {
                var c = request.Cards.FirstOrDefault(x => x.CardNumb == card.CardNumb);
                if (c == null)
                {
                    card.WorkerId = null;
                }
            }

            if (dbEntity != null)
            {
                _dbContext.Update(dbEntity);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
