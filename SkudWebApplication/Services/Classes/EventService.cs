using AutoMapper;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;
using VM = SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;
using SortDirection = BlazorBootstrap.SortDirection;
using SkudWebApplication.ViewModels;
using MudBlazor;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SkudWebApplication.Filters;
using System.Globalization;
using System.Linq.Expressions;
using MiniExcelLibs;

namespace SkudWebApplication.Services.Classes
{
    public static class DbContextState
    {
        public static bool IsDbContextBusy { get; set; } = false;
    }
    public class EventService : IEventService
    {
        private readonly WebAppContext _dbContext;
        private readonly IMapper _mapper;
        private bool _isDbContextBusy = false;
        public EventService(WebAppContext dbContext, IMapper mapper) { _dbContext = dbContext; _mapper = mapper; }
        public async Task<MudBlazor.GridData<Event>> GetReport(ICollection<MudBlazor.SortDefinition<Event>> sortDefinitions, ICollection<MudBlazor.IFilterDefinition<Event>>? filterDefinitions, int pageNumber, int pageSize, AdditionalFiltersEvent? additionalFiltersEvent)
        {
            var dataRequest = _dbContext
                .Set<DB.Event>()
                .AsNoTracking()
                .Include(x => x.Worker)
                .Include(x => x.ControllerLocation)
                .Include(x => x.EventType)
                .Select(x => new VM.Event
                {
                    Id = x.Id,
                    Create = x.Create,
                    Flags = x.Flag,
                    WorkerId = x.WorkerId,
                    CardNumber16 = x.Card,
                    EventTypeName = (x.EventType != null) ? x.EventType.Name : string.Empty,
                    LocationName = (x.ControllerLocation != null) ? x.ControllerLocation.Name : string.Empty,
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

            if (additionalFiltersEvent != null)
            {
                if (additionalFiltersEvent.ControllerLocationsFilter.ControllerLocationSelect.FirstOrDefault(x => !x.Selected) != null)
                {
                    var locationNames = additionalFiltersEvent.ControllerLocationsFilter.ControllerLocationSelect.Where(x => x.Selected).Select(x => x.Location.Name);

                    dataRequest = dataRequest
                        .Where(x => locationNames.Contains(x.LocationName));
                }
                if (additionalFiltersEvent.WorkerGroupsFilter.WorkerGroupSelects.FirstOrDefault(x => !x.Selected) != null)
                {
                    var groupIds = additionalFiltersEvent.WorkerGroupsFilter.WorkerGroupSelects.Where(x => x.Selected).Select(x => x.WorkerGroup.Id);

                    var workers = _dbContext.Set<DB.Worker>()
                        .Include(x => x.Group)
                        .Where(x => x.GroupId != null && groupIds.Contains((int)x.GroupId))
                        .Select(x => x.Id);

                    dataRequest = dataRequest
                        .Where(x => x.WorkerId != null && workers.Contains((int)x.WorkerId));
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
                    dataRequest = dataRequest.OrderByDescending(p => EF.Property<VM.Event>(p, sort.SortBy));
                }
                else
                {
                    dataRequest = dataRequest.OrderBy(p => EF.Property<VM.Event>(p, sort.SortBy));
                }
            }

            var count = await dataRequest.CountAsync();


            

            var data = await dataRequest.ToListAsync();

            return new GridData<Event>()
            {
                Items = data,
                TotalItems = count,
            };
        }
        public async Task<MudBlazor.GridData<Event>> GetGridData(ICollection<MudBlazor.SortDefinition<Event>> sortDefinitions, ICollection<MudBlazor.IFilterDefinition<Event>>? filterDefinitions, int pageNumber, int pageSize, AdditionalFiltersEvent? additionalFiltersEvent)
        {
            var dataRequest = _dbContext
                .Set<DB.Event>()
                .AsNoTracking()
                .Include(x => x.Worker)
                .Include(x => x.ControllerLocation)
                .Include(x => x.EventType)
                .Select(x => new VM.Event
                {
                    Id = x.Id,
                    Create = x.Create,
                    Flags = x.Flag,
                    WorkerId = x.WorkerId,
                    CardNumber16 = x.Card,
                    EventTypeName = (x.EventType != null) ? x.EventType.Name : string.Empty,
                    LocationName = (x.ControllerLocation != null) ? x.ControllerLocation.Name : string.Empty,
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

            if (additionalFiltersEvent != null)
            {
                if (additionalFiltersEvent.ControllerLocationsFilter.ControllerLocationSelect.FirstOrDefault(x => !x.Selected) != null)
                {
                    var locationNames = additionalFiltersEvent.ControllerLocationsFilter.ControllerLocationSelect.Where(x => x.Selected).Select(x => x.Location.Name);

                    dataRequest = dataRequest
                        .Where(x => locationNames.Contains(x.LocationName));
                }
                if (additionalFiltersEvent.WorkerGroupsFilter.WorkerGroupSelects.FirstOrDefault(x => !x.Selected) != null)
                {
                    var groupIds = additionalFiltersEvent.WorkerGroupsFilter.WorkerGroupSelects.Where(x => x.Selected).Select(x => x.WorkerGroup.Id);

                    var workers = _dbContext.Set<DB.Worker>()
                        .Include(x => x.Group)
                        .Where(x => x.GroupId != null && groupIds.Contains((int)x.GroupId))
                        .Select(x => x.Id);

                    dataRequest = dataRequest
                        .Where(x => x.WorkerId != null && workers.Contains((int)x.WorkerId));
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
                    dataRequest = dataRequest.OrderByDescending(p => EF.Property<VM.Event>(p, sort.SortBy));
                }
                else
                {
                    dataRequest = dataRequest.OrderBy(p => EF.Property<VM.Event>(p, sort.SortBy));
                }
            }

            var count = await dataRequest.CountAsync();


            dataRequest = dataRequest.Skip(pageSize * pageNumber).Take(pageSize);

            var data = await dataRequest.ToListAsync();

            return new GridData<Event>()
            {
                Items = data,
                TotalItems = count,
            };
        }

        private async Task<int> GetCountAsync(IQueryable<Event> dataRequest)
        {
            while (DbContextState.IsDbContextBusy)
            {
                Thread.Sleep(1);
            }
            try
            {
                DbContextState.IsDbContextBusy = true;
                return await dataRequest.CountAsync();
            }
            finally
            {
                DbContextState.IsDbContextBusy = false;
            }
        }
        private async Task<List<Event>> GetEventListAsync(IQueryable<Event> dataRequest)
        {
            while (DbContextState.IsDbContextBusy)
            {
                Thread.Yield();
                Thread.Sleep(100);
            }
            try
            {
                DbContextState.IsDbContextBusy = true;
                return await dataRequest.ToListAsync();
            }
            finally
            {
                DbContextState.IsDbContextBusy = false;
            }
        }
        public AdditionalFiltersEvent GetAdditionalFiltersEvent()
        {
            return new AdditionalFiltersEvent()
            {
                ControllerLocationsFilter = new ControllerLocationsFilter()
                {
                    ControllerLocationSelect = _dbContext
                        .Set<DB.ControllerLocation>()
                        .AsNoTracking()
                        .Select(x => new ControllerLocationSelect()
                        {
                            Selected = true,
                            Location = new VM.Location()
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Description = x.Description,
                                ControllerId = (x.Controller != null) ? x.Controller.Id : 0,
                                ControllerIp = (x.Controller != null) ? x.Controller.IpAddress : string.Empty,
                                ControllerSn = (x.Controller != null) ? x.Controller.Sn : string.Empty,
                            }
                        })
                        .ToList()
                },
                WorkerGroupsFilter = new WorkerGroupsFilter()
                {
                    WorkerGroupSelects = _dbContext
                        .Set<DB.WorkerGroup>()
                        .AsNoTracking()
                        .Select(x => new WorkerGroupSelect()
                        {
                            Selected = true,
                            WorkerGroup = new VM.WorkerGroup()
                            {
                                Id = x.Id,
                                Name = x.Name,
                            }
                        })
                        .ToList()
                        }
            };
        }

        public async Task<string> GetReport(ICollection<SortDefinition<Event>> sortDefinitions, ICollection<IFilterDefinition<Event>>? filterDefinitions, AdditionalFiltersEvent? additionalFiltersEvent)
        {
            var dataRequest = _dbContext
                .Set<DB.Event>()
                .AsNoTracking()
                .Include(x => x.Worker)
                .Include(x => x.ControllerLocation)
                .Include(x => x.EventType)
                .Select(x => new VM.Event
                {
                    Id = x.Id,
                    Create = x.Create,
                    Flags = x.Flag,
                    WorkerId = x.WorkerId,
                    CardNumber16 = x.Card,
                    EventTypeName = (x.EventType != null) ? x.EventType.Name : string.Empty,
                    LocationName = (x.ControllerLocation != null) ? x.ControllerLocation.Name : string.Empty,
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

            if (additionalFiltersEvent != null)
            {
                if (additionalFiltersEvent.ControllerLocationsFilter.ControllerLocationSelect.FirstOrDefault(x => !x.Selected) != null)
                {
                    var locationNames = additionalFiltersEvent.ControllerLocationsFilter.ControllerLocationSelect.Where(x => x.Selected).Select(x => x.Location.Name);

                    dataRequest = dataRequest
                        .Where(x => locationNames.Contains(x.LocationName));
                }
                if (additionalFiltersEvent.WorkerGroupsFilter.WorkerGroupSelects.FirstOrDefault(x => !x.Selected) != null)
                {
                    var groupIds = additionalFiltersEvent.WorkerGroupsFilter.WorkerGroupSelects.Where(x => x.Selected).Select(x => x.WorkerGroup.Id);

                    var workers = _dbContext.Set<DB.Worker>()
                        .Include(x => x.Group)
                        .Where(x => x.GroupId != null && groupIds.Contains((int)x.GroupId))
                        .Select(x => x.Id);

                    dataRequest = dataRequest
                        .Where(x => x.WorkerId != null && workers.Contains((int)x.WorkerId));
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
                    dataRequest = dataRequest.OrderByDescending(p => EF.Property<VM.Event>(p, sort.SortBy));
                }
                else
                {
                    dataRequest = dataRequest.OrderBy(p => EF.Property<VM.Event>(p, sort.SortBy));
                }
            }

            var value = new
            {
                data = await dataRequest.ToArrayAsync()
            };

            var path = $"{System.IO.Directory.GetCurrentDirectory()}/wwwroot/reports/{Guid.NewGuid()}.xlsx";
            var templatePath = $"{System.IO.Directory.GetCurrentDirectory()}/wwwroot/template.xlsx";

            //var values = data.Select(x => new Dictionary<string, object>()
            //{
            //    {"Контроллер", x.LocationName}, 
            //    {"Событие", x.EventTypeName},
            //    {"Карта", x.CardNumber16 },
            //    {"Id сотрудника", x.WorkerId != null ? x.WorkerId.ToString() ?? string.Empty : string.Empty},
            //    {"ФИО сотрудника", x.FullName},
            //    {"Время события", x.Create.ToString("dd.MM.yyyy HH:mm:ss") },
            //    {"Флаги", x.Flags }
            //});

            try
            {
                await MiniExcel.SaveAsByTemplateAsync(path, templatePath, value);
            }
            catch (Exception ex)
            {

            }

            return path.ToString();
        }
    }
}
