using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_2.Models
{
    public class Company
    {
        [Key]
        public string CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyDescription { get; set; }
        public string EmployeeCount { get; set; }
    }


}