using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL.Models;
using PBL.Repositories.Interfaces;
using PBL.Services.Interfaces;
using System.IO.Compression;

namespace PBL.Services
{
    public class FileService : IFileService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IRepositoryWrapper repositoryWrapper, IWebHostEnvironment webHostEnvironment)
        {
            _repositoryWrapper = repositoryWrapper;
            _webHostEnvironment=webHostEnvironment;
        }

        public async Task Delete(int fileId)
        {
            var file = await _repositoryWrapper.FileRepository.FindByCondition(c => c.Id== fileId).FirstOrDefaultAsync();
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", file!.Location);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _repositoryWrapper.FileRepository.Delete(file);
            _repositoryWrapper.Save();
        }

        public string GetFileName(int fileId)
        {
            var file = _repositoryWrapper.FileRepository.FindByCondition(c => c.Id== fileId).FirstOrDefault();
            return file!.Name;
        }
        
        public string GetProjectName(int? projectId, int? assignmentId)
        {
            var project = _repositoryWrapper.ProjectRepository.FindByCondition(c=> c.Id == projectId).FirstOrDefault();
            var assignment = _repositoryWrapper.AssignmentRepository.FindByCondition(c=> c.Id == assignmentId).FirstOrDefault();
            return $"{project?.Name} - {assignment?.Name} {DateTime.Now:dd}.{DateTime.Now:MM}.{DateTime.Now:yyyy}.{DateTime.Now:t}.zip";
        }
        public Stream Download(int fileId)
        {
            var file =  _repositoryWrapper.FileRepository.FindByCondition(c => c.Id== fileId).FirstOrDefault();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file!.Location);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return fileStream;
        }

        public Stream DownloadAll(int? projectId, int? assignmentId)
        {
            var files = _repositoryWrapper.FileRepository.FindByCondition(f => (projectId.HasValue && f.ProjectId == projectId.Value) || (assignmentId.HasValue && f.AssignmentId == assignmentId.Value))
                .ToList();
            var project = _repositoryWrapper.ProjectRepository.FindByCondition(f => f.Id == projectId).FirstOrDefault(f => f.Id == projectId);
            var archiveName = $"{project?.Name} - {DateTime.Now:dd}.{DateTime.Now:MM}.{DateTime.Now:yyyy}.{DateTime.Now:t}.zip";
            var archivePath = Path.Combine(Directory.GetCurrentDirectory(), archiveName);
            using (var archive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
            {
                foreach (var file in files)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.Location);
                    if (File.Exists(filePath))
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
            var archiveStream = new FileStream(archivePath, FileMode.Open, FileAccess.Read);
            return archiveStream;
        }
        public async Task Upload(FileUploadViewModel model, int? projectId, int? assignmentId, string uploadedBy)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Save the file information to the database
            var file = new Models.FileModel
            {
                Name = model.File.FileName,
                Location = fileName,
                ProjectId = projectId,
                AssignmentId = assignmentId,
                UploadedBy = uploadedBy
            };
            _repositoryWrapper.FileRepository.Create(file);
            _repositoryWrapper.Save();
        }
    }
}
