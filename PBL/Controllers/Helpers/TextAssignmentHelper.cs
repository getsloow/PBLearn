using PBL.Models;

namespace PBL.Controllers.Helpers
{
    public static class TextAssignmentHelper
    {
        public static TextAssignmentViewModel MapToTextAssignmentViewModel(TextAssignmentModel assignment)
        {
            var viewModel = new TextAssignmentViewModel
            {
                AssignmentId = assignment.Id,
                AssignmentName = assignment.Name,
                AssignmentDescription = assignment.Description,
                AssignmentDueDate = (DateTime)assignment.DueDate!,
                AssignmentGrade = assignment.Grade,
                AssignmentText = assignment.Text,
                AssignmentIsCompleted = (bool)assignment.IsCompleted!,
                AssignmentIsTurnedIn = (bool)assignment.IsTurnedIn!,
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

            return viewModel;
        }
    }
}
