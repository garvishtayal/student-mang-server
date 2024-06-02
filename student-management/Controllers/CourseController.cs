using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using student_management.Models;
using student_management.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace student_management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet, Authorize(Roles = "admin,teacher,student")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            var courses = await _courseService.GetCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("{id}"), Authorize(Roles = "admin,teacher,student")]
        public async Task<ActionResult<Course>> GetCourse(Guid id)
        {
            var course = await _courseService.GetCourseAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult> AddCourse(Course course)
        {
            await _courseService.AddCourseAsync(course);
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteCourse(Guid id)
        {
            var success = await _courseService.DeleteCourseAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("assign"), Authorize(Roles = "admin,teacher")]
        public async Task<ActionResult> AssignCourse([FromBody] AssignModel assignModel)
        {
            if (assignModel == null || string.IsNullOrEmpty(assignModel.StudentId) || assignModel.CourseId == null || !assignModel.CourseId.Any())
            {
                return BadRequest("Invalid assignment data.");
            }

            var success = await _courseService.AssignCoursesToStudentAsync(assignModel.StudentId, assignModel.CourseId);

            if (!success)
            {
                return NotFound("One or more courses or the student were not found.");
            }

            return Ok(new { message = "Courses assigned successfully." });

        }
        [HttpPost("assignedCourses")]
        public async Task<ActionResult> GetAssignedCourses([FromBody] GetCoursesRequest model)
        {
            var result = await _courseService.AssignedCourses(model.Email);
            if (result.Succeeded)
            {
                return Ok(result.Courses);
            }

            return BadRequest(result.Message);

        }

    }
}


public class AssignModel
{
    public string StudentId { get; set; }
    public List<string> CourseId { get; set; }
}
public class GetCoursesRequest
{
    public string Email { get; set; }
}