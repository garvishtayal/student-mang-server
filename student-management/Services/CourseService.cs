using student_management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_server.Data;
using Microsoft.EntityFrameworkCore;

namespace student_management.Services
{
    public class CourseService : ICourseService
    {
        private readonly DataContext _context;

        public CourseService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetCourseAsync(Guid id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task AddCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AssignCoursesToStudentAsync(string studentId, List<string> courseIds)
        {
            var student = await _context.Students.FindAsync(Guid.Parse(studentId));
            if (student == null)
            {
                return false;
            }

            var courses = await _context.Courses.Where(c => courseIds.Contains(c.Id.ToString())).ToListAsync();
            if (courses.Count != courseIds.Count)
            {
                return false;
            }

            // Remove existing course assignments for the student
            var existingAssignments = _context.Student_Courses.Where(sc => sc.StudentId == Guid.Parse(studentId));
            _context.Student_Courses.RemoveRange(existingAssignments);


            foreach (var courseId in courseIds)
            {
                var studentCourse = new Student_Course
                {
                    Id = Guid.NewGuid(),
                    StudentId = Guid.Parse(studentId),
                    CourseId = Guid.Parse(courseId)
                };
                _context.Student_Courses.Add(studentCourse);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AssignedCourses> AssignedCourses(string studentEmail)
        {
            var courses = await _context.Courses.ToListAsync();

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == studentEmail);

            // Handle the case where the student is not found
            if (student == null)
            {
                return new AssignedCourses { Succeeded = false, Courses = null, Message = "Student not found" };
            }

            // Retrieve the list of course IDs associated with the student
            var assignedCourseIds = await _context.Student_Courses
                                                  .Where(sc => sc.StudentId == student.Id)
                                                  .Select(sc => sc.CourseId)
                                                  .ToListAsync();

            // Optionally, retrieve the course details based on the IDs (if needed)
            var assignedCourses = await _context.Courses
                                                .Where(c => assignedCourseIds.Contains(c.Id))
                                                .ToListAsync();


            return new AssignedCourses { Succeeded = true, Courses = assignedCourses, Message = "courses fetched successfully"};

        }
    }
}


public class AssignedCourses
{
    public bool Succeeded { get; set; }
    public List<Course> Courses { get; set; }
    public string Message { get; set; }
}