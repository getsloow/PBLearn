using Microsoft.AspNetCore.Mvc;
using PBL.Models;
using PBL.Repositories.Interfaces;
using PBL.Services.Interfaces;

namespace PBL.Services;

public class CommentService : ICommentService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public CommentService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }
    public void AddComment(int? projectId, string text, int? assignmentId, string userName)
    {
        var comment = new CommentModel
        {
            AssignmentId = assignmentId,
            ProjectId = projectId,
            Text = text,
            PostedOn = DateTime.Now,
            PostedBy = userName
        };
        _repositoryWrapper.CommentRepository.Create(comment);
        _repositoryWrapper.Save();
    }

    public void DeleteComment(int commentId)
    {
        var comment = _repositoryWrapper.CommentRepository.FindByCondition(c => c.Id == commentId).SingleOrDefault();
        _repositoryWrapper.CommentRepository.Delete(comment!);
        _repositoryWrapper.Save();
    }
}
