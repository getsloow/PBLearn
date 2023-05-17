namespace PBL.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        ICommentRepository CommentRepository { get; }
        IProjectRepository ProjectRepository { get; }

        void Save();
    }
}
