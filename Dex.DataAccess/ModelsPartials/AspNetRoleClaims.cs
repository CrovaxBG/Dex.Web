using Microsoft.AspNetCore.Identity;

namespace Dex.DataAccess.Models
{
    public partial class AspNetRoleClaims : IdentityRoleClaim<string>
    {
        public virtual AspNetRoles Role { get; set; }
    }
}
