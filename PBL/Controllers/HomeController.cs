using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL.Data;
using PBL.Models;
using System.Diagnostics;

namespace PBL.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context,RoleManager<IdentityRole> role, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context=context;
            _userManager = userManager;
            _roleManager = role;
        }

        public async Task<IActionResult> Index()
        {
            var roleCreate = new IdentityRole { Name = "Teacher" };
            var result = await _roleManager.CreateAsync(roleCreate);

            var role = await _roleManager.FindByNameAsync("Teacher");
            if (role == null)
            {
                // Role does not exist, handle the error
            }

            var user = await _userManager.FindByEmailAsync("profesor@test.com");
            if (user == null)
            {
                // User does not exist, handle the error
            }

            var isInRole = await _userManager.IsInRoleAsync(user, "Teacher");


            if (user != null && role != null)
            {
                var resultat = await _userManager.AddToRoleAsync(user, role.Name);

                if (resultat.Succeeded)
                {
                    Console.WriteLine("e admin");
                }
                else
                {
                    // Failed to add role to user, check the errors in result.Errors
                }
            }
            var lista = await _userManager.GetUsersInRoleAsync("Teacher");
            var users = await _userManager.Users.Where(u => !lista.Contains(u)).ToListAsync();
            var userList = new List<SelectListItem>();

            foreach (var userListElements in users)
            {
                userList.Add(new SelectListItem() { Value = userListElements.Id, Text = userListElements.UserName });
            }

            ViewBag.UserList = userList;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Allocate()
        {
            var lista = await _userManager.GetUsersInRoleAsync("Teacher");
            var users = await _userManager.Users.Where(u => !lista.Contains(u)).ToListAsync();
            var projects = await _context.Projects.Where(p => string.IsNullOrEmpty(p.UserEmail)).ToListAsync();
            var allocatedProjects = await _context.Projects.Where(p => !string.IsNullOrEmpty(p.UserEmail)).ToListAsync();
            var projectList = new List<SelectListItem>();
            var userList = new List<SelectListItem>();
            var allocatedProjectsList = new List<SelectListItem>();

            foreach (var allocs in allocatedProjects)
            {
                allocatedProjectsList.Add(new SelectListItem() { Value = allocs.Id.ToString(), Text = allocs.Name });
            }
            

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
            ViewBag.AllocatedProjectsList = allocatedProjectsList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Allocate(int Id, string UserEmail)
        {
            var project = await _context.Projects.FindAsync(Id);
            if (project == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(UserEmail);
            if (user == null)
            {
                return NotFound();
            }

            project.UserEmail = user.Email;
            await _context.SaveChangesAsync();

            return RedirectToAction("Allocate", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> DeAllocate(int Id)
        {
            var project = await _context.Projects.FindAsync(Id);
            if (project == null)
            {
                return NotFound();
            }



            project.UserEmail = null;
            await _context.SaveChangesAsync();

            return RedirectToAction("Allocate", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public class FileUploadController : Controller
        {
            private readonly ApplicationDbContext _context;

            public FileUploadController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public IActionResult FileUpload()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Index(IFormFile file)
            {
                if (file == null || file.Length == 0)
                {
                    return Content("File not selected");
                }

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    var fileData = stream.ToArray();

                    // save the file data to the database using your DbContext
                }

                return RedirectToAction("Index");
            }
        }

    }
}