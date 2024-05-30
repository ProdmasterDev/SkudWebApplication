using MediatR;
using SkudWebApplication.Requests.Location;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.Controller
{
    public abstract class ControllerRequest : IRequest
    {
        public int? Id { get; set; }
        public string Sn { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string FwVer { get; set; } = string.Empty;
        public string ComFwVer { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public ControllerLocation? Location { get; set; }
        public DateTime LastPing { get; set; }
        public DateTime LastPowerOn { get; set; }
        protected string _apiMethod = $"{nameof(ControllerRequest).Replace("Request", string.Empty)}Api";
        public abstract Task SendToApiAsync(IApiProvider apiProvider);
    }

    public class ControllerLocation
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
