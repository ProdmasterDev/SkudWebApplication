using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.Requests.Card;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.Card
{
    public class EditCardRequest : CardRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendEditRequestAsync(_apiMethod, this);
        }

        public override async Task ValidateAndThrow(WebAppContext dbContext)
        {
            EditCardValidator validator = new EditCardValidator(dbContext);
            await validator.ValidateAndThrowAsync(this);
        }
    }

    public class EditCardValidator : AbstractValidator<EditCardRequest>
    {
        public EditCardValidator(WebAppContext dbContext)
        {
            RuleFor(x => x)
                .Must(p => dbContext.Set<ControllerDomain.Entities.Card>().AsNoTracking().FirstOrDefault(x => x.CardNumb16 == p.CardNumb16 && x.Id != p.Id) == null)
                    .WithMessage("Такая карта уже существует в системе!");
            RuleFor(x => x.CardNumb16)
                    .NotEmpty()
                        .WithMessage("Карта не заполнена!");
        }
    }
}
