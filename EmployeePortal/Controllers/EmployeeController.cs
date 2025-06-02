using Microsoft.AspNetCore.Mvc;
using EmployeePortal.Models;
using EmployeePortal.Services;

namespace EmployeePortal.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _repo;

        public EmployeeController(EmployeeService repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var employees = _repo.GetAllEmployees();
            return View(employees);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            _repo.InsertEmployee(emp);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id) => View(_repo.GetEmployeeById(id));
        [HttpPost]
        public IActionResult Update(Employee emp)
        {
            _repo.UpdateEmployee(emp);
            return RedirectToAction("Index");
        }

      
        public IActionResult Delete(int id)
        {
            _repo.DeleteEmployee(id);
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id) => View(_repo.GetEmployeeById(id));



        //public IActionResult Create() => View();

        //[HttpPost]
        //public IActionResult Create(Employee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        EmployeeService.Add(employee);
        //        return RedirectToAction("Index");
        //    }
        //    return View(employee);
        //}

        //public IActionResult Edit(int id) => View(EmployeeService.GetById(id));

        //[HttpPost]
        //public IActionResult Edit(Employee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        EmployeeService.Update(employee);
        //        return RedirectToAction("Index");
        //    }
        //    return View(employee);
        //}

        //public IActionResult Delete(int id)
        //{
        //    EmployeeService.Delete(id);
        //    return RedirectToAction("Index");
        //}
    }
}