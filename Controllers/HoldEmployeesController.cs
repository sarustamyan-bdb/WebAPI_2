using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAPI_1.Data;
using WebAPI_1.Models;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace WebAPI_1.Controllers
{
    public class EmployeesController : Controller
    {
        private WebAPI_1CSV db = new WebAPI_1CSV();

        DataTable dt = new DataTable();
        List<Employee> EmployeeList = new List<Employee>();

        DataTable DataStore = new DataTable();

        public void LoadDataStore()
        {
            List<Employee> EmployeeList = new List<Employee>();
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            String file = System.Web.Hosting.HostingEnvironment.MapPath(@"/App_Data/Data.csv");
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))


            {
                DataStore.Clear();

                // Do any configuration to `CsvReader` before creating CsvDataReader.
                using (var dr = new CsvDataReader(csv))
                {
                    DataStore.Load(dr);
                }

                DataStore.TableName = "Employees";
            }

        }

        public List<Employee> GetEmployeeList(DataTable dt)
        {

            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                CompanyId = dr["CompanyId"].ToString(),
                                CompanyCode = dr["CompanyCode"].ToString(),
                                CompanyDescription = dr["CompanyDescription"].ToString(),
                                EmployeeNumber = dr["EmployeeNumber"].ToString(),
                                EmployeeFirstName = dr["EmployeeFirstName"].ToString(),
                                EmployeeLastName = dr["EmployeeLastName"].ToString(),
                                EmployeeEmail = dr["EmployeeEmail"].ToString(),
                                EmployeeDepartment = dr["EmployeeDepartment"].ToString(),
                                HireDate = dr["HireDate"].ToString(),
                                ManagerEmployeeNumber = dr["ManagerEmployeeNumber"].ToString()
                            }).ToList();
            return EmployeeList;
        }

        // GET: Employees
        [ActionName("Employees")]
        public ActionResult Index()
        {
            LoadDataStore();
            EmployeeList = GetEmployeeList(DataStore);
            return View(EmployeeList);
        }


        // GET: Employees/Details/5
        [ActionName("Employees/EmployeeNumber")]
        public ActionResult Details(string EmployeeNumber)
        {
            if (EmployeeNumber == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string expression = "EmployeeNumber =" + EmployeeNumber;
            DataRow[] selectedRows = DataStore.Select(expression);
            EmployeeList = (from DataRow dr in selectedRows
                            select new Employee()
                            {
                                CompanyId = dr["CompanyId"].ToString(),
                                CompanyCode = dr["CompanyCode"].ToString(),
                                CompanyDescription = dr["CompanyDescription"].ToString(),
                                EmployeeNumber = dr["EmployeeNumber"].ToString(),
                                EmployeeFirstName = dr["EmployeeFirstName"].ToString(),
                                EmployeeLastName = dr["EmployeeLastName"].ToString(),
                                EmployeeEmail = dr["EmployeeEmail"].ToString(),
                                EmployeeDepartment = dr["EmployeeDepartment"].ToString(),
                                HireDate = dr["HireDate"].ToString(),
                                ManagerEmployeeNumber = dr["ManagerEmployeeNumber"].ToString()
                            }).ToList();


            if (EmployeeList == null)
            {
                return HttpNotFound();
            }
            return View(EmployeeList);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyId,CompanyCode,CompanyDescription,EmployeeNumber,EmployeeFirstName,EmployeeLastName,EmployeeEmail,EmployeeDepartment,HireDate,ManagerEmployeeNumber")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                DataStore.Rows.Add(employee);
               // db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyId,CompanyCode,CompanyDescription,EmployeeNumber,EmployeeFirstName,EmployeeLastName,EmployeeEmail,EmployeeDepartment,HireDate,ManagerEmployeeNumber")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
