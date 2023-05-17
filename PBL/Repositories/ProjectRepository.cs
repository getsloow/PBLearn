using PBL.Data;
using PBL.Models;
using PBL.Repositories.Interfaces;

namespace PBL.Repositories;
public class ProjectRepository : RepositoryBase<ProjectModel>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext commentContext)
       : base(commentContext)
    {
    }
   
}
