using BlazorBootstrap;
using MudBlazor;
using SkudWebApplication.Requests.Controller;
using SkudWebApplication.Requests.Location;
using SkudWebApplication.ViewModels;
using SortDirection = BlazorBootstrap.SortDirection;

namespace SkudWebApplication.Services.Interfaces
{
    public interface IControllerService
    {
        Task<EditControllerRequest> GetEditRequest(Controller viewModel);
        Task<GridData<Controller>> GetGridData(ICollection<SortDefinition<Controller>> sortDefinitions, ICollection<IFilterDefinition<Controller>>? filterDefinitions, int pageNumber, int pageSize);
        Task<IEnumerable<ControllerLocation>> GetAvailableLocations(int? currentControllerId);
    }
}