using MudBlazor;
using SkudWebApplication.Requests.WorkerGroup;
using SkudWebApplication.ViewModels;

namespace SkudWebApplication.Services.Interfaces
{
    public interface IWorkerGroupService
    {
        Task<AddWorkerGroupRequest> GetAddRequest();
        Task<EditWorkerGroupRequest> GetEditRequest(WorkerGroup viewModel);
        Task<GridData<WorkerGroup>> GetGridData(ICollection<SortDefinition<WorkerGroup>> sortDefinitions, ICollection<IFilterDefinition<WorkerGroup>>? filterDefinitions, int pageNumber, int pageSize);
    }
}
