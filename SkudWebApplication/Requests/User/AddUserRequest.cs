using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.AccessGroup;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.User
{
    public class AddUserRequest : UserRequest
    {
        public override async Task SendToApiAsync(IApiProvider apiProvider)
        {
            await apiProvider.SendAddRequestAsync(_apiMethod, this);
        }

        public override async Task ValidateAndThrow(WebAppContext dbContext)
        {
            AddUserValidator validator = new AddUserValidator(dbContext);
            await validator.ValidateAndThrowAsync(this);
        }
    }
    public class AddUserValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserValidator(WebAppContext dbContext)
        {
            RuleFor(x => x.Login)
                    .NotEmpty()
                        .WithMessage("Логин не заполнен!")
                    .Must(p => dbContext.Set<ControllerDomain.Entities.User>().AsNoTracking().FirstOrDefault(x => x.Login == p) == null)
                        .WithMessage("Пользователь с таким логином уже существует!");
            RuleFor(x => x.Password)
                    .NotEmpty()
                        .WithMessage("Пароль не заполнен!")
                    .Length(6, 24)
                        .WithMessage("Длина пароля должна составлять от 6 до 24 символов!");
        }
    }
}
