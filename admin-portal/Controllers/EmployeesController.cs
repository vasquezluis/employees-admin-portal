using admin_portal.Data;
using admin_portal.Models.Entities;
using AdminPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Controllers
{
    // localhost:<port>/api/employees
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly string endpointName = "Employees";

        // inject DbContext that comes from Program
        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // read all employees
        // IActionResult is a type that represents the result of an action method in an ASP.NET Core MVC application.
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await dbContext.Employees.ToListAsync();

            return Ok(employees);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var employee = await dbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeDto addEmployeeDto)
        {
            var employeeEntity = new Employee()
            {
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Salary = addEmployeeDto.Salary,
            };

            await dbContext.Employees.AddAsync(employeeEntity);
            await dbContext.SaveChangesAsync();

            // CreatedAtAction is a method that returns a CreatedAtActionResult object, which represents a response with a status code of 201 Created and a Location header that points to the newly created resource.
            // It needs endpointName and new { Id = employeeEntity.Id } as parameters.
            return CreatedAtAction(nameof(GetEmployeeById), new { Id = employeeEntity.Id }, employeeEntity);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = await dbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = updateEmployeeDto.Name ?? employee.Name;
            employee.Email = updateEmployeeDto.Email ?? employee.Email;
            employee.Phone = updateEmployeeDto.Phone ?? employee.Phone;
            employee.Salary = updateEmployeeDto.Salary ?? employee.Salary;

            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await dbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            dbContext.Employees.Remove(employee);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
