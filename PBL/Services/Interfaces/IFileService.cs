using Microsoft.AspNetCore.Mvc;
using PBL.Models;

namespace PBL.Services.Interfaces
{
    public interface IFileService
    {
        Task Upload(FileUploadViewModel model, int? projectId, int? assignmentId, string uploadedBy);
        Task Delete(int fileId);
        string GetFileName(int fileId);
        string GetProjectName(int? projectId, int? assignmentId);
        Stream Download(int fileId);
        Stream DownloadAll(int? projectId, int? assignmentId);
    }
}