using FluentValidation;
using MediatR;
using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;
using System.ComponentModel;

namespace SkudWebApplication.Requests.Location
{
    public abstract class LocationRequest : IRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public LocationController? Controller { get; set; }
        protected string _apiMethod = $"{nameof(LocationRequest).Replace("Request", string.Empty)}Api";
        public abstract Task SendToApiAsync(IApiProvider apiProvider);
        public abstract Task ValidateAndThrow(WebAppContext dbContext);
    }
    public class LocationController
    {
        public int? Id {  set; get; }
        public string Sn {  set; get; } = string.Empty;
        public string Ip {  set; get; } = string.Empty;
    }
    public class LocationValidator : AbstractValidator<LocationRequest>
    {
        public LocationValidator()
        {
            RuleFor(x => x.Name)
                    .NotEmpty()
                        .WithMessage("Название не заполнено!");
        }
    }
}
