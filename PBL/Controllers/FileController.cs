using Microsoft.AspNetCore.Mvc;
using PBL.Data;
using PBL.Models;

namespace PBL.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public FileController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(FileUploadViewModel model, int? projectId, int? assignmentId)
        {
            if (model.File == null || model.File.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file to upload.");
                return View(model);
            }

            // Save the file to the Uploads folder
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Save the file information to the database
            var file = new Models.File
            {
                Name = model.File.FileName,
                Location = fileName,
                ProjectId = projectId,
                AssignmentId= assignmentId,

            };
            _dbContext.Files.Add(file);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }

}
