using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TM_EmployeeManagement.Models;

namespace TM_EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        // GET: api/Employees/GetEmployee
        [HttpGet]
        [Route("[action]")]
        [Route("api/Employees/GetEmployees")]
        public IEnumerable<EmployeeDepartmentModel> GetEmployees()
        {
            var employees = employeeRepository.GetEmployees().ToList();
            return employees;
        }

        // GET: api/Employees/GetEmployeeById
        [HttpGet]
        [Route("[action]")]
        [Route("api/Employees/Filter")]
        public object Filter(int? id, string department, string firstName, string lastName)
        {
            return employeeRepository.GetEmployeeByFilter(id, department, firstName, lastName);
        }

        // POST: api/Employees/GetEmployeeById
        [HttpPost]
        [Route("[action]")]
        [Route("api/Employees/CreateEmployee")]
        public object CreateEmployee(EmployeeDepartmentModel employee)
        {
            try
            {
                if (employee == null)
                    return BadRequest();
                if (ModelState.IsValid)
                {
                    var createdEmployee = employeeRepository.AddEmployee(employee);
                }

                return StatusCode(StatusCodes.Status201Created, "Employee created successfully");
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error creating new employee record");
            }

        }

        // POST: api/Employees/GetEmployeeById
        [HttpPut("{id:int}")]
        [Route("[action]")]
        [Route("api/Employees/ModifyEmployee")]
        public object ModifyEmployee(int? id, EmployeeDepartmentModel employee)
        {
            try
            {
                if (id == 0 && employee.EmployeeId == 0 || id == null)
                    return BadRequest("Employee ID mismatch");

                if (ModelState.IsValid)
                {
                    var employeeToUpdate = employeeRepository.UpdateEmployee(employee);

                    if (employeeToUpdate == null)
                        return NotFound($"Employee with Id = {id} not found");
                }

                return StatusCode(StatusCodes.Status200OK, "Employee modified successfully");
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating data");
            }
        }

        // POST: api/Employees/GetEmployeeById
        [HttpDelete("{id:int}")]
        [Route("[action]")]
        [Route("api/Employees/DeleteEmployee")]
        public object DeleteEmployee(int? id)
        {
            try
            {
                if (id == 0 || id == null)
                {
                    return NotFound($"Employee with Id = {id} not found");
                }

                employeeRepository.DeleteEmployee(id);
                return Ok("employee deleted!");
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting data");
            }
        }

        // POST: api/Employees/GetEmployeeById
        [HttpGet]
        [Route("[action]")]
        [Route("api/Employees/GetEmployeeStatus")]
        public object GetEmployeeStatusById(int? id)
        {
            try
            {
                if (id == 0 || id == null)
                {
                    return employeeRepository.GetEmployeeStatusById(id);
                }
                return StatusCode(StatusCodes.Status400BadRequest, "No employeeId supplied to get employee status");
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Something went wrong when getting status of employee. Kindly try later");
            }
            
        }
    }
}
