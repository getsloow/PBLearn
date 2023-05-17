using PBL.Models;

namespace PBL.Controllers.Helpers
{
    public static class AssignmentHelper
    {
        public static AssignmentViewModel MapToAssignmentViewModel(AssignmentModel assignment)
        {
            var viewModel = new AssignmentViewModel
            {
                AssignmentId = assignment.Id,
                AssignmentName = assignment.Name,
                AssignmentDescription = assignment.Description,
                AssignmentDueDate = (DateTime)assignment.DueDate!,
                AssignmentGrade = assignment.Grade,
                AssignmentIsCompleted = (bool)assignment.IsCompleted!,
                AssignmentIsTurnedIn = (bool)assignment.IsTurnedIn!,
                AssigmnemtTurnedInAt = assignment.TurnedInAt,
                AssignmentDiscriminator = assignment.Discriminator,
                ProjectName = assignment.Project!.Name,
                ProjectDescription = assignment.Project.Description,
                ProjectId = (int)assignment!.ProjectId!,
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
            return viewModel;
        }
    }
}
