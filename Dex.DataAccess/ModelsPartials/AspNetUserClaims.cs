using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Dex.DataAccess.Models
{
    public partial class AspNetUserClaims : IdentityUserClaim<string>
    {
        public virtual AspNetUsers User { get; set; }
    }
}
