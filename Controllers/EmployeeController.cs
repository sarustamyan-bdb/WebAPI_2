using WebAPI_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Data;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Diagnostics;

namespace WebAPI_2.Controllers
{
    public class EmployeeController : ApiController
    {

        // NOTES: IN ORDER TO KEEP THE PROJECT ARCHITECTURE AS SUCCINCT AS POSSIBLE THE DATA STORE WILL BE A LOCAL DATA TABLE RATHER THAN SQL DB

        DataTable DataStore = new DataTable();

        List<Employee> EmployeeList = new List<Employee>();

        public bool LoadDataStore()
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

            if (ValidateEmployeeNumbers() == "false")
            { // THERE WERE DUPLICATE EMPLOYEE+COMPANY VALUES IN THE DATA.
              //  HOW THIS SHOULD BE HANDLED HAS NOT BEEN SPECIFIED, SO WE WILL LOG THE ERROR AND MOVE ON.
              //  THIS CAN BE TESTED BY COPYING ONE ROW OF DATA IN THE CSV FILE AND SAVING IT.

                Debug.WriteLine("THERE WERE DUPLICATE EMPLOYEE + COMPANY VALUES IN THE DATA");
                return false;
            }
            else
            {
                return true;
            };
        }

        private List<Employee> GetEmployeeList(DataTable dt)
        {
            LoadDataStore();

            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                EmployeeNumber = dr["EmployeeNumber"].ToString(),
                                EmployeeEmail = dr["EmployeeEmail"].ToString(),
                                EmployeeDepartment = dr["EmployeeDepartment"].ToString(),
                                HireDate = dr["HireDate"].ToString(),
                                ManagerEmployeeNumber = dr["ManagerEmployeeNumber"].ToString()
                            }).ToList();
            return (EmployeeList);
        }

        [Route("~/api/Employee/Employees")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(GetEmployeeList(DataStore));
        }

        [Route("~/api/Employee/Employees/id")]
        [HttpGet]
        public IHttpActionResult GetEmployee(string id)
        {

            if (ValidateEmployeeManager(id) == false)
            {
                //  WIP - THE EMPLOYEE MANAGER ID WAS NOT FOUND.
                //  HOW THIS SHOULD BE HANDLED HAS NOT BEEN SPECIFIED, SO WE WILL LOG THE ERROR AND MOVE ON.
                //  THIS CAN BE TESTED BY CHANGING ONE MANAGER ID TO 'A1' AND TESTING AGAINST THAT EMPLOYEE ID.
                Debug.WriteLine("THE MANAGER FOR THIS EMPLOYEE WAS NOT FOUND");
            }

            List<Employee> EmployeeList = GetEmployeeList(DataStore);

            string expression = "EmployeeNumber ='" + id + "'";
            DataRow[] selectedRows = DataStore.Select(expression);
            EmployeeList = (from DataRow dr in selectedRows
                            select new Employee()
                            {
                                EmployeeNumber = dr["EmployeeNumber"].ToString(),
                                EmployeeEmail = dr["EmployeeEmail"].ToString(),
                                EmployeeDepartment = dr["EmployeeDepartment"].ToString(),
                                HireDate = dr["HireDate"].ToString(),
                                ManagerEmployeeNumber = dr["ManagerEmployeeNumber"].ToString()
                            }).ToList();

            return Ok(EmployeeList);
        }


        [Route("~/api/Companies/{companyId}/Employees/{employeeNumber}")]
        [HttpGet]
        public IHttpActionResult GetCompanyEmployee(string companyId, string employeeNumber)
        {

            if (ValidateEmployeeManager(employeeNumber) == false)
            {
                //  WIP - THE EMPLOYEE MANAGER ID WAS NOT FOUND.
                //  HOW THIS SHOULD BE HANDLED HAS NOT BEEN SPECIFIED, SO WE WILL LOG THE ERROR AND MOVE ON.
                //  THIS CAN BE TESTED BY CHANGING ONE MANAGER ID TO 'A1' AND TESTING AGAINST THAT EMPLOYEE ID.
                Debug.WriteLine("THE MANAGER FOR THIS EMPLOYEE WAS NOT FOUND");
            }

            List<Employee> EmployeeList = GetEmployeeList(DataStore);

            string expression = "EmployeeNumber = '" + employeeNumber + "' and CompanyId = '" + companyId +"'";
            DataRow[] selectedRows = DataStore.Select(expression);
            EmployeeList = (from DataRow dr in selectedRows
                            select new Employee()
                            {
                                EmployeeNumber = dr["EmployeeNumber"].ToString(),
                                EmployeeEmail = dr["EmployeeEmail"].ToString(),
                                EmployeeDepartment = dr["EmployeeDepartment"].ToString(),
                                HireDate = dr["HireDate"].ToString(),
                                ManagerEmployeeNumber = dr["ManagerEmployeeNumber"].ToString()
                            }).ToList();

            return Ok(EmployeeList);
        }


        private String ValidateEmployeeNumbers()
        {
            var distinctValues = DataStore.AsEnumerable()
                                    .Select(row => new
                                    {
                                        EmployeeCompany = row.Field<string>("EmployeeNumber") + row.Field<string>("CompanyId")
                                    })
                                    .Distinct();

            int CountRows = 0;
            foreach (var item in distinctValues)
            {
                CountRows = CountRows + 1;
                Debug.WriteLine(item.EmployeeCompany);
            }
            if (DataStore.Rows.Count == CountRows)
            {
                return "True";
            }
            else
            {
                return "false";
            }
        }


        private bool ValidateEmployeeManager(string EmployeeNumber)
        {

            /*
             =====================================================================
             WORK IN PROGRESS - PROTO CODE              
             =====================================================================
             THIS QUERY CAN BE USED TO VALIDATE THAT THE CURRENLY SELECTED EMPLOYEE 
             HAS A VALID MANAGER ID IN THE DATASTORE. IT WOULD EITHER BE RUN AS
             A SQL STATEMENT AGAINST A SQL SERVER TABLE OR COULD BE TRANSLATED
             INTO A DATATABLE SELECT STATEMENT OR LINQ STATEMENT.
             =====================================================================
             SELECT * FROM (
             SELECT EmployeeNumber, CompanyID, ManagerEmployeeNumber FROM DataStore Where EmployeeNumber = @EmployeeNumber
             ) as a
             WHERE a.ManagerEmployeeNumber in (SELECT EmployeeNumber FROM DataStore)
             ================================================================================================
             */

            return true;

             //  DataRow[] rows = DataStore.Select("SELECT * FROM (SELECT EmployeeNumber, CompanyID, ManagerEmployeeNumber FROM DataStore Where EmployeeNumber = @EmployeeNumber) as a WHERE a.ManagerEmployeeNumber in (SELECT EmployeeNumber FROM DataStore)");
             //  Debug.Write(rows.Length);
             //  if (rows.Length == 1)
             //  {
             //      return true; // "Manager Found"
             //  }
             //  else
             //  {
             //      return false; // "Managere was not found"
             //  }
        }



    }
}