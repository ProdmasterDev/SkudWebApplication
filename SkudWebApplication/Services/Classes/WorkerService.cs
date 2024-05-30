using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;
using VM = SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;
using SkudWebApplication.ViewModels;
using SkudWebApplication.Requests.Worker;
using SkudWebApplication.Requests;
using System.Linq;
using SkudWebApplication.Filters;
using ControllerDomain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SkudWebApplication.Services.Classes
{
    public class WorkerService : IWorkerService
    {
        private readonly WebAppContext _dbContext;
        private readonly IMapper _mapper;
        public WorkerService(WebAppContext dbContext, IMapper mapper) { _dbContext = dbContext; _mapper = mapper; }
        public async Task<IEnumerable<VM.AccessMethod>> GetAccessMethodsAsync()
        {
            return _mapper.Map<IEnumerable<DB.AccessMethod>, IEnumerable<VM.AccessMethod>>(await _dbContext.Set<DB.AccessMethod>().AsNoTracking().OrderBy(x => x.Id).ToListAsync());
        }

        public async Task<AddWorkerRequest> GetAddRequest()
        {
            return new AddWorkerRequest
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

        public async Task<IEnumerable<WorkerAccessMethod>> GetAvailableAccessMethods()
        {
            var entities = await _dbContext
                .Set<DB.AccessMethod>()
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();
            if (entities == null)
            {
                return new List<WorkerAccessMethod>();
            }
            return _mapper.Map<IEnumerable<WorkerAccessMethod>>(entities);
        }

        public async Task<IEnumerable<Requests.Worker.WorkerGroup>> GetAvailableGroups(string? searchString)
        {
            var request = _dbContext
                .Set<DB.WorkerGroup>()
                .AsNoTracking();
            if (searchString != null)
            {
                searchString = searchString.Trim().ToLower();
                request = request.Where(x => x.Name.ToLower().Contains(searchString));
            }
            var entities = await request.Take(10).ToListAsync();
            if (entities == null)
            {
                return new List<Requests.Worker.WorkerGroup>();
            }
            return _mapper.Map<IEnumerable<Requests.Worker.WorkerGroup>>(entities);
        }
        public async Task<EditWorkerRequest> GetEditRequest(VM.Worker viewModel)
        {
            var entity = await _dbContext
                .Set<DB.Worker>()
                .Include(x => x.Cards)
                .Include(x => x.Accesses)
                    .ThenInclude(x => x.ControllerLocation)
                .Include(x => x.Group)
                .Include(x => x.AccessMethod)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Группа доступа не найдена!");
            }
            var request = _mapper.Map<EditWorkerRequest>(entity);
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

        public async Task<MudBlazor.GridData<VM.Worker>> GetGridData(ICollection<MudBlazor.SortDefinition<VM.Worker>> sortDefinitions, ICollection<MudBlazor.IFilterDefinition<VM.Worker>>? filterDefinitions, int pageNumber, int pageSize)
        {
            var dataRequest = _dbContext.Set<DB.Worker>()
                .Include(x => x.Cards)
                .Include(x => x.Group)
                .AsNoTracking()
                .Where(x => !x.Arch)
                .Select(x => new VM.Worker
                {
                    Id = x.Id,
                    LastName = x.LastName,
                    FirstName = x.FirstName,
                    FatherName = x.FatherName,
                    Position = x.Position,
                    PhotoPath = x.ImagePath,
                    Comment = x.Comment,
                    AccessMethodId = x.AccessMethodId,
                    Cards = x.Cards.Select(y => new VM.WorkerCard() { Id = y.Id, Number = y.CardNumb, Number16 = y.CardNumb16 }),
                    Group = (x.Group == null) ? string.Empty : x.Group.Name,
                    AccessMethods = new VM.WorkerAccessMethods()
                })
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
                    dataRequest = dataRequest.OrderByDescending(p => EF.Property<VM.Worker>(p, sort.SortBy));
                }
                else
                {
                    dataRequest = dataRequest.OrderBy(p => EF.Property<VM.Worker>(p, sort.SortBy));
                }
            }

            var count = dataRequest.Count();

            if (pageNumber > 1)
                dataRequest = dataRequest.Skip(pageSize * (pageNumber - 1));

            dataRequest = dataRequest.Take(pageSize);

            var accessMethods = _mapper.Map<IEnumerable<DB.AccessMethod>, IEnumerable<VM.AccessMethod>>(await _dbContext.Set<DB.AccessMethod>().ToListAsync());

            var data = await dataRequest.ToListAsync();

            foreach (var worker in data)
            {
                worker.AccessMethods.List=accessMethods;
                var selectedAccessMethod = worker.AccessMethods.List.FirstOrDefault(x => worker.AccessMethodId != null && worker.AccessMethodId == x.Id);
                if (selectedAccessMethod != null)
                    worker.AccessMethods.Selected = selectedAccessMethod.Id;
            }

            return new MudBlazor.GridData<VM.Worker>()
            {
                Items = data,
                TotalItems = count,
            };
        }

        public async Task<IEnumerable<VM.Worker>> GetWorkersAsync()
        {
            var data = await _dbContext.Set<DB.Worker>()
                .Include(x => x.Cards)
                .Include(x => x.Group)
                .AsNoTracking()
                .Where(x => !x.Arch)
                .Select(x => new VM.Worker
                {
                    Id = x.Id,
                    FullName = ($"{x.LastName} {x.FirstName} {x.FatherName}").Trim(),
                    LastName = x.LastName,
                    FirstName = x.FirstName,
                    FatherName = x.FatherName,
                    Position = x.Position,
                    PhotoPath = x.ImagePath,
                    Comment = x.Comment,
                    AccessMethodId = x.AccessMethodId,
                    Cards = x.Cards.Select(y => new VM.WorkerCard() { Id = y.Id, Number = y.CardNumb, Number16 = y.CardNumb16 }),
                    Group = (x.Group == null) ? string.Empty : x.Group.Name,
                    AccessMethods = new VM.WorkerAccessMethods(),
                    DateBlock = (x.DateBlock != null) ? x.DateBlock.Value.ToLocalTime() : null,
                }).ToListAsync();
            var accessMethods = _mapper.Map<IEnumerable<VM.AccessMethod>>(await _dbContext.Set<DB.AccessMethod>().ToListAsync());
            foreach (var worker in data)
            {
                worker.AccessMethods.List = accessMethods;
                var selectedAccessMethod = worker.AccessMethods.List.FirstOrDefault(x => worker.AccessMethodId != null && worker.AccessMethodId == x.Id);
                if (selectedAccessMethod != null)
                    worker.AccessMethods.Selected = selectedAccessMethod.Id;
            }
            return data;
        }
    }
}
