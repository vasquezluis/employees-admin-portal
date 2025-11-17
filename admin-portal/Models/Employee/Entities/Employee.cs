namespace admin_portal.Models.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public decimal Salary { get; set; }

        // Foreign key for Department
        public Guid? DepartmentId { get; set; }

        // Navigation property - many employees belong to one department
        public Department? Department { get; set; }
    }
}
