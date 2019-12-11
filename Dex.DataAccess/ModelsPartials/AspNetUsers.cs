using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Dex.DataAccess.Models
{
    public partial class AspNetUsers : IdentityUser
    {
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual ICollection<ProjectFavorites> ProjectFavorites { get; set; }
    }
}
