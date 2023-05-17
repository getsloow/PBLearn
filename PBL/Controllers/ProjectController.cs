using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL.Data;
using PBL.Models;
using PBL.Services;
using PBL.Services.Interfaces;

namespace PBL.Controllers;

public class ProjectController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IProjectService _projectService;
    public ProjectController(UserManager<IdentityUser> userManager, IProjectService projectService)
    {
        _userManager = userManager;
        _projectService = projectService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var userProjects = new List<ProjectModel>();

        if (await _userManager.IsInRoleAsync(currentUser, "Teacher"))
        {
            userProjects = await _projectService.GetAllProjects();
        }
        else
        {
            userProjects = await _projectService.GetAllProjectsByEmail(User.Identity!.Name!);
        }

        return View(userProjects);
    }

    [Authorize(Roles = "Teacher")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate")] ProjectModel project)
    {
        project.Grade = 0;
        if (ModelState.IsValid)
        {
            await _projectService.CreateAsync(project);
            return RedirectToAction(nameof(Index));
        }
        return View(project);
    }

    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Grade(int projectId, float ProjectGrade)
    {
        await _projectService.GradeAsync(projectId, ProjectGrade);
        return RedirectToAction("Details", "Project", new { id = projectId });
    }

    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Delete(int? id)
    {
        var project = await _projectService.DetailsAsync(id);
        if (project == null)
        {
            return NotFound();
        }
        var viewModel = ProjectHelper.MapToProjectDetailsViewModel(project);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _projectService.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int? id)
    {
        var project = await _projectService.DetailsAsync(id);

        if (project == null)
        {
            return NotFound();
        }
        var viewModel = ProjectHelper.MapToProjectDetailsViewModel(project);
        return View(viewModel);
    }
}