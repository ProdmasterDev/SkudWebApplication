using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;
using VM = SkudWebApplication.ViewModels;
using DB = ControllerDomain.Entities;
using SkudWebApplication.ViewModels;
using MudBlazor;
using SkudWebApplication.Requests.User;

namespace SkudWebApplication.Services.Classes
{
    public class UserService : IUserService
    {
        private readonly WebAppContext _dbContext;
        private readonly IMapper _mapper;
        public UserService(WebAppContext dbContext, IMapper mapper) { _dbContext = dbContext; _mapper = mapper; }

        public async Task<AddUserRequest> GetAddRequest()
        {
            return await Task.FromResult(new AddUserRequest());
        }

        public async Task<EditUserRequest> GetEditRequest(User viewModel)
        {
            var entity = await _dbContext.Set<DB.User>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Пользователь не найден!");
            }
            return _mapper.Map<EditUserRequest>(entity);
        }

        public async Task<GridData<User>> GetGridData(ICollection<MudBlazor.SortDefinition<User>> sortDefinitions, ICollection<MudBlazor.IFilterDefinition<User>>? filterDefinitions, int pageNumber, int pageSize)
        {
            var dataRequest = _dbContext.Set<DB.User>()
                .Where(x => !x.Arch)
                .AsNoTracking();

            if (sortDefinitions.Count == 0)
            {
                dataRequest = dataRequest.OrderBy(p => p.Id);
            }
            foreach (var sort in sortDefinitions)
            {
                if (sort.Descending)
                {
                    dataRequest = dataRequest.OrderByDescending(p => EF.Property<DB.User>(p, sort.SortBy));
                }
                else
                {
                    dataRequest = dataRequest.OrderBy(p => EF.Property<DB.User>(p, sort.SortBy));
                }
            }

            var count = dataRequest.Count();

            dataRequest = dataRequest.Skip(pageSize * pageNumber).Take(pageSize);

            var data = _mapper.Map<IEnumerable<DB.User>, IEnumerable<VM.User>>(await dataRequest.ToListAsync());

            return new GridData<User>()
            {
                Items = data,
                TotalItems = count,
            };
        }
    }
}
