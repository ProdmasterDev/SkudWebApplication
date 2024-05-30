using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.Requests.User;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.User
{
    public class EditUserRequest : UserRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendEditRequestAsync(_apiMethod, this);
        }

        public override async Task ValidateAndThrow(WebAppContext dbContext)
        {
            EditUserValidator validator = new EditUserValidator(dbContext);
            await validator.ValidateAndThrowAsync(this);
        }
    }

    public class EditUserValidator : AbstractValidator<EditUserRequest>
    {
        public EditUserValidator(WebAppContext dbContext)
        {
            RuleFor(x => x)
                   .Must(p => dbContext.Set<ControllerDomain.Entities.User>().AsNoTracking().FirstOrDefault(x => x.Login == p.Login && x.Id != p.Id) == null)
                       .WithMessage("Пользователь с таким логином уже существует!");
            RuleFor(x => x.Login)
                    .NotEmpty()
                        .WithMessage("Логин не заполнен!");
            RuleFor(x => x.Password)
                    .NotEmpty()
                        .WithMessage("Пароль не заполнен!")
                    .Length(6,24)
                        .WithMessage("Длина пароля должна составлять от 6 до 24 символов!");
        }
    }
}
