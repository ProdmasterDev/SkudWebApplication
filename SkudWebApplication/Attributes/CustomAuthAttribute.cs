using Microsoft.AspNetCore.Mvc.Filters;

namespace SkudWebApplication.Attributes
{
    public sealed class CustomAuthAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context != null)
            {
                // Auth logic
            }
        }
    }
}
