using MudBlazor;
using SkudWebApplication.Requests.Location;
using SkudWebApplication.ViewModels;

namespace SkudWebApplication.Services.Interfaces
{
    public interface ILocationService
    {
        Task<AddLocationRequest> GetAddRequest();
        Task<EditLocationRequest> GetEditRequest(Location viewModel);
        Task<GridData<Location>> GetGridData(ICollection<SortDefinition<Location>> sortDefinitions, ICollection<IFilterDefinition<Location>>? filterDefinitions, int pageNumber, int pageSize);
        Task<IEnumerable<LocationController>> GetAvailableControllers(int? currentLocationId);
    }
}
