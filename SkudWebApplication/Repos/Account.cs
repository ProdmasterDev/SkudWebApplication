using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkudWebApplication.Db;
using SkudWebApplication.Dto;
using SkudWebApplication.Requests.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Repos
{
    public class Account : IAccount
    {
        private readonly WebAppContext _dbContext;
        private readonly IConfiguration _configuration;

        public Account(IConfiguration configuration, WebAppContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if(await _dbContext
                .Set<DB.User>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => 
                    x.Login == request.Login 
                    && x.Password == request.Password 
                    && !x.Arch) != null)
            {
                return new LoginResponse() { Flag = true, Message = "Success", JWTToken = GenerateToken(request.Login) };
            }
            return new LoginResponse() { Flag = false, Message = "Login/Password not valid" };
        }

        private string GenerateToken(string login)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new List<Claim>
                {
                    new(ClaimTypes.Name, login)
                };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"]!,
                audience: _configuration["Jwt:Audience"]!,
                claims: userClaims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
