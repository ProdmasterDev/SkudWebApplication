using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;
using VM = SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;
using SkudWebApplication.ViewModels;
using MudBlazor;
using SkudWebApplication.Requests.Location;

namespace SkudWebApplication.Services.Classes
{
    public class LocationService : ILocationService
    {
        private readonly WebAppContext _dbContext;
        private readonly IMapper _mapper;
        public LocationService(WebAppContext dbContext, IMapper mapper) { _dbContext = dbContext; _mapper = mapper; }
        public async Task<AddLocationRequest> GetAddRequest()
        {
            return await Task.FromResult(new AddLocationRequest());
        }
        public async Task<EditLocationRequest> GetEditRequest(VM.Location viewModel)
        {
            var entity = await _dbContext.Set<DB.ControllerLocation>()
                .Include(x => x.Controller)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);

            if (entity == null)
            {
                throw new KeyNotFoundException("Место установки не найдено!");
            }

            return _mapper.Map<DB.ControllerLocation, EditLocationRequest>(entity);
        }

        public async Task<MudBlazor.GridData<Location>> GetGridData(ICollection<MudBlazor.SortDefinition<Location>> sortDefinitions, ICollection<MudBlazor.IFilterDefinition<Location>>? filterDefinitions, int pageNumber, int pageSize)
        {
            var dataRequest = _dbContext.Set<DB.ControllerLocation>()
                .AsNoTracking()
                .Include(x => x.Controller)
                .Where(x => !x.Arch)
                .Select(x => new VM.Location()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ControllerId = (x.Controller != null) ? x.Controller.Id : null,
                    ControllerSn = (x.Controller != null) ? x.Controller.Sn : string.Empty,
                    ControllerIp = (x.Controller != null) ? x.Controller.IpAddress : string.Empty,
                })
                .AsQueryable();

            if (sortDefinitions.Count == 0)
            {
                dataRequest = dataRequest.OrderBy(p => p.Id);
            }
            foreach (var sort in sortDefinitions)
            {
                if (sort.Descending)
                {
                    dataRequest = dataRequest.OrderByDescending(p => EF.Property<DB.ControllerLocation>(p, sort.SortBy));
                }
                else
                {
                    dataRequest = dataRequest.OrderBy(p => EF.Property<DB.ControllerLocation>(p, sort.SortBy));
                }
            }

            var count = dataRequest.Count();

            dataRequest = dataRequest.Skip(pageSize * pageNumber).Take(pageSize);

            var data = await dataRequest.ToListAsync();

            return new GridData<Location>()
            {
                Items = data,
                TotalItems = count,
            };
        }

        public async Task<IEnumerable<LocationController>> GetAvailableControllers(int? currentLocationId)
        {
            var entities = await _dbContext
                .Set<DB.Controller>()
                .Include(x => x.ControllerLocation)
                .AsNoTracking()
                .Where(x => x.ControllerLocation == null || (currentLocationId != null && x.ControllerLocationId == currentLocationId))
                .ToListAsync();
            return _mapper.Map<IEnumerable<LocationController>>(entities);
        }
    }
}
