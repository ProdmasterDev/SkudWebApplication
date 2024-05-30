using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.AccessGroup
{
    public class EditAccessGroupRequest : AccessGroupRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendEditRequestAsync(_apiMethod, this);
        }
        public async override Task ValidateAndThrow(WebAppContext dbContext)
        {
            EditAccessGroupValidator validator = new EditAccessGroupValidator(dbContext);
            await validator.ValidateAndThrowAsync(this);
        }
    }
    public class EditAccessGroupValidator : AbstractValidator<EditAccessGroupRequest>
    {
        public EditAccessGroupValidator(WebAppContext dbContext)
        {
            RuleFor(x=>x)
                .Must(p => dbContext.Set<ControllerDomain.Entities.AccessGroup>().AsNoTracking().FirstOrDefault(x => x.Name == p.Name && x.Id != p.Id) == null)
                    .WithMessage("Группа доступа с таким названием уже существует!");
            RuleFor(x => x.Name)
                    .NotEmpty()
                        .WithMessage("Название не заполнено!");
        }
    }
}
