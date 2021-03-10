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
        public object Filter(int id,string department,string firstName,string lastName)
        {
            return employeeRepository.GetEmployeeByFilter(id,department,firstName,lastName);
        }

        // POST: api/Employees/GetEmployeeById
        [HttpPost]
        [Route("[action]")]
        [Route("api/Employees/CreateEmployee")]
        public object CreateEmployee(EmployeeDepartmentModel employee)
        {
            employeeRepository.AddEmployee(employee);
            return Ok();
        }
    }
}
