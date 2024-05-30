using MediatR;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Card;
using SkudWebApplication.Services.Interfaces;
using System.ComponentModel;

namespace SkudWebApplication.Requests.AccessGroup
{
    public abstract class AccessGroupRequest : IRequest
    {
        public int? Id { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; } = string.Empty;
        public IEnumerable<AccessRequest> Accesses { get; set; } = new List<AccessRequest>();
        protected string _apiMethod = $"{nameof(AccessGroupRequest).Replace("Request", string.Empty)}Api";
        public abstract Task SendToApiAsync(IApiProvider apiProvider);
        public abstract Task ValidateAndThrow(WebAppContext dbContext);
    }
}
