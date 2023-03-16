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

        public async Task<IActionResult> IndexAsync()
        {
            var role = new IdentityRole { Name = "Teacher" };
            var result = await _roleManager.CreateAsync(role);

            var usr = await _userManager.FindByEmailAsync("profesor@test.com");
            var rol = await _userManager.FindByNameAsync("Teacher");


            if (usr != null && role != null)
            {
                var resultat = await _userManager.AddToRoleAsync(usr, role.Name);

                if (resultat.Succeeded)
                {
                    Console.WriteLine("e admin");
                }
                else
                {
                    // Failed to add role to user, check the errors in result.Errors
                }
            }

            var users = await _userManager.Users.ToListAsync();
            var userList = new List<SelectListItem>();

            foreach (var user in users)
            {
                userList.Add(new SelectListItem() { Value = user.Id, Text = user.UserName });
            }

            ViewBag.UserList = userList;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
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