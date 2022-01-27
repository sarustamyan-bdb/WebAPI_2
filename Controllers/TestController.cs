using WebAPI_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.IO;
using CsvHelper;
using System.Globalization;


namespace WebAPI_2.Controllers
{
    
    public class TestController : ApiController
    {
        // GET: Test
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("{Test, Success}");
        }
    }
}