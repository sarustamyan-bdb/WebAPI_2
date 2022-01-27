using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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