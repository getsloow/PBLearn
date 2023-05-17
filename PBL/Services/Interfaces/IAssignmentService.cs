using Microsoft.AspNetCore.Mvc;
using PBL.Models;

namespace PBL.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<AssignmentModel?> GetAssignmentAsync(int? id);
        Task GradeAsync(int assignmentId, float assignmentGrade);
        Task CreateAsync(AssignmentModel model, int projectId);
        Task TurnInAsync(int assignmentId);
        Task RevertAsync(int assignmentId);

    }
}
