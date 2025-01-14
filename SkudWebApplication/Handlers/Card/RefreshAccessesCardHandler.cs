using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Card;
using SkudWebApplication.Requests.Controller;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Card
{
    public class RefreshAccessesCardHandler : IRequestHandler<RefreshAccessesCardRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public RefreshAccessesCardHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(RefreshAccessesCardRequest request, CancellationToken cancellationToken)
        {
            if(request.Id == null) { return; }
            var card = await _dbContext
                .Set<DB.Card>()
                .Include(x => x.Worker)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (card == null) { return; }

            if(card.Worker == null) 
            {
                var quickAccessesToBeNotGranted = await _dbContext.Set<DB.QuickAccess>().Where(x => x.Card == card.CardNumb16).ToListAsync();
                quickAccessesToBeNotGranted.ForEach(x => x.Granted = 0);
                _dbContext.UpdateRange(quickAccessesToBeNotGranted);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return;
            }

            var dbWorker = await _dbContext
                .Set<DB.Worker>()
                .Include(x => x.Cards)
                .Include(x => x.Accesses)
                    .ThenInclude(x => x.ControllerLocation)
                        .ThenInclude(x => x!.Controller)
                .Include(x => x.Group)
                    .ThenInclude(x => x.GroupAccess)
                        .ThenInclude(x => x.ControllerLocation)
                            .ThenInclude(x => x.Controller)
                .Include(x => x.AccessMethod)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (dbWorker == null) { return; }

            List<DB.QuickAccess> newQuickAccesses = new();


            if (dbWorker.AccessMethodId == 3)
            {
                dbWorker.Accesses.ToList().ForEach(x => AddAccessToList(newQuickAccesses, x, string.Empty, dbWorker.DateBlock));
            }

            if (dbWorker.AccessMethodId == 1 && dbWorker.Group != null)
            {
                dbWorker.Group.GroupAccess.ToList().ForEach(x => AddGroupAccessToList(newQuickAccesses, x, string.Empty, dbWorker.DateBlock));
            }

            var quickAccessesRequest = _dbContext.Set<DB.QuickAccess>();
            foreach (var workerCard in dbWorker.Cards)
            {
                if (newQuickAccesses.Count == 0)
                {
                    var dbQuickAccesses = await quickAccessesRequest.Where(x => x.Card == workerCard.CardNumb16).ToListAsync(cancellationToken);
                    dbQuickAccesses.ForEach(x => x.Granted = 0);
                    _dbContext.UpdateRange(dbQuickAccesses);
                }
                foreach (var qa in newQuickAccesses)
                {
                    var dbQuickAccess = await quickAccessesRequest.FirstOrDefaultAsync(x => x.Sn == qa.Sn && x.Reader == qa.Reader && x.Card == workerCard.CardNumb16, cancellationToken);
                    if (dbQuickAccess != null)
                    {
                        if (dbQuickAccess.Granted != qa.Granted || dbQuickAccess.DateBlock != qa.DateBlock)
                        {
                            dbQuickAccess.Granted = qa.Granted;
                            dbQuickAccess.DateBlock = qa.DateBlock;
                            _dbContext.Update(dbQuickAccess);
                        }
                    }
                    else
                    {
                        dbQuickAccess = new DB.QuickAccess() { Sn = qa.Sn, Reader = qa.Reader, Card = workerCard.CardNumb16 };
                        await _dbContext.AddAsync(dbQuickAccess, cancellationToken);
                    }
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        private void AddAccessToList(List<DB.QuickAccess> accesses, DB.Access access, string card, DateTime? dateBlock)
        {
            if (access.ControllerLocation != null && access.ControllerLocation.Controller != null)
            {
                for (var readerNumber = 1; readerNumber <= 2; readerNumber++)
                {
                    DateTime? dateBlockUtc = (dateBlock != null) ? dateBlock.Value.ToUniversalTime() : null;
                    accesses.Add(_mapper.Map(access, new DB.QuickAccess() { Id = default, Reader = readerNumber, Card = card, DateBlock = dateBlockUtc }));
                }
            }
        }
        private void AddGroupAccessToList(List<DB.QuickAccess> accesses, DB.GroupAccess access, string card, DateTime? dateBlock)
        {
            if (access.ControllerLocation != null && access.ControllerLocation.Controller != null)
            {
                for (var readerNumber = 1; readerNumber <= 2; readerNumber++)
                {
                    DateTime? dateBlockUtc = (dateBlock != null) ? dateBlock.Value.ToUniversalTime() : null;
                    accesses.Add(_mapper.Map(access, new DB.QuickAccess() { Id = default, Reader = readerNumber, Card = card, DateBlock = dateBlockUtc }));
                }
            }
        }
    }
}