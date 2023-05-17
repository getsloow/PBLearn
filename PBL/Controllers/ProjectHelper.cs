using PBL.Models;

namespace PBL.Controllers;

public static class ProjectHelper
{
    public static ProjectDetailsViewModel MapToProjectDetailsViewModel(ProjectModel project)
    {
        var viewModel = new ProjectDetailsViewModel
        {
            ProjectId = project.Id,
            ProjectName = project.Name,
            ProjectStartDate = project.StartDate,
            ProjectGrade = project.Grade,
            ProjectDescription = project.Description,
            ProjectEndDate = project.EndDate,
            UserEmail = project.UserEmail,
            Assignments = project.Assignments?.Select(a => new AssignmentViewModel
            {
                AssignmentId = a.Id,
                AssignmentName = a.Name,
                AssignmentDescription= a.Description,
                AssignmentDueDate = a.DueDate,
                AssignmentIsCompleted = a.IsCompleted,
                AssignmentDiscriminator = a.Discriminator

            }).OrderBy(a => a.AssignmentDueDate).ToList()
           ,
            Files = project.Files?.Select(p => new FileViewModel
            {
                FileId = p.Id,
                FileName = p.Name,
                FileLocation = p.Location,
                UploadedBy = p.UploadedBy
            }).ToList()
           ,
            Comments = project.Comments != null ? project.Comments.Select(c => new CommentViewModel
            {

                CommentId = c.Id,
                CommentText = c.Text,
                CreatedAt = c.PostedOn,
                CreatedBy = c.PostedBy
            }
           ).ToList() : new List<CommentViewModel>()
        };

        if (viewModel.Assignments != null)
        {
            foreach (var assigns in viewModel.Assignments)
            {
                if (assigns.AssignmentDueDate < DateTime.UtcNow)
                    assigns.AssignmentIsCompleted = false;
                else
                    assigns.AssignmentIsCompleted = true;
            }
        }

        return viewModel;
    }
}
