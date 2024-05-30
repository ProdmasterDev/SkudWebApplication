using FluentValidation;
using Newtonsoft.Json;
using RestEase;
using SkudWebApplication.ApiInterfaces;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Net;
using SkudWebApplication.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.AccessGroup;

namespace SkudWebApplication.Requests.WorkerGroup
{
    public class EditWorkerGroupRequest : WorkerGroupRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendEditRequestAsync(_apiMethod, this);    
        }

        public override async Task ValidateAndThrow(WebAppContext dbContext)
        {
            EditWorkerGroupValidator validator = new EditWorkerGroupValidator(dbContext);
            await validator.ValidateAndThrowAsync(this);
        }
    }
    public class EditWorkerGroupValidator : AbstractValidator<EditWorkerGroupRequest>
    {
        public EditWorkerGroupValidator(WebAppContext dbContext)
        {
            RuleFor(x => x)
                .Must(p => dbContext.Set<ControllerDomain.Entities.WorkerGroup>().AsNoTracking().FirstOrDefault(x => x.Name == p.Name && x.Id != p.Id) == null)
                    .WithMessage("Подразделение с таким названием уже существует!");
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Название не заполнено!");
        }
    }
}
