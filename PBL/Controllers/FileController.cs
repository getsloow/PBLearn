using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PBL.Data;
using PBL.Models;
using System.IO.Compression;

namespace PBL.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment=webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int fileId)
        {
            var file = await _dbContext.Files.FindAsync(fileId);
            var type = "";

            
            if (file == null)
            {
                return NotFound();
            }
            else
            {
                if (file.AssignmentId != null)
                {
                    type = "a";
                }
                else
                {
                    type = "p";
                }
            }

            // Delete the file from the file system
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", file.Location);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Delete the file from the database
            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();

            if (type == "a") { return RedirectToAction("Details", "Assignment", new { id = file.AssignmentId }); }
            else if (type == "p" ) { return RedirectToAction("Details", "Project", new { id = file.ProjectId }); }
            else
                return View();
        }
        public IActionResult Download(int id)
        {
            var file = _dbContext.Files.FirstOrDefault(f => f.Id == id);
            if (file == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.Location);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return File(fileStream, "application/octet-stream", file.Name);
        }

        public IActionResult DownloadAll(int? projectId, int? assignmentId)
        {
            // Get all files that match the projectId or assignmentId
            var files = _dbContext.Files
                .Where(f => f.ProjectId == projectId || f.AssignmentId == assignmentId)
                .ToList();
            var project = _dbContext.Projects.FirstOrDefault(f => f.Id == projectId);
            var archiveName = $"{project?.Name} - {DateTime.Now:dd}.{DateTime.Now:MM}.{DateTime.Now:yyyy}.{DateTime.Now:t}.zip";
            var archivePath = Path.Combine(Directory.GetCurrentDirectory(), archiveName);
            using (var archive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
            {
                // Add each file to the archive
                foreach (var file in files)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.Location);
                    if (System.IO.File.Exists(filePath))
                    {
                        var archiveEntry = archive.CreateEntry(file.Name);
                        using (var stream = archiveEntry.Open())
                        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            fileStream.CopyTo(stream);
                        }
                    }
                }
            }

            // Download the archive file
            var archiveStream = new FileStream(archivePath, FileMode.Open, FileAccess.Read);
            return File(archiveStream, "application/zip", archiveName);
        }

        [HttpGet]
        public IActionResult Upload(int? projectId, int? assignmentId, string? uploadedBy)
        {
            var model = new FileUploadViewModel
            {
                ProjectId = projectId,
                AssignmentId = assignmentId,
                UploadedBy = uploadedBy
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(FileUploadViewModel model, int? projectId, int? assignmentId, string uploadedBy)
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
                AssignmentId = assignmentId,
                UploadedBy = uploadedBy
            };
            _dbContext.Files.Add(file);
            await _dbContext.SaveChangesAsync();

            if (assignmentId != null) { return RedirectToAction("Details", "Assignment", new { id = assignmentId }); }
            else if (projectId != null) { return RedirectToAction("Details", "Project", new { id = projectId }); }
            else
                return View();
        }
    }

}
