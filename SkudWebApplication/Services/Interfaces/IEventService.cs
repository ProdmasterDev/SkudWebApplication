using BlazorBootstrap;
using MudBlazor;
using SkudWebApplication.ViewModels;
using SortDirection = BlazorBootstrap.SortDirection;

namespace SkudWebApplication.Services.Interfaces
{
    public interface IEventService
    {
        Task<string> GetReport(ICollection<SortDefinition<Event>> sortDefinitions, ICollection<IFilterDefinition<Event>>? filterDefinitions, AdditionalFiltersEvent? additionalFiltersEvent);
        Task<GridData<Event>> GetGridData(ICollection<SortDefinition<Event>> sortDefinitions, ICollection<IFilterDefinition<Event>>? filterDefinitions, int pageNumber, int pageSize, AdditionalFiltersEvent? additionalFiltersEvent);
        AdditionalFiltersEvent GetAdditionalFiltersEvent();
    }
}
