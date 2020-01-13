namespace Dex.Common.DTO
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public class AspNetUsersDTO
    {
        public ICollection<AspNetUserClaimsDTO> AspNetUserClaims
        {
            get;
            set;
        }

        public ICollection<AspNetUserLoginsDTO> AspNetUserLogins
        {
            get;
            set;
        }

        public ICollection<AspNetUserRolesDTO> AspNetUserRoles
        {
            get;
            set;
        }

        public ICollection<AspNetUserTokensDTO> AspNetUserTokens
        {
            get;
            set;
        }

        public ICollection<ProjectFavoritesDTO> ProjectFavorites
        {
            get;
            set;
        }

        public ICollection<AspNetUsersSessionDTO> AspNetUsersSession
        {
            get;
            set;
        }

        public String Id
        {
            get;
            set;
        }

        public String UserName
        {
            get;
            set;
        }

        public String NormalizedUserName
        {
            get;
            set;
        }

        public String Email
        {
            get;
            set;
        }

        public String NormalizedEmail
        {
            get;
            set;
        }

        public Boolean EmailConfirmed
        {
            get;
            set;
        }

        public String PasswordHash
        {
            get;
            set;
        }

        public String SecurityStamp
        {
            get;
            set;
        }

        public String ConcurrencyStamp
        {
            get;
            set;
        }

        public String PhoneNumber
        {
            get;
            set;
        }

        public Boolean PhoneNumberConfirmed
        {
            get;
            set;
        }

        public Boolean TwoFactorEnabled
        {
            get;
            set;
        }

        public Nullable<DateTimeOffset> LockoutEnd
        {
            get;
            set;
        }

        public Boolean LockoutEnabled
        {
            get;
            set;
        }

        public Int32 AccessFailedCount
        {
            get;
            set;
        }
    }
}