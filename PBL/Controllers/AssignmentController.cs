using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL.Data;
using PBL.Models;
using System.ComponentModel.Design;

namespace PBL.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentController(ApplicationDbContext context)
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
            var assignment = _context.Assignments
                            .Include(a => a.Project)
                            .Include(c => c.Comments)
                            .Include(f => f.Files)
                            .FirstOrDefault(a => a.Id == id);

            if (assignment == null)
            {
                // If the assignment was not found, return a 404 Not Found status code
                return NotFound();
            }

            var viewModel = new AssignmentViewModel
            {
                AssignmentId = assignment.Id,
                AssignmentName = assignment.Name,
                AssignmentDescription = assignment.Description,
                AssignmentDueDate = assignment.DueDate,
                AssignmentGrade = assignment.Grade,
                AssignmentIsCompleted = assignment.IsCompleted,
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

        public async Task<IActionResult> GradeAssignment(int assignmentId, float AssignmentGrade)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);
            if (assignment == null)
            {
                return NotFound();
            }

            assignment.Grade = AssignmentGrade;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Assignment", new { id = assignment.Id });
        }

        public IActionResult Create(int projectId)
        {
            var model = new AssignmentCreateViewModel
            {

                ProjectId = projectId,
                
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AssignmentCreateViewModel model)
        {


            if (ModelState.IsValid)
            {
                var assignment = new AssignmentModel
                {
                    Name = model.Name,
                    Description = model.Description,
                    DueDate = model.DueDate,
                    IsCompleted = model.IsCompleted,
                    ProjectId = model.ProjectId,
                    Grade = 0
                };

                _context.Assignments.Add(assignment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Project", new { id = model.ProjectId });
            }

            return View(model);
        }

    }
}
