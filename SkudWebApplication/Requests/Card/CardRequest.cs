using MediatR;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.User;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.Card
{
    public abstract class CardRequest : IRequest
    {
        public int? Id { get; set; }
        public string CardNumb16 { get; set; } = string.Empty;
        public string CardNumb { get; set; } = string.Empty;
        public virtual CardWorker? Worker { get; set; }
        protected string _apiMethod = $"{nameof(CardRequest).Replace("Request", string.Empty)}Api";
        public abstract Task SendToApiAsync(IApiProvider apiProvider);
        public abstract Task ValidateAndThrow(WebAppContext dbContext);
    }
    public class CardWorker
    {
        public int Id {  get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FatherName {  get; set; } = string.Empty;
        public string Position {  get; set; } = string.Empty;
    }
}
