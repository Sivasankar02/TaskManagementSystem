using System.ComponentModel.DataAnnotations;

namespace TaskMangementSystem.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string? Description { get; set; }
        [Required]
        [Display(Name = "DueDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }
        public bool IsComplete { get; set; }
        public string? UserId { get; set; }
    }

}
