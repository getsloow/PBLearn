namespace PBL.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        ICommentRepository CommentRepository { get; }
        IProjectRepository ProjectRepository { get; }
        IAssignmentRepository AssignmentRepository { get; }
        ITextAssignmentRepository TextAssignmentRepository { get; }
        IFileRepository FileRepository { get; }
        void Save();
    }
}
