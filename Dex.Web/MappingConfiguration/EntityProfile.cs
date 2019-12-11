using AutoMapper;
using Dex.Common.DTO;
using Dex.DataAccess.Models;

namespace Dex.Web.MappingConfiguration
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            EntityDtoMaps();
        }

        private void EntityDtoMaps()
        {
            CreateMap<Log, LogDTO>()
                .ReverseMap();            
            
            CreateMap<Projects, ProjectsDTO>()
                .ReverseMap();  
            
            CreateMap<ProjectFavorites, ProjectFavoritesDTO>()
                .ReverseMap();      
            
            CreateMap<AspNetUsers, AspNetUsersDTO>()
                .ReverseMap(); 
            
            CreateMap<AspNetUserClaims, AspNetUserClaimsDTO>()
                .ReverseMap(); 
            
            CreateMap<AspNetUserLogins, AspNetUserLoginsDTO>()
                .ReverseMap();     
            
            CreateMap<AspNetUserRoles, AspNetUserRolesDTO>()
                .ReverseMap(); 
            
            CreateMap<AspNetUserTokens, AspNetUserTokensDTO>()
                .ReverseMap(); 
            
            CreateMap<AspNetRoleClaims, AspNetRoleClaimsDTO>()
                .ReverseMap();   
            
            CreateMap<AspNetRoles, AspNetRolesDTO>()
                .ReverseMap();
        }
    }
}
