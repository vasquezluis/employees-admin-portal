using admin_portal.Controllers;
using admin_portal.Data;
using admin_portal.Models.Entities;
using AdminPortal.Controllers;
using AdminPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace admin_portal.Tests
{
    public class EmployeesControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployeesController _controller;

        // This constructor runs before each test
        public EmployeesControllerTests()
        {
            // Create an in-memory database that exists only for this test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new EmployeesController(_context);
        }

        // This method runs after each test to clean up
        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllEmployees_WhenNoEmployees_ReturnsEmptyList()
        {
            // Arrange: The database is already empty from constructor

            // Act: Call the method we're testing
            var result = await _controller.GetAllEmployees();

            // Assert: Check the results
            var okResult = Assert.IsType<OkObjectResult>(result);
            var employees = Assert.IsAssignableFrom<List<Employee>>(okResult.Value);
            Assert.Empty(employees);
        }

        [Fact]
        public async Task GetAllEmployees_WhenEmployeesExist_ReturnsAllEmployees()
        {
            // Arrange: Add test data to our in-memory database
            var testEmployees = new List<Employee>
            {
                new Employee { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@test.com", Phone = "123-456-7890", Salary = 50000 },
                new Employee { Id = Guid.NewGuid(), Name = "Jane Smith", Email = "jane@test.com", Phone = "098-765-4321", Salary = 60000 }
            };

            _context.Employees.AddRange(testEmployees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var employees = Assert.IsAssignableFrom<List<Employee>>(okResult.Value);
            Assert.Equal(2, employees.Count);
        }

        [Fact]
        public async Task GetEmployeeById_WhenEmployeeExists_ReturnsEmployee()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var testEmployee = new Employee
            {
                Id = employeeId,
                Name = "Test Employee",
                Email = "test@test.com",
                Salary = 45000
            };

            _context.Employees.Add(testEmployee);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetEmployeeById(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var employee = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal(employeeId, employee.Id);
            Assert.Equal("Test Employee", employee.Name);
        }

        [Fact]
        public async Task GetEmployeeById_WhenEmployeeDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _controller.GetEmployeeById(nonExistentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddEmployee_WithValidData_CreatesEmployeeAndReturnsCreatedResult()
        {
            // Arrange
            var addEmployeeDto = new AddEmployeeDto
            {
                Name = "New Employee",
                Email = "new@test.com",
                Phone = "555-0123",
                Salary = 55000
            };

            // Act
            var result = await _controller.AddEmployee(addEmployeeDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var employee = Assert.IsType<Employee>(createdResult.Value);

            Assert.Equal("New Employee", employee.Name);
            Assert.Equal("new@test.com", employee.Email);
            Assert.NotEqual(Guid.Empty, employee.Id); // Should have generated an ID

            // Verify it was actually saved to the database
            var savedEmployee = await _context.Employees.FindAsync(employee.Id);
            Assert.NotNull(savedEmployee);
        }

        [Fact]
        public async Task UpdateEmployee_WhenEmployeeExists_UpdatesEmployeeAndReturnsNoContent()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var originalEmployee = new Employee
            {
                Id = employeeId,
                Name = "Original Name",
                Email = "original@test.com",
                Phone = "111-111-1111",
                Salary = 40000
            };

            _context.Employees.Add(originalEmployee);
            await _context.SaveChangesAsync();

            var updateDto = new UpdateEmployeeDto
            {
                Name = "Updated Name",
                Email = "updated@test.com",
                Salary = 50000
                // Note: Phone is null, so it should remain unchanged
            };

            // Act
            var result = await _controller.UpdateEmployee(employeeId, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify the changes were saved
            var updatedEmployee = await _context.Employees.FindAsync(employeeId);
            Assert.NotNull(updatedEmployee);
            Assert.Equal("Updated Name", updatedEmployee.Name);
            Assert.Equal("updated@test.com", updatedEmployee.Email);
            Assert.Equal("111-111-1111", updatedEmployee.Phone); // Should remain unchanged
            Assert.Equal(50000, updatedEmployee.Salary);
        }

        [Fact]
        public async Task UpdateEmployee_WhenEmployeeDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var updateDto = new UpdateEmployeeDto { Name = "Test" };

            // Act
            var result = await _controller.UpdateEmployee(nonExistentId, updateDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_WhenEmployeeExists_DeletesEmployeeAndReturnsNoContent()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = new Employee
            {
                Id = employeeId,
                Name = "To Delete",
                Email = "delete@test.com",
                Salary = 30000
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteEmployee(employeeId);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify it was actually deleted
            var deletedEmployee = await _context.Employees.FindAsync(employeeId);
            Assert.Null(deletedEmployee);
        }

        [Fact]
        public async Task DeleteEmployee_WhenEmployeeDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _controller.DeleteEmployee(nonExistentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
