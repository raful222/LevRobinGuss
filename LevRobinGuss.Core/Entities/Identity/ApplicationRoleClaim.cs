using Microsoft.AspNetCore.Identity;

namespace LevRobinGuss.Core.Entities.Identity
{
    public class ApplicationRoleClaim : IdentityRoleClaim<int>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}
