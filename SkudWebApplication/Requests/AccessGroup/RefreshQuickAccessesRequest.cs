using SkudWebApplication.Db;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.AccessGroup
{
    public class RefreshQuickAccessesRequest : AccessGroupRequest
    {
        public override Task SendToApiAsync(IApiProvider apiProvider)
        {
            throw new NotImplementedException();
        }

        public override Task ValidateAndThrow(WebAppContext dbContext)
        {
            throw new NotImplementedException();
        }
    }
}
