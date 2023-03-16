using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace PBL.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float? Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public string? File { get; set; } // Add this property

        public List<Assignment>? Assignments { get; set; }
        public List<Comment>? Comments { get; set; }
    }

    public class Assignment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float? Grade { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public List<Comment>? Comments { get; set; }
    }

    public class ProjectDetailsViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public float? ProjectGrade { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public List<AssignmentViewModel>? Assignments { get; set; }
        public List<CommentViewModel>? Comments { get; set; }
    }

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
        public int ProjectId {get; set; }
        public List<CommentViewModel>? Comments { get; set; }
    }


    public class Comment
    {
        public int Id { get; set; }
        public string PostedBy { get; set; }
        public string Text { get; set; }
        public DateTime PostedOn { get; set; }
        public int? AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }
    }



    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }


    public class AssignmentCreateViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }

        public int ProjectId { get; set; }
    }

    public class FileUpload
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }
    }



}
