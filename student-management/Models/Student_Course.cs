using System.ComponentModel.DataAnnotations.Schema;


namespace student_management.Models
{
    public class Student_Course
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }

        [ForeignKey("StudentId")]
        public Student StudentNavigation { get; set; }

        [ForeignKey("CourseId")]
        public Course CourseNavigation { get; set; }
    }
}