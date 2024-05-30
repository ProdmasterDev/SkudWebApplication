using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.Location
{
    public class EditLocationRequest : LocationRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendEditRequestAsync(_apiMethod, this);
        }

        public override async Task ValidateAndThrow(WebAppContext dbContext)
        {
            EditLocationValidator validator = new EditLocationValidator(dbContext);
            await validator.ValidateAndThrowAsync(this);
        }
    }
    public class EditLocationValidator : AbstractValidator<EditLocationRequest>
    {
        public EditLocationValidator(WebAppContext dbContext)
        {
            RuleFor(x => x)
                    .Must(p => dbContext.Set<ControllerDomain.Entities.ControllerLocation>().AsNoTracking().FirstOrDefault(x => x.Name == p.Name && x.Id != p.Id) == null)
                       .WithMessage("Место прохода с таким названием уже существует!");
            RuleFor(x => x.Name)
                    .NotEmpty()
                        .WithMessage("Название не заполнено!");
        }
    }
}
