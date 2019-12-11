using System;
using System.Collections.Generic;

namespace Dex.DataAccess.Models
{
    public partial class ProjectFavorites
    {
        public string UserId { get; set; }
        public int ProjectId { get; set; }

        public virtual Projects Project { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
