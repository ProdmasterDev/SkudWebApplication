using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests;
using SkudWebApplication.Requests.Worker;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Worker
{
    public class RefreshAccessesWorkerHandler : IRequestHandler<RefreshAccessesWorkerRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public RefreshAccessesWorkerHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(RefreshAccessesWorkerRequest request, CancellationToken cancellationToken)
        {
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
                .Include(x=>x.WorkerAccessGroup)
                    .ThenInclude(x => x.AccessGroup)
                        .ThenInclude(x => x.Accesses)
                            .ThenInclude(x => x.ControllerLocation)
                                .ThenInclude(x => x.Controller)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (dbWorker == null) { return; }

            List<DB.QuickAccess> newQuickAccesses = new();


            if (dbWorker.AccessMethodId == 1)
            {
                dbWorker.Accesses.ToList().ForEach(x => AddAccessToList(newQuickAccesses, x, string.Empty, dbWorker.DateBlock));
            }

            if (dbWorker.AccessMethodId == 2 && dbWorker.Group != null)
            {
                dbWorker.Group.GroupAccess.ToList().ForEach(x => AddGroupAccessToList(newQuickAccesses, x, string.Empty, dbWorker.DateBlock));
            }
            if (dbWorker.AccessMethodId == 3 && dbWorker.WorkerAccessGroup != null)
            {
                foreach (var workerAccessGroup in dbWorker.WorkerAccessGroup)
                {
                    if (workerAccessGroup.isActive)
                    {
                        var accessGroup = workerAccessGroup.AccessGroup.Accesses;
                        accessGroup.ToList().ForEach(x => AddAccessGroupToList(newQuickAccesses, x, string.Empty, dbWorker.DateBlock));
                    }
                }

                //var availableAccesses = await _dbContext
                //.Set<DB.ControllerLocation>()
                //.Include(x => x.Controller)
                //.Select(x => new DB.AccessGroupAccess()
                //{
                //    ControllerLocationId = x.Id,
                //    Enterance = false,
                //    Exit = false,
                //    ControllerLocation = x,
                //})
                //.AsNoTracking()
                //.ToListAsync();
                //List<DB.QuickAccess> oldQuickAccesses = new();
                //availableAccesses.ToList().ForEach(x => AddAccessGroupToList(oldQuickAccesses, x, string.Empty, dbWorker.DateBlock));
            }

            var quickAccessesRequest = _dbContext.Set<DB.QuickAccess>();
            foreach (var card in dbWorker.Cards)
            {
                if(newQuickAccesses.Count == 0)
                {
                    var dbQuickAccesses = await quickAccessesRequest.Where(x => x.Card == card.CardNumb16).ToListAsync(cancellationToken);
                    dbQuickAccesses.ForEach(x => x.Granted = 0);
                    _dbContext.UpdateRange(dbQuickAccesses);
                }
                foreach(var qa in newQuickAccesses)
                {
                    var dbQuickAccess = await quickAccessesRequest.FirstOrDefaultAsync(x => x.Sn == qa.Sn && x.Reader == qa.Reader && x.Card == card.CardNumb16, cancellationToken);
                    if(dbQuickAccess != null)
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
                        var dateBlock = dbWorker.DateBlock;
                        if (dateBlock != null)
                            dateBlock = dateBlock.Value.ToUniversalTime();
                        dbQuickAccess = new DB.QuickAccess() { Sn = qa.Sn, Reader = qa.Reader, Card = card.CardNumb16, DateBlock = dateBlock };
                        await _dbContext.AddAsync(dbQuickAccess, cancellationToken);
                    }
                }
            }
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch(Exception ex)
            {

            }
        }

        private void AddAccessToList(List<DB.QuickAccess> accesses, DB.Access access, string card, DateTime? dateBlock)
        {
            if (access.ControllerLocation != null && access.ControllerLocation.Controller != null)
            {
                for (var readerNumber = 1; readerNumber <= 2; readerNumber++)
                {
                    if (dateBlock != null)
                        dateBlock = dateBlock.Value.ToUniversalTime();
                    accesses.Add(_mapper.Map(access, new DB.QuickAccess() { Id = default, Reader = readerNumber, Card = card, DateBlock = dateBlock }));
                }
            }
        }
        private void AddGroupAccessToList(List<DB.QuickAccess> accesses, DB.GroupAccess access, string card, DateTime? dateBlock)
        {
            if (access.ControllerLocation != null && access.ControllerLocation.Controller != null)
            {
                for (var readerNumber = 1; readerNumber <= 2; readerNumber++)
                {
                    if(dateBlock != null)
                        dateBlock = dateBlock.Value.ToUniversalTime();
                    accesses.Add(_mapper.Map(access, new DB.QuickAccess() { Id = default, Reader = readerNumber, Card = card, DateBlock = dateBlock }));
                }
            }
        }
        private void AddAccessGroupToList(List<DB.QuickAccess> accesses, DB.AccessGroupAccess access, string card, DateTime? dateBlock)
        {
            if (access.ControllerLocation != null && access.ControllerLocation.Controller != null)
            {
                for (var readerNumber = 1; readerNumber <= 2; readerNumber++)
                {
                    if (dateBlock != null)
                        dateBlock = dateBlock.Value.ToUniversalTime();
                    var oldQuickAccess = accesses.FirstOrDefault(x => x.Sn == access.ControllerLocation.Controller.Sn && x.Reader == readerNumber);
                    if (oldQuickAccess == null || oldQuickAccess.Granted == 0)
                    { 
                        accesses.Add(_mapper.Map(access, new DB.QuickAccess() { Id = default, Reader = readerNumber, Card = card, DateBlock = dateBlock }));
                    }
                }
            }
        }
    }
}
