using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TM_EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeManagementContext db;
        private readonly EmailHelper emailHelper;

        public SQLEmployeeRepository(EmployeeManagementContext db, EmailHelper emailHelper)
        {
            this.db = db;
            this.emailHelper = emailHelper;
        }
        public IEnumerable<EmployeeDepartmentModel> GetEmployees()
        {
            var employees = PerformJoin();
            return employees;
        }
        public Employee AddEmployee(EmployeeDepartmentModel employee)
        {
            if (employee != null)
            {
                Employee newEmployee = new Employee
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    EmailId = employee.Email,
                    DepartmentId = db.Departments.FirstOrDefault(d => d.DepartmentName == employee.Department).DepartmentId,
                    ManagerId = db.Employees.FirstOrDefault(e=>e.FirstName == employee.Manager).EmployeeId
                };
                db.Employees.Add(newEmployee);
                db.SaveChanges();
                //SendEmail(newEmployee.EmailId);
                return newEmployee;
            }
            return null;
        }
        public Employee UpdateEmployee(EmployeeDepartmentModel employee)
        {
            Employee modifyEmployee = db.Employees.FirstOrDefault(emp => emp.EmployeeId == employee.EmployeeId);
            modifyEmployee.FirstName = employee.FirstName;
            modifyEmployee.LastName = employee.LastName;
            modifyEmployee.EmailId = employee.Email;
            modifyEmployee.DepartmentId = db.Departments.FirstOrDefault(d => d.DepartmentName == employee.Department).DepartmentId;
            modifyEmployee.ManagerId = db.Employees.FirstOrDefault(e => e.FirstName == employee.Manager).EmployeeId;

            db.Entry(modifyEmployee).State = EntityState.Modified;
            db.SaveChanges();
            return modifyEmployee;
        }
        public Employee DeleteEmployee(int? id)
        {
            var employee = db.Employees.FirstOrDefault(x => x.EmployeeId == id);
            db.Entry(employee).State = EntityState.Deleted;
            db.SaveChanges();
            return employee;
        }
        public object GetEmployeeByFilter(int? id, string department, string firstName, string lastName)
        {
            var employees = PerformJoin();
            if(id > 0)
            {
                return employees.FirstOrDefault(emp => emp.EmployeeId == id);
            }
            else if(department != null)
            {
                employees = employees.Where(e => e.Department == department).ToList();
                return employees;
            }

            else if (firstName != null)
            {
                return employees.Where(emp => emp.FirstName == firstName);
            }

            else if (lastName != null)
            {
                return employees.Where(emp => emp.LastName == lastName);
            }

            return null;

        }

        public IEnumerable<EmployeeDepartmentModel> PerformJoin()
        {
            var employees = from employee in db.Employees.ToList()
                            join department in db.Departments.ToList()
                        on employee.DepartmentId equals department.DepartmentId
                            join manager in db.Employees.ToList()
                            on employee.ManagerId equals manager.EmployeeId
                            into eGroup
                            from e in eGroup.DefaultIfEmpty()
                            orderby employee.FirstName ascending
                            select new EmployeeDepartmentModel
                            {
                                EmployeeId = employee.EmployeeId,
                                FirstName = employee.FirstName,
                                LastName = employee.LastName,
                                Email = employee.EmailId,
                                Department = department.DepartmentName,
                                Manager = e == null ? "No Manager" : e.FirstName
                            };
            return employees;
        }

        public void SendEmail(string emailId)
        {
            var emailModel = new EmailModel(emailId, // To  
                "Email Test", // Subject  
                "Sending Email using Asp.Net Core.", // Message  
                false // IsBodyHTML  
            );
            emailHelper.SendEmail(emailModel);
        }

        public object GetEmployeeStatusById(int? id)
        {
            var managerAndEmployees = db.Employees.ToLookup(pair => pair.ManagerId, pair => pair.EmployeeId);

            foreach (var item in managerAndEmployees)
            {
                if(id == item.Key)
                {
                    if (item.Count() >= 1)
                    {
                        return new { Status = "Manager" };
                    }
                }
            }

            return new { Status = "Associate" };
        }
        
    }
}
