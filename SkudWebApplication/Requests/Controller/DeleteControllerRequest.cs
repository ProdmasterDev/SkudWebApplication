using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.Controller
{
    public class DeleteControllerRequest : ControllerRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendDeleteRequestAsync(_apiMethod, this);
        }
    }
}
