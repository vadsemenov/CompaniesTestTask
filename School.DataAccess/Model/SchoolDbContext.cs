using Microsoft.EntityFrameworkCore;

namespace School.DataAccess.Model
{
    public class SchoolDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Assessment> Assessments { get; set; }

        public SchoolDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=School.db");
        }
    }
}