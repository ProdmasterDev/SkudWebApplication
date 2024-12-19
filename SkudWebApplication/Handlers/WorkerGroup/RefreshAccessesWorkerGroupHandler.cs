using AutoMapper;
using ControllerDomain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.WorkerGroup;
using SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.WorkerGroup
{
    public class RefreshAccessesWorkerGroupHandler : IRequestHandler<RefreshAccessesWorkerGroupRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public RefreshAccessesWorkerGroupHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Handle(RefreshAccessesWorkerGroupRequest request, CancellationToken cancellationToken)
        {
            if (request.Id == null) { return; }

            var cards = await _dbContext
                .Set<DB.Worker>()
                .Include(x => x.Cards)
                .AsNoTracking()
                .Where(x => x.GroupId == request.Id && x.AccessMethodId == 2)
                .SelectMany(x => x.Cards)
                .ToListAsync(cancellationToken);

            var workerGroup = await _dbContext
                .Set<DB.WorkerGroup>()
                .Include(x => x.GroupAccess)
                    .ThenInclude(x => x.ControllerLocation)
                        .ThenInclude(x => x.Controller)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            var workerGroupAccess = await _dbContext
                .Set<DB.WorkerGroupAccess>()
                .Include(x => x.AccessGroup)
                    .ThenInclude(x => x.Accesses)
                        .ThenInclude(x => x.ControllerLocation)
                                .ThenInclude(x => x.Controller)
                .AsNoTracking()
                .Where(x => x.WorkerGroupId == request.Id)
                .ToListAsync(cancellationToken);


            List<DB.QuickAccess> newQuickAccesses = [];

            //if (workerGroup != null)
            //{
            //    workerGroup.GroupAccess.ToList().ForEach(x => AddGroupAccessToList(newQuickAccesses, x));
            //}
            foreach (var groupsAccess in workerGroupAccess)
            {
                if (groupsAccess.isActive)
                {
                    var accessGroup = groupsAccess.AccessGroup.Accesses;
                    accessGroup.ToList().ForEach(x => AddAccessGroupToList(newQuickAccesses, x));
                }
            }

            var quickAccessesRequest = _dbContext.Set<DB.QuickAccess>();
            foreach (var card in cards)
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
                        dbQuickAccess = new DB.QuickAccess() { Sn = qa.Sn, Reader = qa.Reader, Card = card.CardNumb16 };
                        await _dbContext.AddAsync(dbQuickAccess, cancellationToken);
                    }
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        private void AddGroupAccessToList(List<DB.QuickAccess> accesses, DB.GroupAccess access)
        {
            if (access.ControllerLocation != null && access.ControllerLocation.Controller != null)
            {
                for (var readerNumber = 1; readerNumber <= 2; readerNumber++)
                {
                    accesses.Add(_mapper.Map(access, new DB.QuickAccess() { Id = default, Reader = readerNumber}));
                }
            }
        }
        private void AddAccessGroupToList(List<DB.QuickAccess> accesses, DB.AccessGroupAccess access)
        {
            if (access.ControllerLocation != null && access.ControllerLocation.Controller != null)
            {
                for (var readerNumber = 1; readerNumber <= 2; readerNumber++)
                {
                    var oldQuickAccess = accesses.FirstOrDefault(x => x.Sn == access.ControllerLocation.Controller.Sn && x.Reader == readerNumber);
                    if (oldQuickAccess == null || oldQuickAccess.Granted == 0) 
                    {
                        accesses.Add(_mapper.Map(access, new DB.QuickAccess() { Id = default, Reader = readerNumber }));
                    }
                }
            }
        }
    }
}

