using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Dex.DataAccess.Models
{
    public partial class AspNetUserRoles : IdentityUserRole<string>
    {
        public virtual AspNetRoles Role { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
