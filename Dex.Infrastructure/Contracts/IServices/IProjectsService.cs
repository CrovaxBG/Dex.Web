using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dex.Common.DTO;
using Dex.DataAccess.Models;

namespace Dex.Infrastructure.Contracts.IServices
{
    public interface IProjectsService
    {
        Task<int> AddProject(ProjectsDTO dto);
        Task<int> ModifyProject(ProjectsDTO dto);
        Task RemoveProject(int id);
        Task<ProjectsDTO> GetProject(int id);
        Task<List<ProjectsDTO>> GetProjects();
    }
}