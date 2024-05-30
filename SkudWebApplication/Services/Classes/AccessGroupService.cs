using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;
using DB = ControllerDomain.Entities;
using VM = SkudWebApplication.ViewModels;
using SkudWebApplication.ViewModels;
using MudBlazor;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.Requests.WorkerGroup;
using SkudWebApplication.Requests;
using static MudBlazor.Colors;
using SkudWebApplication.Filters;

namespace SkudWebApplication.Services.Classes
{
    public class AccessGroupService : IAccessGroupService
    {
        private readonly WebAppContext _dbContext;
        private readonly IMapper _mapper;
        public AccessGroupService(WebAppContext dbContext, IMapper mapper) { _dbContext = dbContext; _mapper = mapper; }

        public async Task<AddAccessGroupRequest> GetAddRequest()
        {
            return new AddAccessGroupRequest()
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
            };
        }

        public async Task<EditAccessGroupRequest> GetEditRequest(AccessGroup viewModel)
        {
            var entity = await _dbContext.Set<DB.AccessGroup>()
                .Include(x => x.Accesses)
                .ThenInclude(x => x.ControllerLocation)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Группа доступа не найдена!");
            }
            var request = _mapper.Map<DB.AccessGroup, EditAccessGroupRequest>(entity);
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
            var toAdd = availableAccesses.Except(request.Accesses, new AccessComparer()).ToList();
            request.Accesses = request.Accesses.Union(toAdd);
            return request;
        }

        public async Task<MudBlazor.GridData<AccessGroup>> GetGridData(ICollection<MudBlazor.SortDefinition<AccessGroup>> sortDefinitions, ICollection<MudBlazor.IFilterDefinition<AccessGroup>>? filterDefinitions, int pageNumber, int pageSize)
        {
            var dataRequest = _dbContext.Set<DB.AccessGroup>()
                .Include(x => x.Accesses)
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
                    dataRequest = dataRequest.OrderByDescending(p => EF.Property<DB.AccessGroup>(p, sort.SortBy));
                }
                else
                {
                    dataRequest = dataRequest.OrderBy(p => EF.Property<DB.AccessGroup>(p, sort.SortBy));
                }
            }

            var count = dataRequest.Count();

            dataRequest = dataRequest.Skip(pageSize * pageNumber).Take(pageSize);

            var data = _mapper.Map<IEnumerable<DB.AccessGroup>, IEnumerable<VM.AccessGroup>>(await dataRequest.ToListAsync());

            return new GridData<AccessGroup>()
            {
                Items = data,
                TotalItems = count,
            };
        }
    }
}
