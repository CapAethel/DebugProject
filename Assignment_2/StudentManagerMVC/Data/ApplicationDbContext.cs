using Microsoft.EntityFrameworkCore;
using StudentManagerMVC.Models;

namespace StudentManagerMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}
