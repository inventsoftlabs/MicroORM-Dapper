using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MicroORMWithDapper;
using Sample.Models;
using Microsoft.Extensions.Configuration;

namespace Sample.Controllers
{
    public class HomeController : Controller
    {
        private EmployeeModel employeeModel;
        private AddressInformationModel addressModel;

        public HomeController(IConfiguration config)
        {
            this.employeeModel = new EmployeeModel(config);
            this.addressModel = new AddressInformationModel(config);
        }

        public IActionResult Index()
        {
            return View("~/Views/Home/OrmHome.cshtml");
        }

        public IActionResult EmployeeDetail(long id)
        {
            // Get all records from employee table.
            var employee = this.employeeModel.FindById(id);
            var address = this.addressModel.Find(item => item.EmployeeId == id);
            employee.Address = address;

            return View("~/Views/Home/EmployeeDetail.cshtml", employee);
        }

        public IActionResult Employee()
        {
            // Get all records from employee table.
            var employeeList = this.employeeModel.GetAll();
            var addressList = this.addressModel.GetAll();

            foreach (var item in employeeList)
            {
                item.Address = addressList.Where(address => address.EmployeeId == item.Id);
            }

            return View("~/Views/Home/EmployeeDetails.cshtml", employeeList);
        }

        public IActionResult EmployeeDetails(string ids)
        {
            long[] idList = ids.Split(',').Select(item => Convert.ToInt64(item)).ToArray();
            var employeeList = this.employeeModel.FindByIds(idList);
            var addressList = this.addressModel.Find(item => item.Active == true && idList.Contains(item.Id));

            foreach (var item in employeeList)
            {
                item.Address = addressList.Where(address => address.EmployeeId == item.Id);
            }

            return View("~/Views/Home/EmployeeDetails.cshtml", employeeList);
        }

        public IActionResult StoredProcedure(long id)
        {
            // using stored Procedure and mapper.
            var empSPDetails = this.employeeModel.ExecStoredProcedureQueryMultiple<Employee, MultiModelMapper>("GetEmployeeInformation", new { EmployeeId = id });

            return View("~/Views/Home/EmployeeDetail.cshtml", empSPDetails);
        }

        public IActionResult Views(long id)
        {
            // using view.
            var empDetails = this.employeeModel.ExecViewResult<Employee>("vw_Employee", item => item.Id == id)?.FirstOrDefault();
            var address = this.addressModel.Find(item => item.EmployeeId == id);
            empDetails.Address = address;

            return View("~/Views/Home/EmployeeDetail.cshtml", empDetails);
        }

        public IActionResult EmployeePaged()
        {
            // using paged view result.
            var sortExpression = new List<SortExpression<Employee>>();
            sortExpression.Add(new SortExpression<Employee>(item => item.Name, SortDirection.Descending));
            var empDetails = this.employeeModel.ExecPagedViewResult<Employee>("vw_Employee", item => item.Id > 0, sortExpression.ToArray(), new List<string>() { "Name" }, string.Empty);
            var employeeList = empDetails.ToList();
            var addressList = this.addressModel.Find(item => item.Active == true && employeeList.Select(emp => emp.Id).ToArray().Contains(item.Id));

            foreach (var item in employeeList)
            {
                item.Address = addressList.Where(address => address.EmployeeId == item.Id);
            }

            return View("~/Views/Home/EmployeeDetails.cshtml", empDetails.ToList());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
