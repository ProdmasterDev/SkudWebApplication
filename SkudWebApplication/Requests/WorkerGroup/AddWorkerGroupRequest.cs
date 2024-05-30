
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestEase;
using SkudWebApplication.ApiInterfaces;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.WorkerGroup
{
    public class AddWorkerGroupRequest : WorkerGroupRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendAddRequestAsync(_apiMethod, this);
        }
        public override async Task ValidateAndThrow(WebAppContext dbContext)
        {
            AddWorkerGroupValidator validator = new AddWorkerGroupValidator(dbContext);
            await validator.ValidateAndThrowAsync(this);
        }
    }
    public class AddWorkerGroupValidator : AbstractValidator<AddWorkerGroupRequest>
    {
        public AddWorkerGroupValidator(WebAppContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Название не заполнено!")
                .Must(p => dbContext.Set<ControllerDomain.Entities.WorkerGroup>().AsNoTracking().FirstOrDefault(x => x.Name == p) == null)
                        .WithMessage("Подразделение с таким названием уже существует!");
        }
    }
}
