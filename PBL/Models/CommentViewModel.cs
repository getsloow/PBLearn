namespace PBL.Models
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
