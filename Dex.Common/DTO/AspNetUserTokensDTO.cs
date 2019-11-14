namespace Dex.Common.DTO
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public class AspNetUserTokensDTO
    {
        public String UserId
        {
            get;
            set;
        }

        public String LoginProvider
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public String Value
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