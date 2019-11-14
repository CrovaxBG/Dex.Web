namespace Dex.Common.DTO
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public class AspNetRoleClaimsDTO
    {
        public Int32 Id
        {
            get;
            set;
        }

        public String RoleId
        {
            get;
            set;
        }

        public String ClaimType
        {
            get;
            set;
        }

        public String ClaimValue
        {
            get;
            set;
        }

        public AspNetRolesDTO Role
        {
            get;
            set;
        }
    }
}