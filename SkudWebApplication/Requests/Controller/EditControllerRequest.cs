using FluentValidation;
using SkudWebApplication.Requests.Controller;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.Controller
{
    public class EditControllerRequest : ControllerRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendEditRequestAsync(_apiMethod, this);
        }
    }
}
