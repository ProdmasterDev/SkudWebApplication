using FluentValidation;
using MediatR;
using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;
using System.ComponentModel;

namespace SkudWebApplication.Requests.WorkerGroup
{
    public abstract class WorkerGroupRequest : IRequest
    {
        public int? Id { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; } = string.Empty;
        public IEnumerable<AccessRequest> Accesses { get; set; } = new List<AccessRequest>();
        protected string _apiMethod = $"{nameof(WorkerGroupRequest).Replace("Request",string.Empty)}Api";
        public abstract Task SendToApiAsync(IApiProvider apiProvider);
        public abstract Task ValidateAndThrow(WebAppContext dbContext);
    }

    public class WorkerGroupValidator : AbstractValidator<WorkerGroupRequest>
    {
        public WorkerGroupValidator()
        {
            RuleFor(x => x.Name)
                    .NotEmpty()
                        .WithMessage("Название не заполнено!");
        }
    }
}
