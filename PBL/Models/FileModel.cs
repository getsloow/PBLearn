namespace PBL.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int? ProjectId { get; set; }
        public int? AssignmentId { get; set; }
        public virtual ProjectModel Project { get; set; }
        public virtual AssignmentModel Assignment { get; set; }
        public string UploadedBy { get; set; }
    }
}
