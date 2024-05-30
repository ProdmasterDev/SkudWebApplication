using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.Location
{
    public class AddLocationRequest : LocationRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendAddRequestAsync(_apiMethod, this);
        }

        public override async Task ValidateAndThrow(WebAppContext dbContext)
        {
            AddLocationValidator validator = new AddLocationValidator(dbContext);
            await validator.ValidateAndThrowAsync(this);
        }
    }
    public class AddLocationValidator : AbstractValidator<AddLocationRequest>
    {
        public AddLocationValidator(WebAppContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Название не заполнено!")
                .Must(p => dbContext.Set<ControllerDomain.Entities.ControllerLocation>().AsNoTracking().FirstOrDefault(x => x.Name == p) == null)
                    .WithMessage("Место прохода с таким названием уже существует!");
        }
    }
}
