/*
=================================================================================================================================================================
PROJECT: APIWEB_1 
AUTHOR: MARK ABRAMS
DATE:   JAN 26, 2022
PURPOSE: MODEL FOR WEB API EMPLOYEE 
=================================================================================================================================================================
*/

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
