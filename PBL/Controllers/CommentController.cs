using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBL.Data;
using PBL.Models;

namespace PBL.Controllers
{

    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int? projectId, string text, int? assignmentId)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                TempData["ErrorMessage"] = "Comment text is required.";
                return RedirectToAction("Details", "Project", new { id = projectId });
            }

            var comment = new Comment
            {
                Text = text.Trim(),
                PostedOn = DateTime.UtcNow,
                ProjectId = projectId,
                AssignmentId = assignmentId,
                PostedBy = User?.Identity?.Name // or use a custom user identity if you have implemented one
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Comment added successfully.";
            if (assignmentId != null) { return RedirectToAction("Details", "Assignment", new { id = assignmentId }); }
            else if (projectId != null) { return RedirectToAction("Details", "Project", new { id = projectId }); }
            else
                return View();
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int commentId, string view)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            if (view == "AssignmentDetails")
                return RedirectToAction("Details", "Assignment", new { id = comment.AssignmentId });
            else if (view == "ProjectDetails")
                return RedirectToAction("Details", "Project", new { id = comment.ProjectId });
              else
            {
                return View();
            }
        }

    }
}
