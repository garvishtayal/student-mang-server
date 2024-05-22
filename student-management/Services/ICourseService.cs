using student_management.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace student_management.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetCoursesAsync();
        Task<Course?> GetCourseAsync(Guid id);
        Task AddCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(Guid id);
    }
}
