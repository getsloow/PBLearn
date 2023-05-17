namespace PBL.Services.Interfaces
{
    public interface ICommentService
    {
        void AddComment(int? projectId, string text, int? assignmentId, string userName);
        void DeleteComment(int commentId);
    }
}