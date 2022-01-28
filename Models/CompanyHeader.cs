/*
=================================================================================================================================================================
PROJECT: APIWEB_1 
AUTHOR: MARK ABRAMS
DATE:   JAN 26, 2022
PURPOSE: MODEL FOR WEB API COMPANY HEADER 
=================================================================================================================================================================
*/

using System.ComponentModel.DataAnnotations;

namespace WebAPI_2.Models
{
    public class CompanyHeader
    {
        [Key]
        public string CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyDescription { get; set; }
        public string EmployeeCount { get; set; }
    }

}