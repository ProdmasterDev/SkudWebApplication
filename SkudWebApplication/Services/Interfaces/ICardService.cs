using MudBlazor;
using SkudWebApplication.Requests.Card;
using SkudWebApplication.Requests.Location;
using Card = SkudWebApplication.ViewModels.Card;

namespace SkudWebApplication.Services.Interfaces
{
    public interface ICardService
    {
        Task<AddCardRequest> GetAddRequest();
        Task<EditCardRequest> GetEditRequest(Card viewModel);
        Task<GridData<Card>> GetGridData(ICollection<SortDefinition<Card>> sortDefinitions, ICollection<IFilterDefinition<Card>>? filterDefinitions, int pageNumber, int pageSize);
        Task<IEnumerable<CardWorker>> GetAvailableWorkers(string? searchString);
    }
}