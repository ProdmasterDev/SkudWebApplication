using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Dto;
using SkudWebApplication.Requests.Card;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.Auth
{
    public class LoginRequest : IRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        protected string _apiMethod = $"{nameof(LoginRequest).Replace("Request", string.Empty)}Api";
        public async Task ValidateAndThrow(WebAppContext dbContext)
        {
            LoginValidator validator = new LoginValidator(dbContext);
            await validator.ValidateAndThrowAsync(this);
        }
        public async Task<LoginResponse> SendToApiAsync(IApiProvider apiProvider)
        {
            return await apiProvider.SendGetRequestAsync<LoginRequest,LoginResponse>(_apiMethod, this);
        }
    }

    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator(WebAppContext dbContext)
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("Логин должен быть не пустым!");
            RuleFor(x => x.Password)
               .NotEmpty()
               .WithMessage("Пароль должен быть не пустым!");
            RuleFor(x => x)
                .Must(p => dbContext.Set<ControllerDomain.Entities.User>().AsNoTracking().FirstOrDefault(x => x.Login == p.Login && x.Password == p.Password) != null)
                    .WithMessage("Неверный логин или пароль!");
        }
    }
}
