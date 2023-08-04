using Microsoft.AspNetCore.Identity;

namespace LevRobinGuss.Core.Entities.Identity
{
    public class ApplicationUserToken : IdentityUserToken<int>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
