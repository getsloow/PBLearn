using Microsoft.AspNetCore.Mvc;
using PBL.Data;
using PBL.Models;
using System.Diagnostics;

namespace PBL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context=context;
        }

        public IActionResult Index()
        {
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