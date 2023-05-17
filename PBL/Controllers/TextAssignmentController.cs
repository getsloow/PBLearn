using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL.Controllers.Helpers;
using PBL.Data;
using PBL.Models;
using PBL.Services;
using PBL.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace PBL.Controllers
{
    public class TextAssignmentController : Controller
    {
        private readonly ITextAssignmentService _textAssignmentService;
        public TextAssignmentController(ITextAssignmentService textAssignmentService)
        {
            _textAssignmentService=textAssignmentService;
        }
        public IActionResult Index(int? id)
        {
            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            var assignment = await _textAssignmentService.GetTextAssignmentAsync(id);

            if (assignment == null)
            {
                return NotFound();
            }
            var viewModel = TextAssignmentHelper.MapToTextAssignmentViewModel(assignment);
            return View(viewModel);
        }

        public async Task<IActionResult> TurnIn(int assignmentId, string text)
        {
            await _textAssignmentService.TurnInAsync(assignmentId, text);
            return RedirectToAction("Details", "TextAssignment", new { id = assignmentId });
        }
        public async Task<IActionResult> Revert(int assignmentId)
        {
            await _textAssignmentService.RevertAsync(assignmentId);
            return RedirectToAction("Details", "TextAssignment", new { id = assignmentId });
        }

        public async Task<IActionResult> GradeAssignment(int assignmentId, float assignmentGrade)
        {
            await _textAssignmentService.GradeAsync(assignmentId, assignmentGrade);
            return RedirectToAction("Details", "TextAssignment", new { id = assignmentId });
        }

        public IActionResult Create(int projectId)
        {
            return View(new TextAssignmentModel { ProjectId = projectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TextAssignmentModel model, int projectId)
        {
            if (ModelState.IsValid)
            {
                await _textAssignmentService.CreateAsync(model, projectId);
                return RedirectToAction("Details", "Project", new { id = model.ProjectId });
            }
            return View(model);
        }
    }
}
