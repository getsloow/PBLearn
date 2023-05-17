using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL.Data;
using PBL.Models;
using PBL.Repositories.Interfaces;
using PBL.Services.Interfaces;

namespace PBL.Services
{
    public class TextAssignmentService : ITextAssignmentService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public TextAssignmentService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<TextAssignmentModel?> GetTextAssignmentAsync(int? id)
        {
            return await _repositoryWrapper.TextAssignmentRepository.FindByCondition(p => p.Id == id)
                   .Include(p => p.Project)
                   .Include(a => a.Files)
                   .Include(c => c.Comments)
                   .FirstOrDefaultAsync(n => n.Id == id);
        }
        public async Task GradeAsync(int assignmentId, float assignmentGrade)
        {
            var assignment = await _repositoryWrapper.TextAssignmentRepository.FindByCondition(p => p.Id == assignmentId).FirstOrDefaultAsync();
            assignment!.Grade = assignmentGrade;
            _repositoryWrapper.TextAssignmentRepository.Update(assignment);
            _repositoryWrapper.Save();
        }

        public async Task CreateAsync(TextAssignmentModel model, int projectId)
        {
            var assignment = new TextAssignmentModel
            {
               
                Name = model.Name,
                Description = model.Description,
                Grade = 0,
                DueDate = model.DueDate,
                IsCompleted = false,
                IsTurnedIn = false,
                ProjectId = model.ProjectId
            };

            _repositoryWrapper.TextAssignmentRepository.Create(assignment);
            _repositoryWrapper.Save();
        }

        public async Task TurnInAsync(int assignmentId, string text)
        {
            var assignment = await _repositoryWrapper.TextAssignmentRepository.FindByCondition(p => p.Id == assignmentId).FirstOrDefaultAsync();
            assignment!.IsTurnedIn = true;
            assignment.TurnedInAt = DateTime.Now;
            assignment.Text = text;
            _repositoryWrapper.TextAssignmentRepository.Update(assignment);
            _repositoryWrapper.Save();
        }

        public async Task RevertAsync(int assignmentId)
        {
            var assignment = await _repositoryWrapper.TextAssignmentRepository.FindByCondition(p => p.Id == assignmentId).FirstOrDefaultAsync();
            assignment!.IsTurnedIn = false;
            assignment.TurnedInAt = null;
            _repositoryWrapper.TextAssignmentRepository.Update(assignment);
            _repositoryWrapper.Save();
        }
    }
}
