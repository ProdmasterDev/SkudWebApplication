using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.Services.Interfaces;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Requests.Worker
{
    public class EditWorkerRequest : WorkerRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendEditRequestAsync(_apiMethod, this);
        }

        public override async Task ValidateAndThrow(WebAppContext dbContext)
        {
            EditWorkerValidator validator = new EditWorkerValidator(dbContext);
            await validator.ValidateAndThrowAsync(this);
        }
    }
    public class EditWorkerValidator : AbstractValidator<EditWorkerRequest>
    {
        public EditWorkerValidator(WebAppContext dbContext)
        {
            RuleFor(x => x.LastName)
                .NotEmpty()
                    .WithMessage("Фамилия не заполнена!");
            RuleFor(x => x.FirstName)
                .NotEmpty()
                    .WithMessage("Имя не заполнено!");
            RuleFor(x => x.FatherName)
                .NotEmpty()
                    .WithMessage("Отчество не заполнено!");
            RuleFor(x => x.Cards)
                .Must(x => !x.GroupBy(y => y.CardNumb16).Where(y => y.Count() > 1).Any())
                    .WithMessage("Карты не должны повторяться!");
            RuleForEach(x => x.Cards)
                .MustAsync(async (x, token) =>
                    x.Id != default
                    || await dbContext.Set<DB.Card>()
                    .FirstOrDefaultAsync(y =>
                        y.CardNumb == x.CardNumb, token)
                    == null)
                .WithMessage("Карта уже существует в системе");
        }
    }
}
