using Microsoft.AspNetCore.Mvc;
using PBL.Models;

namespace PBL.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<List<AssignmentViewModel>> GetAssignmentsByProjectId(int projectId);
        Task<AssignmentViewModel> GetAssignment(int id);
        Task<AssignmentModel> CreateAssignment(AssignmentCreateViewModel model);
        Task<AssignmentModel> GradeAssignment(int assignmentId, float grade);
        Task<bool> TurnInAssignment(int assignmentId);
        Task<bool> RevertAssignment(int assignmentId);
    }
}
