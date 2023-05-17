using PBL.Data;
using PBL.Models;
using PBL.Repositories.Interfaces;

namespace PBL.Repositories;
public class TextAssignmentRepository : RepositoryBase<TextAssignmentModel>, ITextAssignmentRepository
{
    public TextAssignmentRepository(ApplicationDbContext assignmentContext)
       : base(assignmentContext)
    {
    }
   
}
