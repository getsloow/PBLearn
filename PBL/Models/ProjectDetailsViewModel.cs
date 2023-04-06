namespace PBL.Models
{
    public class ProjectDetailsViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public float? ProjectGrade { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public string? UserEmail { get; set; }
        public List<AssignmentViewModel>? Assignments { get; set; }
        public List<CommentViewModel>? Comments { get; set; }
        public List<FileViewModel>? Files { get; set; }
    }
}
