using System;
using System.Collections.Generic;
using System.Text;

namespace LevRobinGuss.Core.Entities
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}
