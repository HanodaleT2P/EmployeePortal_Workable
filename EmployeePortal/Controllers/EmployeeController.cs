using Microsoft.AspNetCore.Mvc;
using EmployeePortal.Models;
using EmployeePortal.Services;

namespace EmployeePortal.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index() => View(EmployeeService.GetAll());

        public IActionResult Details(int id) => View(EmployeeService.GetById(id));

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                EmployeeService.Add(employee);
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        public IActionResult Edit(int id) => View(EmployeeService.GetById(id));

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                EmployeeService.Update(employee);
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        public IActionResult Delete(int id)
        {
            EmployeeService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}