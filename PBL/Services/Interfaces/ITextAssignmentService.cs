using Microsoft.AspNetCore.Mvc;
using PBL.Models;

namespace PBL.Services.Interfaces
{
    public interface ITextAssignmentService
    {
        Task<TextAssignmentModel?> GetTextAssignmentAsync(int? id);
        Task GradeAsync(int assignmentId, float assignmentGrade);
        Task CreateAsync(TextAssignmentModel model, int projectId);
        Task TurnInAsync(int assignmentId, string text);
        Task RevertAsync(int assignmentId);
    }
}
