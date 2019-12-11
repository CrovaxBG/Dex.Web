using System;
using System.Collections.Generic;

namespace Dex.DataAccess.Models
{
    public partial class Projects
    {
        public Projects()
        {
            ProjectFavorites = new HashSet<ProjectFavorites>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RepositoryLink { get; set; }
        public string ExecutableName { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual ICollection<ProjectFavorites> ProjectFavorites { get; set; }
    }
}
