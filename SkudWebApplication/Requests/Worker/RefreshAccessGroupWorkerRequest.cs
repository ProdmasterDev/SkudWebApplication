using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.Worker
{
    public class RefreshAccessGroupWorkerRequest : WorkerRequest
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
