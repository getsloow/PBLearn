using System.ComponentModel.DataAnnotations;

namespace PBL.Models
{
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
}
