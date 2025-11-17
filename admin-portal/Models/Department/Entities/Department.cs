namespace admin_portal.Models.Entities
{
    public class Department
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        // Navigation property - one department has many employees
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
