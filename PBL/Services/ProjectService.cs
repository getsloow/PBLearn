using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL.Models;
using PBL.Repositories.Interfaces;
using PBL.Services.Interfaces;

namespace PBL.Services;

public class ProjectService : IProjectService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public ProjectService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<List<ProjectModel>> GetAllProjectsByEmail(string userEmail)
    {
        return await _repositoryWrapper.ProjectRepository.FindByCondition(p => p.UserEmail == userEmail).ToListAsync();
    }
    public async Task<List<ProjectModel>> GetAllProjects()
    {
        return await _repositoryWrapper.ProjectRepository.FindAll().ToListAsync();
    }
    public async Task<ProjectModel?> DetailsAsync(int? id)
    {
        return await _repositoryWrapper.ProjectRepository.FindByCondition(p => p.Id == id)
                           .Include(p => p.Assignments)
                           .Include(a => a.Comments)
                           .Include(p => p.Files)
                           .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task DeleteAsync(int id)
    {
        var project = await _repositoryWrapper.ProjectRepository.FindByCondition(p => p.Id == id).FirstOrDefaultAsync();

        _repositoryWrapper.ProjectRepository.Delete(project!);
        _repositoryWrapper.Save();
    }
    public async Task GradeAsync(int projectId, float ProjectGrade)
    {
        var project =  await _repositoryWrapper.ProjectRepository.FindByCondition(p => p.Id == projectId).FirstOrDefaultAsync();
        project!.Grade = ProjectGrade;
        _repositoryWrapper.ProjectRepository.Update(project);
        _repositoryWrapper.Save();
    }

    public async Task CreateAsync(ProjectModel project)
    {
        project.Grade = 0;
        _repositoryWrapper.ProjectRepository.Create(project);
        _repositoryWrapper.Save();
    }
}
