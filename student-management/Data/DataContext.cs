using Microsoft.EntityFrameworkCore;
using student_management.Models;
using System.Collections.Generic;
    
namespace test_server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Management> Managements { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Student_Course> Student_Courses { get; set; }
    }
}