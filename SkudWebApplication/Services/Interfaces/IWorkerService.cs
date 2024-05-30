using MudBlazor;
using SkudWebApplication.Requests.Worker;
using SkudWebApplication.ViewModels;

namespace SkudWebApplication.Services.Interfaces
{
    public interface IWorkerService
    {
        Task<IEnumerable<Worker>> GetWorkersAsync();
        Task<AddWorkerRequest> GetAddRequest();
        Task<EditWorkerRequest> GetEditRequest(Worker viewModel);
        Task<IEnumerable<AccessMethod>> GetAccessMethodsAsync();
        Task<IEnumerable<WorkerAccessMethod>> GetAvailableAccessMethods();
        Task<IEnumerable<Requests.Worker.WorkerGroup>> GetAvailableGroups(string searchString);
        Task<GridData<Worker>> GetGridData(ICollection<SortDefinition<Worker>> sortDefinitions, ICollection<IFilterDefinition<Worker>>? filterDefinitions, int pageNumber, int pageSize);
    }
}
