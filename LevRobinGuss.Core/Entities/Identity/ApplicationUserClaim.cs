﻿using Microsoft.AspNetCore.Identity;

namespace LevRobinGuss.Core.Entities.Identity
{
    public class ApplicationUserClaim : IdentityUserClaim<int>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
