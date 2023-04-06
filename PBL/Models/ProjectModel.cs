namespace PBL.Models
{
      public class ProjectModel
        {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float? Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? UserEmail { get; set; }
        public List<AssignmentModel>? Assignments { get; set; }
        public List<CommentModel>? Comments { get; set; }

        public List<FileModel>? Files { get; set; }
    }
}
