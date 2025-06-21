using System.ComponentModel.DataAnnotations;

namespace StudentCourseResults.Models
{
    public class StudentResult
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Student Name")]
        public required string  StudentName { get; set; }

        [Required]
        [Display(Name = "Course Title")]
        public required string CourseTitle { get; set; }

        [Range(0, 100)]
        [Display(Name = "Total Marks")]
        public required int TotalMarks { get; set; }

        public  ResultStatusName Status { get; set; }
    }
    public enum ResultStatusName
    {
        NeedsImprovement,
        Good,
        Excellent
    }
}
