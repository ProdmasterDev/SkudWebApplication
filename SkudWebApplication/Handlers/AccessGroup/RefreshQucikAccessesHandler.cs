using AutoMapper;
using ControllerDomain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.AccessGroup
{
    public class RefreshQuickAccessesHanlder : IRequestHandler<RefreshQuickAccessesRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public RefreshQuickAccessesHanlder(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(RefreshQuickAccessesRequest request, CancellationToken cancellationToken)
        {
            if (request.Id == null) { return; }

            //Обновление доступа для всех сотрудников, у которых метод группы доступа и привязана редактируемая группа
            var dbWorkers = await _dbContext
               .Set<DB.Worker>()
               .Include(x => x.Cards)
               .Include(x => x.Accesses)
                   .ThenInclude(x => x.ControllerLocation)
               .Include(x => x.Group)
               .Include(x => x.AccessMethod)
               .Include(x => x.WorkerAccessGroup)
                   .ThenInclude(x => x.AccessGroup)
                        .ThenInclude(x => x.Accesses)
                            .ThenInclude(x => x.ControllerLocation)
                                .ThenInclude(x => x.Controller)
               .AsNoTracking()
               .Where(x => x.WorkerAccessGroup.Any(wag => wag.AccessGroup.Id == request.Id) && x.AccessMethodId == 3)
                .ToListAsync(cancellationToken);

            List<DB.QuickAccess> newQuickAccesses = [];

            foreach (var worker in dbWorkers) {
                if (worker.AccessMethodId == 3 && worker.WorkerAccessGroup != null)

                    foreach (var workeraccessgroup in worker.WorkerAccessGroup)
                    {
                        if (workeraccessgroup.isActive)
                        {
                            var accessgroup = workeraccessgroup.AccessGroup.Accesses;
                            accessgroup.ToList().ForEach(x => AddAccessGroupToList(newQuickAccesses, x, string.Empty, worker.DateBlock));
                        }
                    }

                var quickAccessesRequest = _dbContext.Set<DB.QuickAccess>();
                foreach (var card in worker.Cards)
                {
                    if (newQuickAccesses.Count == 0)
                    {
                        var dbQuickAccesses = await quickAccessesRequest.Where(x => x.Card == card.CardNumb16).ToListAsync(cancellationToken);
                        dbQuickAccesses.ForEach(x => x.Granted = 0);
                        _dbContext.UpdateRange(dbQuickAccesses);
                    }
                    foreach (var qa in newQuickAccesses)
                    {
                        var dbQuickAccess = await quickAccessesRequest.FirstOrDefaultAsync(x => x.Sn == qa.Sn && x.Reader == qa.Reader && x.Card == card.CardNumb16, cancellationToken);
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
                            var dateBlock = worker.DateBlock;
                            if (dateBlock != null)
                                dateBlock = dateBlock.Value.ToUniversalTime();
                            dbQuickAccess = new DB.QuickAccess() { Sn = qa.Sn, Reader = qa.Reader, Card = card.CardNumb16, DateBlock = dateBlock };
                            await _dbContext.AddAsync(dbQuickAccess, cancellationToken);
                        }
                    }
                }

            }

            //Обновление доступа для всех сотрудников, у которых в подразделении выбрана редактируемая группа
            var dbWorkers2 = await _dbContext
                 .Set<DB.Worker>()
                 .Include(x => x.Cards)
                 .Include(x => x.Accesses)
                     .ThenInclude(x => x.ControllerLocation)
                 .Include(x => x.Group)
                    .ThenInclude(x => x.WorkerGroupAccess)
                        .ThenInclude(x => x.AccessGroup)
                            .ThenInclude(x => x.Accesses)
                                .ThenInclude(x => x.ControllerLocation)
                                        .ThenInclude(x => x.Controller)
                 .AsNoTracking()
                 .Where(x => x.Group.WorkerGroupAccess.Any(wag => wag.AccessGroup.Id == request.Id))
                  .ToListAsync(cancellationToken);

            List<DB.QuickAccess> newQuickAccesses2 = [];

            foreach (var worker in dbWorkers2)
            {
                if (worker.AccessMethodId == 2 && worker.Group != null)

                    foreach (var workeraccessgroup  in worker.Group.WorkerGroupAccess)
                    {
                        if (workeraccessgroup.isActive)
                        {
                            var accessgroup = workeraccessgroup.AccessGroup.Accesses;
                            accessgroup.ToList().ForEach(x => AddAccessGroupToList(newQuickAccesses2, x, string.Empty, worker.DateBlock));
                        }
                    }

                var quickAccessesRequest = _dbContext.Set<DB.QuickAccess>();
                foreach (var card in worker.Cards)
                {
                    if (newQuickAccesses.Count == 0)
                    {
                        var dbQuickAccesses = await quickAccessesRequest.Where(x => x.Card == card.CardNumb16).ToListAsync(cancellationToken);
                        dbQuickAccesses.ForEach(x => x.Granted = 0);
                        _dbContext.UpdateRange(dbQuickAccesses);
                    }
                    foreach (var qa in newQuickAccesses)
                    {
                        var dbQuickAccess = await quickAccessesRequest.FirstOrDefaultAsync(x => x.Sn == qa.Sn && x.Reader == qa.Reader && x.Card == card.CardNumb16, cancellationToken);
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
                            var dateBlock = worker.DateBlock;
                            if (dateBlock != null)
                                dateBlock = dateBlock.Value.ToUniversalTime();
                            dbQuickAccess = new DB.QuickAccess() { Sn = qa.Sn, Reader = qa.Reader, Card = card.CardNumb16, DateBlock = dateBlock };
                            await _dbContext.AddAsync(dbQuickAccess, cancellationToken);
                        }
                    }
                }

            }


            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

            }

            

           // List<DB.QuickAccess> newQuickAccesses = [];
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

