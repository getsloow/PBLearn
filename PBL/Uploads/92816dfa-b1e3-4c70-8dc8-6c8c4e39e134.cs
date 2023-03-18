using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL.Data;

namespace PBL.Controllers
{
    public class AllocateController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AllocateController (UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager=userManager;
            _roleManager=roleManager;
            _context=context;
        }

        [HttpPost]
        public async Task<IActionResult> Allocate(int projectId, string userId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            project.UserEmail = user.Email;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Allocate));
        }


        public async Task<IActionResult> Index()
        {
            var lista = await _userManager.GetUsersInRoleAsync("Teacher");
            var users = await _userManager.Users.Where(u => !lista.Contains(u)).ToListAsync();
            var projects = await _context.Projects.ToListAsync();

            var projectList = new List<SelectListItem>();
            var userList = new List<SelectListItem>();

            foreach (var user in users)
            {
                userList.Add(new SelectListItem() { Value = user.Id, Text = user.UserName });
            }

            foreach (var project in projects)
            {
                projectList.Add(new SelectListItem() { Value = project.Id.ToString(), Text = project.Name });
            }
            ViewBag.UserList = userList;
            ViewBag.ProjectList = projectList;

            return View();
        }
    }
}
