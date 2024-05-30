using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Controller;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Controller
{
    public class RefreshAccessesControllerHandler : IRequestHandler<RefreshAccessesControllerRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public RefreshAccessesControllerHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(RefreshAccessesControllerRequest request, CancellationToken cancellationToken)
        {
            if (request.Id == null) { return; }

            var controllerDb = await _dbContext
                .Set<DB.Controller>()
                .Include(x => x.ControllerLocation)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (controllerDb == null) { return; }

            if (controllerDb.ControllerLocation == null)
            {
                var quickAccessesToBeNotGranted = await _dbContext.Set<DB.QuickAccess>().Where(x => x.Sn == controllerDb.Sn).ToListAsync();
                quickAccessesToBeNotGranted.ForEach(x => x.Granted = 0);
                _dbContext.UpdateRange(quickAccessesToBeNotGranted);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return;
            }

            var workers = await _dbContext
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
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            List<DB.QuickAccess> newQuickAccesses = new();

            foreach(var worker in workers)
            {
                foreach(var card in worker.Cards)
                {
                    if (worker.AccessMethodId == 1)
                    {
                        worker.Accesses.ToList().ForEach(x => AddAccessToList(newQuickAccesses, x, card.CardNumb16, worker.DateBlock));
                    }

                    if (worker.AccessMethodId == 2 && worker.Group != null)
                    {
                        worker.Group.GroupAccess.ToList().ForEach(x => AddGroupAccessToList(newQuickAccesses, x, card.CardNumb16, worker.DateBlock));
                    }
                }
            }
            var quickAccessesRequest = _dbContext.Set<DB.QuickAccess>();
            foreach (var newQuickAccess in newQuickAccesses)
            {
                var dbQuickAccess = await quickAccessesRequest.FirstOrDefaultAsync(x => x.Sn == newQuickAccess.Sn && x.Reader == newQuickAccess.Reader && x.Card == newQuickAccess.Card, cancellationToken);
                if (dbQuickAccess != null)
                {
                    if (dbQuickAccess.Granted != newQuickAccess.Granted || dbQuickAccess.DateBlock != newQuickAccess.DateBlock)
                    {
                        dbQuickAccess.Granted = newQuickAccess.Granted;
                        dbQuickAccess.DateBlock = newQuickAccess.DateBlock;
                        _dbContext.Update(dbQuickAccess);
                    }
                }
                else
                {
                    dbQuickAccess = new DB.QuickAccess() { Sn =  newQuickAccess.Sn, Reader = newQuickAccess.Reader, Card = newQuickAccess.Card, Granted = newQuickAccess.Granted };
                    await _dbContext.AddAsync(dbQuickAccess, cancellationToken);
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
