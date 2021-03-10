﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TM_EmployeeManagement.Models
{
    public interface IEmployeeRepository
    {
        IEnumerable<EmployeeDepartmentModel> GetEmployees();
        object GetEmployeeByFilter(int id, string department, string firstName, string lastName);
        Employee AddEmployee(EmployeeDepartmentModel employee);
        Employee UpdateEmployee(Employee employee);
        Employee DeleteEmployee(int id);
    }
}