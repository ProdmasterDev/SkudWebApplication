using MudBlazor;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.ViewModels;

namespace SkudWebApplication.Services.Interfaces
{
    public interface IAccessGroupService
    {
        Task<AddAccessGroupRequest> GetAddRequest();
        Task<EditAccessGroupRequest> GetEditRequest(AccessGroup viewModel);
        Task<GridData<AccessGroup>> GetGridData(ICollection<SortDefinition<AccessGroup>> sortDefinitions, ICollection<IFilterDefinition<AccessGroup>>? filterDefinitions, int pageNumber, int pageSize);
    }
}
