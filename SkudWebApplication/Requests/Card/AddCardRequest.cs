using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.Card
{
    public class AddCardRequest : CardRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendAddRequestAsync(_apiMethod, this);
        }

        public override async Task ValidateAndThrow(WebAppContext dbContext)
        {
            AddCardValidator validator = new AddCardValidator(dbContext);
            await validator.ValidateAndThrowAsync(this);
        }
    }

    public class AddCardValidator : AbstractValidator<AddCardRequest>
    {
        public AddCardValidator(WebAppContext dbContext)
        {
            RuleFor(x => x.CardNumb16)
                .NotEmpty()
                    .WithMessage("Карта не заполнена!")
                .Must(p => dbContext.Set<ControllerDomain.Entities.Card>().AsNoTracking().FirstOrDefault(x => x.CardNumb16 == p) == null)
                    .WithMessage("Такая карта уже существует в системе!");
        }
    }
}
