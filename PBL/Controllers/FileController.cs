using Microsoft.AspNetCore.Mvc;
using PBL.Models;
using PBL.Services.Interfaces;

namespace PBL.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int fileId, int? projectId, int? assignmentId)
        {
            await _fileService.Delete(fileId);
            return Redirect(projectId, assignmentId);
        }
        public IActionResult Download(int id)
        {
            var fileStream = _fileService.Download(id);
            var fileName = _fileService.GetFileName(id);

            return File((Stream)fileStream, "application/octet-stream", fileName);
        }

        public IActionResult DownloadAll(int? projectId, int? assignmentId)
        {
            var archiveStream = _fileService.DownloadAll(projectId, assignmentId);
            var archiveName = _fileService.GetProjectName(projectId, assignmentId);
            return File(archiveStream, "application/zip", archiveName);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(FileUploadViewModel model, int? projectId, int? assignmentId, string uploadedBy)
        {
            if (model.File == null || model.File.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file to upload.");
                return View(model);
            }

            await _fileService.Upload(model, projectId, assignmentId, uploadedBy);
            return Redirect(projectId, assignmentId);
        }
        private RedirectToActionResult Redirect(int? projectId, int? assignmentId)
        {
            return assignmentId != null
                ? RedirectToAction("Details", "Assignment", new { id = assignmentId })
                : projectId != null
                      ? RedirectToAction("Details", "Project", new { id = projectId })
                    : RedirectToAction("Index", "Home");
        }
    }
}