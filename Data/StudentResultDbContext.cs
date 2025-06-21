using Microsoft.EntityFrameworkCore;
using StudentCourseResults.Models;

namespace StudentCourseResults.Data
{
    public class StudentResultDbContext : DbContext
    {
        public StudentResultDbContext(DbContextOptions<StudentResultDbContext> options)
            : base(options) { }

        public DbSet<StudentResult> StudentResults { get; set; }
    }
}
