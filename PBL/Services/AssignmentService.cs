using Microsoft.EntityFrameworkCore;
using PBL.Data;
using PBL.Models;
using PBL.Services.Interfaces;

namespace PBL.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ApplicationDbContext _context;

        public AssignmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssignmentViewModel>> GetAssignmentsByProjectId(int projectId)
        {
            var assignments = await _context.Assignments
                .Include(a => a.Project)
                .Where(a => a.ProjectId == projectId)
                .OrderByDescending(a => a.DueDate)
                .ToListAsync();

            return assignments.Select(a => new AssignmentViewModel
            {
                AssignmentId = a.Id,
                AssignmentName = a.Name,
                AssignmentDescription = a.Description,
                AssignmentDueDate = a.DueDate,
                AssignmentGrade = a.Grade,
                AssignmentIsCompleted = a.IsCompleted,
                AssignmentIsTurnedIn = a.IsTurnedIn,
                AssigmnemtTurnedInAt = a.TurnedInAt,
                AssignmentDiscriminator = a.Discriminator,
                ProjectName = a.Project.Name,
                ProjectDescription = a.Project.Description,
                ProjectId = a.Project.Id
            }).ToList();
        }

        public async Task<AssignmentViewModel> GetAssignment(int id)
        {
            var assignment = await _context.Assignments
                .Include(a => a.Project)
                .Include(c => c.Comments)
                .Include(f => f.Files)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
            {
                return null;
            }

            var viewModel = new AssignmentViewModel
            {
                AssignmentId = assignment.Id,
                AssignmentName = assignment.Name,
                AssignmentDescription = assignment.Description,
                AssignmentDueDate = assignment.DueDate,
                AssignmentGrade = assignment.Grade,
                AssignmentIsCompleted = assignment.IsCompleted,
                AssignmentIsTurnedIn = assignment.IsTurnedIn,
                AssigmnemtTurnedInAt = assignment.TurnedInAt,
                AssignmentDiscriminator = assignment.Discriminator,
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
                }).ToList() : new List<CommentViewModel>()
            };

            return viewModel;
        }

        public async Task<AssignmentModel> CreateAssignment(AssignmentCreateViewModel model)
        {
            var assignment = new AssignmentModel
            {
                Name = model.Name,
                Description = model.Description,
                DueDate = model.DueDate,
                IsCompleted = model.IsCompleted,
                ProjectId = model.ProjectId,
                //Discriminator= model.Discriminator,
                Grade = 0
            };

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            return assignment;
        }

        public async Task<AssignmentModel> GradeAssignment(int assignmentId, float grade)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);
            if (assignment == null)
            {
                return null;
            }

            assignment.Grade = grade;
            await _context.SaveChangesAsync();

            return assignment;
        }

        public async Task<bool> TurnInAssignment(int assignmentId)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);
            if (assignment == null)
            {
                return false;
            }

            assignment.IsTurnedIn = true;
            assignment.TurnedInAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RevertAssignment(int assignmentId)
        {
            var assignment = await _context.Assignments.FindAsync(assignmentId);
            if (assignment == null)
            {
                return false;
            }

            assignment.IsTurnedIn = false;
            assignment.TurnedInAt = null;
            await _context.SaveChangesAsync();

            return true;
        }
    }
    


}
