namespace Dex.Common.DTO
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public class AspNetUserRolesDTO
    {
        public String UserId
        {
            get;
            set;
        }

        public String RoleId
        {
            get;
            set;
        }

        public AspNetRolesDTO Role
        {
            get;
            set;
        }

        public AspNetUsersDTO User
        {
            get;
            set;
        }
    }
}