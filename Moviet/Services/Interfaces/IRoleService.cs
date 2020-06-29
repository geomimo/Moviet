using System.Security.Claims;

namespace Moviet.Services.Interfaces
{
    public interface IRoleService
    {
        public void UpgradeToContentManager(ClaimsPrincipal principal);
        public void DowngradeToRater(ClaimsPrincipal principal);

    }
}
