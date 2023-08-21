using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using JwtAuth.Models;
using JwtAuth.Controllers;
using Microsoft.AspNetCore.Authorization;


namespace JwtAut.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private static List<Employee> Employees = new List<Employee>()
        {
            new Employee { Id = 0, Name = "EmployeeA" },
            new Employee { Id = 1, Name = "EmployeeB" },
            new Employee { Id = 2, Name = "EmployeeC" }
        };

        // GET: api/Employee/all

        [HttpGet("all")]
        
        public IActionResult GetAllEmployees()
        {
            return Ok(Employees);
        }

        // GET: api/Employee/byId/1
        [HttpGet("byId/{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            if (id >= 0 && id < Employees.Count)
            {
                return Ok(Employees[id]);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/Employee/create
        [HttpPost("create")]
        public IActionResult CreateEmployee([FromBody] Employee newEmployee)
        {
            Employees.Add(newEmployee);
            return Created("", newEmployee);
        }

        // PUT: api/Employee/update/1
        [HttpPut("update/{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            if (id >= 0 && id < Employees.Count)
            {
                Employees[id] = updatedEmployee;
                return Ok(Employees);
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE: api/Employee/delete/1
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            if (id >= 0 && id < Employees.Count) 
            {
                Employees.RemoveAt(id);
                return Ok(Employees);
            }
            else
            {
                return NotFound();
            }
        }
    }
}