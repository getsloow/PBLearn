using Microsoft.AspNetCore.Mvc;
using PBL.Controllers.Helpers;
using PBL.Models;
using PBL.Services.Interfaces;

namespace PBL.Controllers;

public class AssignmentController : Controller
{
    private readonly IAssignmentService _assignmentService;
    public AssignmentController(IAssignmentService assignmentService)
    {
        _assignmentService=assignmentService;
    }
    public IActionResult Index(int? id)
    {
        return View();
    }
    public async Task<IActionResult> Details(int id)
    {
        var assignment = await _assignmentService.GetAssignmentAsync(id);

        if (assignment == null)
        {
            return NotFound();
        }
        var viewModel = AssignmentHelper.MapToAssignmentViewModel(assignment);
        return View(viewModel);
    }

    public async Task<IActionResult> GradeAssignment(int assignmentId, float assignmentGrade)
    {
        await _assignmentService.GradeAsync(assignmentId, assignmentGrade);
        return RedirectToAction("Details", "Assignment", new { id = assignmentId });
    }

    public IActionResult Create(int projectId)
    {
        return View(new AssignmentModel { ProjectId = projectId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AssignmentModel model, int projectId)
    {
        if (ModelState.IsValid)
        {
            await _assignmentService.CreateAsync(model, projectId);
            return RedirectToAction("Details", "Project", new { id = model.ProjectId });
        }
        return View(model);
    }
    public async Task<IActionResult> TurnIn(int assignmentId)
    {
        await _assignmentService.TurnInAsync(assignmentId);
        return RedirectToAction("Details", "Assignment", new { id = assignmentId });
    }
    public async Task<IActionResult> Revert(int assignmentId)
    {
        await _assignmentService.RevertAsync(assignmentId);
        return RedirectToAction("Details", "Assignment", new { id = assignmentId });
    }
}
