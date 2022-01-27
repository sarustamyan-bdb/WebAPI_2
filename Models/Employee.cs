using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_2.Models
{
    public class Employee
    {
        [Key]
        public string EmployeeNumber { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeDepartment { get; set; }
        public string HireDate { get; set; }
        public string ManagerEmployeeNumber { get; set; }
    }

}
