using Microsoft.AspNetCore.Identity;

namespace LevRobinGuss.Core.Entities.Identity
{
    public class ApplicationUserLogin : IdentityUserLogin<int>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
