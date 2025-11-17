using admin_portal.Data;
using AdminPortal.Models;
using Microsoft.AspNetCore.Mvc;
using admin_portal.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Controllers
{
    // localhost:<port>/api/departments
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public DepartmentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Get all departments with their employees
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await dbContext.Departments
                .Include(d => d.Employees) // Include related employees
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Employees = d.Employees.Select(e => new EmployeeDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Email = e.Email,
                        Phone = e.Phone,
                        Salary = e.Salary,
                        DepartmentId = e.DepartmentId

                    }).ToList()
                })
                .ToListAsync();

            return Ok(departments);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            var department = await dbContext.Departments
                .Include(d => d.Employees)
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Employees = d.Employees.Select(e => new EmployeeDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Email = e.Email,
                        Phone = e.Phone,
                        Salary = e.Salary,
                        DepartmentId = e.DepartmentId

                    }).ToList()
                })
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment(AddDepartmentDto addDepartmentDto)
        {
            var departmentEntity = new Department()
            {
                Name = addDepartmentDto.Name,
                Description = addDepartmentDto.Description,
            };

            await dbContext.Departments.AddAsync(departmentEntity);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartmentById), new { Id = departmentEntity.Id }, departmentEntity);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDepartment(Guid id, UpdateDepartmentDto updateDepartmentDto)
        {
            var department = await dbContext.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            department.Name = updateDepartmentDto.Name ?? department.Name;
            department.Description = updateDepartmentDto.Description ?? department.Description;

            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            var department = await dbContext.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            dbContext.Departments.Remove(department);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
