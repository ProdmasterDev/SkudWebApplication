using MudBlazor;
using SkudWebApplication.ViewModels;
using SkudWebApplication.Requests.User;

namespace SkudWebApplication.Services.Interfaces
{
    public interface IUserService
    {
        Task<AddUserRequest> GetAddRequest();
        Task<EditUserRequest> GetEditRequest(User viewModel);
        Task<GridData<User>> GetGridData(ICollection<SortDefinition<User>> sortDefinitions, ICollection<IFilterDefinition<User>>? filterDefinitions, int pageNumber, int pageSize);
    }
}
