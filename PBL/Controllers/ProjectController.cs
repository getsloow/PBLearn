using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL.Data;
using PBL.Models;

namespace PBL.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult>Index()
        {
            return View(await _context.Projects.ToListAsync());
        }

        // GET: Project/Create
        [Authorize(Roles = "Profesor")]
        public IActionResult Create()
        {
            return View();
        }
        // POST: Project/Create
        [HttpPost]
        [Authorize(Roles = "Profesor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> Grade(int projectId, float ProjectGrade)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            project.Grade = ProjectGrade;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Project", new { id = project.Id });
        }

        [Authorize(Roles = "Profesor")]
        public async  Task<IActionResult> Delete(int? id)
        {
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var project = await _context.Projects
                                .Include(p => p.Assignments)
                                .ThenInclude(a => a.Comments)
                                .FirstOrDefaultAsync(p => p.Id == id);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

            if (project == null)
            {
                return NotFound();
            }

            var viewModel = new ProjectDetailsViewModel
            {
                ProjectId = project.Id,
                ProjectName = project.Name,
                ProjectGrade= project.Grade,
                ProjectStartDate = project.StartDate,
                ProjectEndDate = project.EndDate,
                Assignments = project.Assignments?.Select(a => new AssignmentViewModel
                {
                    AssignmentId = a.Id,
                    AssignmentName = a.Name,
                    AssignmentDescription = a.Description,
                    AssignmentDueDate = a.DueDate,
                    AssignmentIsCompleted = a.IsCompleted
                }
                ).ToList(),
                Comments = project.Comments?.Select(c => new CommentViewModel
                {
                  CommentId = c.Id,
                  CommentText = c.Text,
                  CreatedAt = c.PostedOn,
                  CreatedBy = c.PostedBy
                }
                ).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //[Authorize(Roles = "Profesor")]
        public async Task<IActionResult>Details(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var proiect = await _context.Projects
                                .Include(p =>p.Assignments)
                                .Include(a => a.Comments)
                                .FirstOrDefaultAsync(n => n.Id == id);

            if (proiect == null)
            {
                return NotFound();
            }
            var viewModel = new ProjectDetailsViewModel
            {
                ProjectId = proiect.Id,
                ProjectName = proiect.Name,
                ProjectStartDate = proiect.StartDate,
                ProjectGrade = proiect.Grade,
                ProjectDescription = proiect.Description,
                ProjectEndDate = proiect.EndDate,
                Assignments = proiect.Assignments.Select(a => new AssignmentViewModel
                {
                    AssignmentId = a.Id,
                    AssignmentName = a.Name,
                    AssignmentDescription= a.Description,
                    AssignmentDueDate = a.DueDate,
                    AssignmentIsCompleted = a.IsCompleted
                }).ToList()
                ,
                Comments = proiect.Comments != null ? proiect.Comments.Select(c => new CommentViewModel
                {
                    
                    CommentId = c.Id,
                    CommentText = c.Text,
                    CreatedAt = c.PostedOn,
                    CreatedBy = c.PostedBy
                }
                ).ToList() : new List<CommentViewModel>()
            };
            return View(viewModel);
        }
           
    }
}
