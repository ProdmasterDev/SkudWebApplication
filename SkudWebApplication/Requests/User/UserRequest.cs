using MediatR;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Worker;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.User
{
    public abstract class UserRequest : IRequest
    {
        public int? Id {  get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set;} = string.Empty;
        protected string _apiMethod = $"{nameof(UserRequest).Replace("Request", string.Empty)}Api";
        public abstract Task SendToApiAsync(IApiProvider apiProvider);
        public abstract Task ValidateAndThrow(WebAppContext dbContext);
    }
}
