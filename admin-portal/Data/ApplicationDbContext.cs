using admin_portal.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace admin_portal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Add entities for EF
        public DbSet<Employee> Employees { get; set; }
    }
}
