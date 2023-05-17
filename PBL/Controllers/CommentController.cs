using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBL.Services.Interfaces;

namespace PBL.Controllers;

[Authorize]
public class CommentController : Controller
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService=commentService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddComment(int? projectId, string text, int? assignmentId)
    {
        try
        {
            _commentService.AddComment(projectId, text, assignmentId, User.Identity!.Name!);
        }
        catch (ArgumentException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return Redirect(projectId, assignmentId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteComment(int commentId, int? projectId, int? assignmentId)
    {
        try
        {
            _commentService.DeleteComment(commentId);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return Redirect(projectId, assignmentId);
    }

    private RedirectToActionResult Redirect(int? projectId, int? assignmentId)
    {
        return assignmentId != null
            ? RedirectToAction("Details", "Assignment", new { id = assignmentId })
            : projectId != null
                  ? RedirectToAction("Details", "Project", new { id = projectId })
                : RedirectToAction("Index", "Home");
    }
}