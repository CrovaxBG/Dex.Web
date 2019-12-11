namespace Dex.Common.DTO
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public class ProjectFavoritesDTO
    {
        public String UserId
        {
            get;
            set;
        }

        public Int32 ProjectId
        {
            get;
            set;
        }
    }
}