using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TM_EmployeeManagement.Models
{
    public class EmployeeDepartmentModel
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Email is not in valid format!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Department name is required")]
        public string Department { get; set; }
        [Required(ErrorMessage = "Manager name is required")]
        public string Manager { get; set; }
    }
}
