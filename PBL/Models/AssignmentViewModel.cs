namespace PBL.Models
{
    public class AssignmentViewModel
    {
        public int AssignmentId { get; set; }
        public string AssignmentName { get; set; }
        public string AssignmentDescription { get; set; }
        public float? AssignmentGrade { get; set; }
        public DateTime AssignmentDueDate { get; set; }
        public bool AssignmentIsCompleted { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public int ProjectId { get; set; }
        public List<CommentViewModel>? Comments { get; set; }
        public List<FileViewModel>? Files { get; set; }
    }
}
