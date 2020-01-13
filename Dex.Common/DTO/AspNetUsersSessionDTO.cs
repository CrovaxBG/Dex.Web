namespace Dex.Common.DTO
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public class AspNetUsersSessionDTO
    {
        public Int32 Id
        {
            get;
            set;
        }

        public String UserId
        {
            get;
            set;
        }

        public DateTime LoginDate
        {
            get;
            set;
        }

        public Nullable<DateTime> LogoutDate
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