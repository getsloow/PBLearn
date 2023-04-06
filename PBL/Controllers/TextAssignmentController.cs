using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL.Data;
using PBL.Models;
using static System.Net.Mime.MediaTypeNames;

namespace PBL.Controllers
{
    public class TextAssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TextAssignmentController(ApplicationDbContext context)
        {
            _context=context;
        }
        public IActionResult Index(int? id)
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            // Retrieve the assignment from the database using its ID
            var assignment = _context.TextAssignments
                            .Include(a => a.Project)
                            .Include(c => c.Comments)
                            .Include(f => f.Files)
                            .FirstOrDefault(a => a.Id == id);

            if (assignment == null)
            {
                // If the assignment was not found, return a 404 Not Found status code
                return NotFound();
            }

            var viewModel = new TextAssignmentViewModel
            {
                AssignmentId = assignment.Id,
                AssignmentName = assignment.Name,
                AssignmentDescription = assignment.Description,
                AssignmentDueDate = assignment.DueDate,
                AssignmentGrade = assignment.Grade,
                AssignmentText = assignment.Text,
                AssignmentIsCompleted = assignment.IsCompleted,
                AssignmentIsTurnedIn = assignment.IsTurnedIn,
                AssigmnemtTurnedInAt = assignment.TurnedInAt,
                ProjectName = assignment.Project.Name,
                ProjectDescription = assignment.Project.Description,
                ProjectId = assignment.Project.Id,
                Files = assignment.Files != null ? assignment.Files.Select(f => new FileViewModel
                {
                    FileId = f.Id,
                    FileLocation = f.Location,
                    FileName = f.Name,
                    UploadedBy = f.UploadedBy
                }).ToList() : new List<FileViewModel>(),
                Comments = assignment.Comments != null ? assignment.Comments.Select(c => new CommentViewModel
                {
                    CommentId = c.Id,
                    CommentText = c.Text,
                    CreatedAt = c.PostedOn,
                    CreatedBy = c.PostedBy
                }
                ).ToList() : new List<CommentViewModel>()
            };

            // Pass the assignment to the view
            return View(viewModel);
        }

        public async Task<IActionResult> TurnIn(int assignmentId, string text)
        {
            var assignment = await _context.TextAssignments.FindAsync(assignmentId);
            if (assignment == null)
            {
                return NotFound();
            }

            assignment.Text = text;
            assignment.IsTurnedIn = true;
            assignment.TurnedInAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "TextAssignment", new { id = assignment.Id });

        }
        public async Task<IActionResult> Revert(int assignmentId)
        {
            var assignment = await _context.TextAssignments.FindAsync(assignmentId);
            if (assignment == null)
            {
                return NotFound();
            }

           
            assignment.IsTurnedIn = false;
            assignment.TurnedInAt = null;
           await _context.SaveChangesAsync();

            return RedirectToAction("Details", "TextAssignment", new { id = assignment.Id });


        }

        public async Task<IActionResult> GradeAssignment(int assignmentId, float AssignmentGrade)
        {
            var assignment = await _context.TextAssignments.FindAsync(assignmentId);
            if (assignment == null)
            {
                return NotFound();
            }

            assignment.Grade = AssignmentGrade;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "TextAssignment", new { id = assignment.Id });
        }

        public IActionResult Create(int projectId)
        {
            var model = new TextAssignmentCreateViewModel
            {

                ProjectId = projectId,
                IsCompleted = false,
                Text = null

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TextAssignmentCreateViewModel model)
        {


            if (ModelState.IsValid)
            {
                var assignment = new TextAssignmentModel
                {
                    Name = model.Name,
                    Description = model.Description,
                    DueDate = model.DueDate,
                    IsCompleted = model.IsCompleted,
                    ProjectId = model.ProjectId,
                    Grade = 0,
                    Text = model.Text
                };

                _context.TextAssignments.Add(assignment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Project", new { id = model.ProjectId });
            }

            return View(model);
        }


    }
}
