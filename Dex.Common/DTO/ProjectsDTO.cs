namespace Dex.Common.DTO
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public class ProjectsDTO
    {
        public Int32 Id
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }

        public String RepositoryLink
        {
            get;
            set;
        }

        public String ExecutableName
        {
            get;
            set;
        }

        public DateTime DateAdded
        {
            get;
            set;
        }

        public ICollection<ProjectFavoritesDTO> ProjectFavorites
        {
            get;
            set;
        }
    }
}