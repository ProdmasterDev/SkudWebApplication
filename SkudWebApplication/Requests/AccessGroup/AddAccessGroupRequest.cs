using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Location;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.AccessGroup
{
    public class AddAccessGroupRequest : AccessGroupRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendAddRequestAsync(_apiMethod, this);
        }
        public async override Task ValidateAndThrow(WebAppContext dbContext)
        {
            AddAccessGroupValidator validator = new AddAccessGroupValidator(dbContext);
            await validator.ValidateAndThrowAsync(this);
        }
    }

    public class AddAccessGroupValidator : AbstractValidator<AddAccessGroupRequest>
    {
        public AddAccessGroupValidator(WebAppContext dbContext)
        {
            RuleFor(x => x.Name)
                    .NotEmpty()
                        .WithMessage("Название не заполнено!")
                    .Must(p => dbContext.Set<ControllerDomain.Entities.AccessGroup>().AsNoTracking().FirstOrDefault(x => x.Name == p) == null)
                        .WithMessage("Группа доступа с таким названием уже существует!");
        }
    }
}
