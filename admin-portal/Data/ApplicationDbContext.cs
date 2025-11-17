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
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configure relationships
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department) // Employee has one Department
                .WithMany(d => d.Employees) // Department has many Employees
                .HasForeignKey(e => e.DepartmentId) // The relationship is defined by DepartmentId
                .OnDelete(DeleteBehavior.SetNull); // when department is deleted, set DepartmentId to null
        }
    }
}
