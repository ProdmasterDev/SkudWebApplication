using SkudWebApplication.Dto;
using SkudWebApplication.Requests.Auth;

namespace SkudWebApplication.Repos
{
    public interface IAccount
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
