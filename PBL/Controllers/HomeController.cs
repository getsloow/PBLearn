using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL.Data;
using PBL.Models;
using PBL.Repositories.Interfaces;
using PBL.Services.Interfaces;
using System.Diagnostics;

namespace PBL.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAllocateService _allocateService;
    private readonly IProjectService _projectService;

    public HomeController(IProjectService projectService, IAllocateService allocateService, UserManager<IdentityUser> userManager)
    {
        _allocateService = allocateService;
        _projectService = projectService;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public async Task<IActionResult> Allocate()
    {
        var filter = await _userManager.GetUsersInRoleAsync("Teacher");
        var users = await _userManager.Users.Where(u => !filter.Contains(u)).ToListAsync();

        var userList = users.Select(user => new SelectListItem { Value = user.Email, Text = user.UserName }).ToList();
        var allocatedProjects = await _projectService.GetAllocatedProjects();
        var unallocatedProjects = await _projectService.GetUnallocatedProjects();

        ViewBag.ProjectList = unallocatedProjects;
        ViewBag.AllocatedProjectsList = allocatedProjects;
        ViewBag.UserList = userList;

        return View();
    }

    [HttpPost]
    public IActionResult Allocate(int Id, string UserEmail)
    {
        _allocateService.Allocate(Id, UserEmail);
        return RedirectToAction("Allocate", "Home");
    }

    [HttpPost]
    public IActionResult DeAllocate(int Id)
    {
        _allocateService.DeAllocate(Id);
        return RedirectToAction("Allocate", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}