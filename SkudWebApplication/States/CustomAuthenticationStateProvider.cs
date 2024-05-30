using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using SkudWebApplication.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SkudWebApplication.States
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonimous = new(new ClaimsPrincipal());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Constants.JWTToken))
                    return await Task.FromResult(new AuthenticationState(anonimous));
                var getUserClaims = DecryptToken(Constants.JWTToken);
                if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonimous));
                var claimsPrincipal = SetClaimPrincipal(getUserClaims);
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch { return await Task.FromResult(new AuthenticationState(anonimous)); }
        }

        public async void UpdateAuthenticationState(string jwtToken)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if (!string.IsNullOrEmpty(jwtToken))
            {
                Constants.JWTToken = jwtToken;
                //async localSorageService.SetToken(jwtToken);
                var getUserClaims = DecryptToken(jwtToken);
                claimsPrincipal = SetClaimPrincipal(getUserClaims);
            }
            else
            {
                Constants.JWTToken = null!;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claimsPrincipal)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new(ClaimTypes.Name, claimsPrincipal.Name),
                    
                }, "JwtAuth"));
        }

        private static CustomUserClaims DecryptToken(string jwtToken) 
        {
            if(string.IsNullOrEmpty(jwtToken)) return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var name = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            return new CustomUserClaims(name!.Value);
        }
    }
}
