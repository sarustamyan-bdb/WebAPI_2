/*
=================================================================================================================================================================
PROJECT: APIWEB_1 
AUTHOR: MARK ABRAMS
DATE:   JAN 26, 2022
PURPOSE: MODEL FOR WEB API EMPLOYEE HEADER 
=================================================================================================================================================================
*/

using System.ComponentModel.DataAnnotations;

namespace WebAPI_2.Models
{
    public class EmployeeHeader
    {
        [Key]
        public string EmployeeNumber { get; set; }
        public string EmployeeFullName { get; set; }
    }

}