using PBL.Data;
using PBL.Models;
using PBL.Repositories.Interfaces;

namespace PBL.Repositories;
public class AssignmentRepository : RepositoryBase<AssignmentModel>, IAssignmentRepository
{
    public AssignmentRepository(ApplicationDbContext assignmentContext)
       : base(assignmentContext)
    {
    }
   
}
