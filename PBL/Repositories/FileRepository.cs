using PBL.Data;
using PBL.Models;
using PBL.Repositories.Interfaces;

namespace PBL.Repositories
{
    public class FileRepository : RepositoryBase<FileModel>, IFileRepository
    {
        public FileRepository(ApplicationDbContext assignmentContext)
           : base(assignmentContext)
        {
        }
    }
}
