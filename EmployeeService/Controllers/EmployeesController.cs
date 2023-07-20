using EmployeeService.Data;
using EmployeeService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var employees = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (employees == null)
            {
                return Problem(detail: "Employee with id " + id + " is not found.", statusCode: 404);
            }
            return Ok(_context.Employees);
        }

        [HttpGet("{gender}")]
        public IActionResult GetByGender(string? gender = "All")
        {
            switch (gender.ToLower())
            {
                case "all":
                    return Ok(_context.Employees);
                case "male":
                    return Ok(_context.Employees.Where(e => e.Gender.ToLower() == "male"));
                case "female":
                    return Ok(_context.Employees.Where(e => e.Gender.ToLower() == "female"));
                default:
                    return Problem(detail: "Employee with gender " + gender + " is not found.", statusCode: 404);
            }
        }

        [HttpPost]
        public IActionResult Post(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();

            return CreatedAtAction("GetAll", new {id = employee.Id}, employee);
        }

        [HttpPut]
        public IActionResult Put(int? id, Employee employee)
        {
            var entity = _context.Employees.FirstOrDefault(e => e.Id == employee.Id);
            if(entity == null)
                return Problem(detail: "Employee with id " + id + " is not found.", statusCode: 404);

            entity.FirstName = employee.FirstName;
            entity.LastName = employee.LastName;
            entity.Gender = employee.Gender;
            entity.Salary = employee.Salary;

            _context.SaveChanges();

            return Ok(entity);
        }

        [HttpDelete]
        public IActionResult Delete(int? id, Employee employee)
        {
            var entity = _context.Employees.FirstOrDefault();
            if (entity == null)
                return Problem(detail: "Employee with id " + id + " is not found.", statusCode: 404);

            _context.Employees.Remove(entity);
            _context.SaveChanges();

            return Ok(entity);
        }
    }
}
