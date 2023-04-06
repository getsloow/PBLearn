namespace PBL.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string PostedBy { get; set; }
        public string Text { get; set; }
        public DateTime PostedOn { get; set; }
        public int? AssignmentId { get; set; }
        public AssignmentModel? Assignment { get; set; }
        public int? ProjectId { get; set; }
        public ProjectModel? Project { get; set; }
    }
}
