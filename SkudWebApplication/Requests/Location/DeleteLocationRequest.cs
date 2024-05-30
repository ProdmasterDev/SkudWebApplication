using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.Location
{
    public class DeleteLocationRequest : LocationRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendDeleteRequestAsync(_apiMethod, this);
        }

        public override Task ValidateAndThrow(WebAppContext dbContext)
        {
            throw new NotImplementedException();
        }
    }
}
