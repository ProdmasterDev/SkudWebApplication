using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;
using VM = SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;
using SkudWebApplication.ViewModels;
using MudBlazor;
using SkudWebApplication.Requests.Controller;
using RestEase;
using System.Linq.Expressions;
using System;
using System.Globalization;

namespace SkudWebApplication.Services.Classes
{
    public class ControllerService : IControllerService
    {
        private readonly WebAppContext _dbContext;
        private readonly IMapper _mapper;
        public ControllerService(WebAppContext dbContext, IMapper mapper) 
        {
            _dbContext = dbContext; 
            _mapper = mapper;
        }
        public async Task<IEnumerable<ControllerLocation>> GetAvailableLocations(int? currentControllerId)
        {
            var entities = await _dbContext
               .Set<DB.ControllerLocation>()
               .Include(x => x.Controller)
               .AsNoTracking()
               .Where(x => x.Controller == null || (currentControllerId != null && x.Controller != null && x.Controller.Id == currentControllerId))
               .ToListAsync();
            return _mapper.Map<IEnumerable<ControllerLocation>>(entities);
        }
        public async Task<EditControllerRequest> GetEditRequest(Controller viewModel)
        {
            var entity = await _dbContext.Set<DB.Controller>()
                .Include(x => x.ControllerLocation)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Контроллер не найден!");
            }
            var request = _mapper.Map<DB.Controller, EditControllerRequest>(entity);
            request.Location = _mapper.Map<ControllerLocation>(entity.ControllerLocation);
            return request;
        }
        public async Task<MudBlazor.GridData<Controller>> GetGridData(ICollection<MudBlazor.SortDefinition<Controller>> sortDefinitions, ICollection<MudBlazor.IFilterDefinition<Controller>>? filterDefinitions, int pageNumber, int pageSize)
        {
            var dataRequest = _dbContext.Set<DB.Controller>()
                .AsNoTracking()
                .Include(x => x.ControllerLocation)
                .Where(x => !x.Arch)
                .Select(x => new VM.Controller()
                {
                    Id = x.Id,
                    Sn = x.Sn,
                    Type = x.Type,
                    IpAddress = x.IpAddress,
                    FwVer = x.FwVer,
                    ComFwVer = x.ComFwVer,
                    LastPing = x.LastPing,
                    LastPowerOn = x.LastPowerOn,
                    LocationId = x.ControllerLocationId,
                    LocationName = (x.ControllerLocation != null) ? x.ControllerLocation.Name : string.Empty,
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
                    dataRequest = dataRequest.OrderByDescending(p => EF.Property<VM.Controller>(p, sort.SortBy));
                }
                else
                {
                    dataRequest = dataRequest.OrderBy(p => EF.Property<VM.Controller>(p, sort.SortBy));
                }
            }

            var count = dataRequest.Count();

            dataRequest = dataRequest.Skip(pageSize * pageNumber).Take(pageSize);

            var data = await dataRequest.ToListAsync();

            return new GridData<VM.Controller>()
            {
                Items = data,
                TotalItems = count,
            };
        }
    }
}
