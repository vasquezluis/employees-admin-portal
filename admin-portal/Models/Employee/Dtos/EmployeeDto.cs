namespace AdminPortal.Models
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public decimal Salary { get; set; }

        public Guid? DepartmentId { get; set; }
        public DepartmentDto? Department { get; set; }
    }
}
