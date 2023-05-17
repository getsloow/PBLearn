using Microsoft.EntityFrameworkCore;
using PBL.Data;
using PBL.Models;
using PBL.Repositories.Interfaces;

namespace PBL.Repositories
{
    public class CommentRepository : RepositoryBase<CommentModel>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext commentContext)
           : base(commentContext)
        {
        }
       
    }
}
