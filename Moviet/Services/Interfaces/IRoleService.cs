using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Moviet.Services.Interfaces
{
    public interface IRoleService
    {
        public void UpgradeToContentManager(ClaimsPrincipal principal);
        public void DowngradeToRater(ClaimsPrincipal principal);

    }
}
