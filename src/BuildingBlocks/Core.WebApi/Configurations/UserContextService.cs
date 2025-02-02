using Microsoft.AspNetCore.Http;

namespace Core.WebApi.Configurations
{
    public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
    {
        public string UserId => httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value ?? string.Empty;
    }
}
