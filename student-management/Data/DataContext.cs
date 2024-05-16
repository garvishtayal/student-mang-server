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

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}