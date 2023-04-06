namespace PBL.Models
{
    public class FileUploadViewModel
    {
        public IFormFile File { get; set; }
        public int? ProjectId { get; set; }
        public int? AssignmentId { get; set; }
        public string UploadedBy { get; set; }

    }
}
