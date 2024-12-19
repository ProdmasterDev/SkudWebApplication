using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;
using VM = SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;
using MudBlazor;
using SkudWebApplication.Requests;
using Requests = SkudWebApplication.Requests;
using SkudWebApplication.Requests.WorkerGroup;
using SkudWebApplication.Filters;

namespace SkudWebApplication.Services.Classes
{
    public class WorkerGroupService : IWorkerGroupService
    {
        private readonly WebAppContext _dbContext;
        private readonly IMapper _mapper;
        public WorkerGroupService(WebAppContext dbContext, IMapper mapper) { _dbContext = dbContext; _mapper = mapper; }

        public async Task<AddWorkerGroupRequest> GetAddRequest()
        {
            return new AddWorkerGroupRequest()
            {
                Accesses = await _dbContext
                    .Set<DB.ControllerLocation>()
                    .Select(x => new AccessRequest()
                    {
                        ControllerLocationId = x.Id,
                        Enterance = false,
                        Exit = false,
                        LocationName = x.Name,
                    })
                    .AsNoTracking()
                    .ToListAsync(),
                WorkerGroupAccess = await _dbContext
                    .Set<DB.AccessGroup>()
                    .Select(x => new AccessGroupWorker()
                    {
                        AccessGroupId = x.Id,
                        isActive = false,
                        AccessGroupName = x.Name,
                    })
                    .AsNoTracking()
                    .ToListAsync(),
            };
        }
        public async Task<EditWorkerGroupRequest> GetEditRequest(VM.WorkerGroup viewModel)
        {
            var entity = await _dbContext.Set<DB.WorkerGroup>()
                .Include(x => x.GroupAccess)
                    .ThenInclude(x => x.ControllerLocation)
                .Include(x => x.WorkerGroupAccess)
                    .ThenInclude(x => x.AccessGroup)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == viewModel.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Подразделение не найдено!");
            }
            var request = _mapper.Map<DB.WorkerGroup, EditWorkerGroupRequest>(entity);
            request.Accesses = _mapper.Map<IEnumerable<DB.GroupAccess>, IEnumerable<AccessRequest>>(entity.GroupAccess);
            request.WorkerGroupAccess = _mapper.Map<IEnumerable<DB.WorkerGroupAccess>, IEnumerable<AccessGroupWorker>>(entity.WorkerGroupAccess);
            var availableAccesses = await _dbContext
            .Set<DB.ControllerLocation>()
            .Select(x => new AccessRequest()
            {
                ControllerLocationId = x.Id,
                Enterance = false,
                Exit = false,
                LocationName = x.Name,
            })
            .AsNoTracking()
            .ToListAsync();
            var avaibleAccessGroup = await _dbContext
            .Set<DB.AccessGroup>()
            .Select(x => new AccessGroupWorker()
            {
                AccessGroupId = x.Id,
                isActive = false,
                AccessGroupName = x.Name,
            })
            .AsNoTracking()
            .ToListAsync();
            var toAddGroups = avaibleAccessGroup.Except(request.WorkerGroupAccess, new AccessGroupComparer()).ToList();
            request.WorkerGroupAccess = request.WorkerGroupAccess.Union(toAddGroups);
            var toAdd = availableAccesses.Except(request.Accesses, new AccessComparer()).ToList();
            request.Accesses = request.Accesses.Union(toAdd);
            return request;
        }
        public async Task<GridData<VM.WorkerGroup>> GetGridData(ICollection<MudBlazor.SortDefinition<VM.WorkerGroup>> sortDefinitions, ICollection<MudBlazor.IFilterDefinition<VM.WorkerGroup>>? filterDefinitions, int pageNumber, int pageSize)
        {
            var dataRequest = _dbContext.Set<DB.WorkerGroup>()
                .Include(x => x.GroupAccess)
                .AsNoTracking()
                .Where(x => !x.Arch)
                .AsQueryable();

            if (filterDefinitions != null)
            {
                foreach (var filter in filterDefinitions)
                {
                    dataRequest = dataRequest.Filter(filter);
                }
            }

            if (sortDefinitions.Count == 0)
            {
                dataRequest = dataRequest.OrderBy(p => p.Id);
            }
            foreach (var sort in sortDefinitions)
            {
                if (sort.Descending)
                {
                    dataRequest = dataRequest.OrderByDescending(p => EF.Property<DB.WorkerGroup>(p, sort.SortBy));
                }
                else
                {
                    dataRequest = dataRequest.OrderBy(p => EF.Property<DB.WorkerGroup>(p, sort.SortBy));
                }
            }

            var count = dataRequest.Count();

            dataRequest = dataRequest.Skip(pageSize * pageNumber).Take(pageSize);

            var data = _mapper.Map<IEnumerable<DB.WorkerGroup>, IEnumerable<VM.WorkerGroup>>(await dataRequest.ToListAsync());

            return new GridData<VM.WorkerGroup>()
            {
                Items = data,
                TotalItems = count,
            };
        }
    }
}
