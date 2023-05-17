using PBL.Models;

namespace PBL.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectModel?> DetailsAsync(int? id);
        Task DeleteAsync(int id);
        Task GradeAsync(int projectId, float ProjectGrade);
        Task CreateAsync(ProjectModel project);
        Task<List<ProjectModel>> GetAllProjects();
        Task<List<ProjectModel>> GetAllProjectsByEmail(string userEmail);
    }
}