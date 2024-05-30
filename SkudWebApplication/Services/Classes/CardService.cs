using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;
using VM = SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;
using MudBlazor;
using SkudWebApplication.Requests.Card;
using SkudWebApplication.Filters;

namespace SkudWebApplication.Services.Classes
{
    public class CardService : ICardService
    {
        private readonly WebAppContext _dbContext;
        private readonly IMapper _mapper;
        public CardService(WebAppContext dbContext, IMapper mapper) { _dbContext = dbContext; _mapper = mapper; }
        public async Task<IEnumerable<CardWorker>> GetAvailableWorkers(string? searchString)
        {
            var entities = _dbContext
                .Set<DB.Worker>()
                .OrderByDescending(x => x.Id)
                .AsNoTracking();

            if (searchString != null && searchString != string.Empty)
            {
                searchString = searchString.Trim().ToLower();
                entities = entities.Where(x =>
                    x.LastName.ToLower().Contains(searchString)
                    || x.FirstName.ToLower().Contains(searchString)
                    || x.FatherName.ToLower().Contains(searchString)
                    || x.Position.ToLower().Contains(searchString)
                    || x.Id.ToString().Contains(searchString)
                );
            }

            return _mapper.Map<IEnumerable<CardWorker>>(
                await entities
                .Take(10)
                .ToListAsync()
            );
        }
        public async Task<AddCardRequest> GetAddRequest()
        {
            return await Task.FromResult(new AddCardRequest());
        }
        public async Task<EditCardRequest> GetEditRequest(VM.Card viewModel)
        {
            var entity = await _dbContext.Set<DB.Card>()
                .Include(x => x.Worker)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Карта не найдена!");
            }
            var request = _mapper.Map<EditCardRequest>(entity);
            request.Worker = _mapper.Map<CardWorker>(entity.Worker);
            return request;
        }
        public async Task<MudBlazor.GridData<VM.Card>> GetGridData(ICollection<MudBlazor.SortDefinition<VM.Card>> sortDefinitions, ICollection<MudBlazor.IFilterDefinition<VM.Card>>? filterDefinitions, int pageNumber, int pageSize)
        {
            var dataRequest = _dbContext.Set<DB.Card>()
                .AsNoTracking()
                .Include(x => x.Worker)
                .Where(x => !x.Arch)
                .Select(x => new VM.Card()
                {
                    Id = x.Id,
                    CardNumber = x.CardNumb,
                    CardNumber16 = x.CardNumb16,
                    WorkerId = x.WorkerId,
                    FullName = (x.Worker != null) ? (x.Worker.LastName + " " + x.Worker.FirstName + " " + x.Worker.FatherName).Trim() : string.Empty,
                    LastName = (x.Worker != null) ? x.Worker.LastName : string.Empty,
                    FirstName = (x.Worker != null) ? x.Worker.FirstName : string.Empty,
                    FatherName = (x.Worker != null) ? x.Worker.FatherName : string.Empty,
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
                dataRequest = dataRequest.OrderByDescending(p => p.Id);
            }
            foreach (var sort in sortDefinitions)
            {
                if (sort.Descending)
                {
                    dataRequest = dataRequest.OrderByDescending(p => EF.Property<VM.Card>(p, sort.SortBy));
                }
                else
                {
                    dataRequest = dataRequest.OrderBy(p => EF.Property<VM.Card>(p, sort.SortBy));
                }
            }

            var count = dataRequest.Count();

            dataRequest = dataRequest.Skip(pageSize * pageNumber).Take(pageSize);

            var data = await dataRequest.ToListAsync();

            return new GridData<VM.Card>()
            {
                Items = data,
                TotalItems = count,
            };
        }
    }
}
