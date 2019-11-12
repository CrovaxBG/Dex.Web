namespace Dex.DataAccess.UnitOfWork
{
    using Dex.DataAccess.Models;
    using Dex.DataAccess.Repository;

    public partial class UnitOfWork : IUnitOfWork
    {
        private readonly Dex.DataAccess.Models.DexContext _dbContext;
        private Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetRoleClaims> _aspNetRoleClaims;
        private Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetRoles> _aspNetRoles;
        private Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUserClaims> _aspNetUserClaims;
        private Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUserLogins> _aspNetUserLogins;
        private Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUserRoles> _aspNetUserRoles;
        private Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUserTokens> _aspNetUserTokens;
        private Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUsers> _aspNetUsers;

        public Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetRoleClaims> AspNetRoleClaims
        {
            get { return _aspNetRoleClaims ??= new GenericRepository<AspNetRoleClaims>(_dbContext); }
        }

        public Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetRoles> AspNetRoles
        {
            get { return _aspNetRoles ??= new GenericRepository<AspNetRoles>(_dbContext); }
        }

        public Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUserClaims> AspNetUserClaims
        {
            get { return _aspNetUserClaims ??= new GenericRepository<AspNetUserClaims>(_dbContext); }
        }

        public Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUserLogins> AspNetUserLogins
        {
            get { return _aspNetUserLogins ??= new GenericRepository<AspNetUserLogins>(_dbContext); }
        }

        public Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUserRoles> AspNetUserRoles
        {
            get { return _aspNetUserRoles ??= new GenericRepository<AspNetUserRoles>(_dbContext); }
        }

        public Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUserTokens> AspNetUserTokens
        {
            get { return _aspNetUserTokens ??= new GenericRepository<AspNetUserTokens>(_dbContext); }
        }

        public Dex.DataAccess.Repository.IRepository<Dex.DataAccess.Models.AspNetUsers> AspNetUsers
        {
            get { return _aspNetUsers ??= new GenericRepository<AspNetUsers>(_dbContext); }
        }

        public UnitOfWork(Dex.DataAccess.Models.DexContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}