/*
=================================================================================================================================================================
PROJECT: APIWEB_1 
AUTHOR: MARK ABRAMS
DATE:   JAN 26, 2022
PURPOSE: CONTROLLER FOR WEB API COMPANY 
=================================================================================================================================================================
*/

using WebAPI_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Data;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace WebAPI_2.Controllers
{
    public class CompanyController : ApiController
    {

        // NOTES: IN ORDER TO KEEP THE PROJECT ARCHITECTURE AS SUCCINCT AS POSSIBLE THE DATA STORE WILL BE A LOCAL DATA TABLE RATHER THAN SQL DB

        DataTable DataStore = new DataTable();
        List<Company> CompanyList = new List<Company>();

        //========================================================================================================================================================
        [Route("~/api/Company/DataStore")]
        [HttpPost]
        public IHttpActionResult LoadDataStore()
        {

            List<Employee> EmployeeList = new List<Employee>();
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            String file = System.Web.Hosting.HostingEnvironment.MapPath(@"/App_Data/Data.csv");
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))

            {
                DataStore.Clear();

                using (var dr = new CsvDataReader(csv))
                {
                    DataStore.Load(dr);
                }

                DataStore.TableName = "CompanyEmployee";
            }

            return Ok("{Data Loaded}");

        }

        //========================================================================================================================================================
        private List<Company> GetCompanyList(DataTable dt)
        {
            LoadDataStore();

            CompanyList = (from DataRow dr in dt.Rows
                           select new Company()
                           {
                               CompanyId = dr["CompanyId"].ToString(),
                               CompanyCode = dr["CompanyCode"].ToString(),
                               CompanyDescription = dr["CompanyDescription"].ToString(),
                               EmployeeCount = dt.AsEnumerable().Where(x => x.Field<string>("CompanyId") == dr["CompanyId"].ToString()).Count().ToString()
                           }).ToList();
            return (CompanyList);
        }

        [Route("~/api/Company/Companies")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(GetCompanyList(DataStore));
        }

        //========================================================================================================================================================
        [Route("~/api/Company/Companies/id")]
        [HttpGet]
        public IHttpActionResult GetCompany(string id)
        {
            List<Company> CompanyList = GetCompanyList(DataStore);
 
            string expression = "CompanyId =" + id;
            DataRow[] selectedRows = DataStore.Select(expression);
            CompanyList = (from DataRow dr in selectedRows
                            select new Company()
                            {
                                CompanyId = dr["CompanyId"].ToString(),
                                CompanyCode = dr["CompanyCode"].ToString(),
                                CompanyDescription = dr["CompanyDescription"].ToString(),
                                EmployeeCount = DataStore.AsEnumerable().Where(x => x.Field<string>("CompanyId") == dr["CompanyId"].ToString()).Count().ToString()
                            }).ToList();

            return Ok(CompanyList);
        }


    }
}