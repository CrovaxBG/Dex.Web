using AutoMapper;
using Dex.Common.DTO;
using Dex.Web.ViewModels.Downloads;

namespace Dex.Web.MappingConfiguration
{
    public class ViewModelProfile : Profile
    {
        public ViewModelProfile()
        {
            DtoViewModelMaps();
        }

        private void DtoViewModelMaps()
        {
            CreateMap<ProjectsDTO, ProjectViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProjectDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ProjectExecutableName, opt => opt.MapFrom(src => src.ExecutableName))
                .ForMember(dest => dest.ProjectRepositoryLink, opt => opt.MapFrom(src => src.RepositoryLink))
                .ForMember(dest => dest.ProjectDate, opt => opt.MapFrom(src => src.DateAdded))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProjectName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ProjectDescription))
                .ForMember(dest => dest.ExecutableName, opt => opt.MapFrom(src => src.ProjectExecutableName))
                .ForMember(dest => dest.RepositoryLink, opt => opt.MapFrom(src => src.ProjectRepositoryLink))
                .ForMember(dest => dest.DateAdded, opt => opt.MapFrom(src => src.ProjectDate));
        }
    }
}