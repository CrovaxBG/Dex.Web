namespace Dex.DataAccess.UnitOfWork
{
    using Dex.DataAccess.Models;
    using Dex.DataAccess.Repository;

    public interface IUnitOfWork
    {
        Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetRoleClaims> AspNetRoleClaims
        {
            get;
        }

        Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetRoles> AspNetRoles
        {
            get;
        }

        Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUserClaims> AspNetUserClaims
        {
            get;
        }

        Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUserLogins> AspNetUserLogins
        {
            get;
        }

        Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUserRoles> AspNetUserRoles
        {
            get;
        }

        Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUserTokens> AspNetUserTokens
        {
            get;
        }

        Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUsers> AspNetUsers
        {
            get;
        }

        void SaveChanges();
    }
}