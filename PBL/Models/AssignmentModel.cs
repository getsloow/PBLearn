namespace PBL.Models
{
    public class AssignmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float? Grade { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsTurnedIn { get; set; }
        public DateTime? TurnedInAt { get; set; }
        public int ProjectId { get; set; }
        public string Discriminator { get; set; }
        public ProjectModel Project { get; set; }
        public List<CommentModel>? Comments { get; set; }
        public List<FileModel>? Files { get; set; }
    }
}
