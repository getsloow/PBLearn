using PBL.Data;
using PBL.Repositories.Interfaces;

namespace PBL.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private ICommentRepository _commentRepository;
        private IProjectRepository _projectRepository;
        private IAssignmentRepository _assignmentRepository;
        private IFileRepository _fileRepository;

        public IFileRepository FileRepository
        {
            get
            {
                if (_fileRepository == null)
                {
                    _fileRepository = new FileRepository(_dbContext);
                }

                return _fileRepository;
            }
        }
        public ICommentRepository CommentRepository
        {
            get
            {
                if (_commentRepository == null)
                {
                    _commentRepository = new CommentRepository(_dbContext);
                }

                return _commentRepository;
            }
        }
        public IAssignmentRepository AssignmentRepository
        {
            get
            {
                if (_assignmentRepository == null)
                {
                    _assignmentRepository = new AssignmentRepository(_dbContext);
                }

                return _assignmentRepository;
            }
        }
        public IProjectRepository ProjectRepository
        {
            get
            {
                if (_projectRepository == null)
                {
                    _projectRepository = new ProjectRepository(_dbContext);
                }

                return _projectRepository;
            }
        }

        public RepositoryWrapper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
