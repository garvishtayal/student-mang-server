namespace student_management.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Source { get; set; }
    }
}
