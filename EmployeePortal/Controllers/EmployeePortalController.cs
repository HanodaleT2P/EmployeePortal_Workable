using Microsoft.AspNetCore.Mvc;
using EmployeePortal.Models;
using EmployeePortal.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace EmployeePortal.Controllers
{
    public class EmployeePortalController : Controller
    {
        private readonly EmployeeADOService _repo;

        public EmployeePortalController(EmployeeADOService repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var employees = _repo.GetAll();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _repo.GetDepartments();
            var viewModel = new EmployeeCreateViewModel
            {
                Employee = new Employees(),
                Department = departments.Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                }).ToList()
            };
            return View(viewModel);
        }



        // POST: receive form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeCreateViewModel viewModel)
        {
            // Validate Department selection (0 means none selected)
            if (viewModel.Employee.DepartmentId == 0)
            {
                ModelState.AddModelError("Employee.DepartmentId", "Please select a department.");
            }

            // Check email duplicate
            if (_repo.IsEmailDuplicate(viewModel.Employee.Email))
            {
                ModelState.AddModelError("Employee.Email", "Email already exists.");
            }
            // Re-populate Departments for dropdown before returning view
            var departments = _repo.GetDepartments();
            viewModel.Department = departments.Select(d => new SelectListItem
            {
                Value = d.DepartmentId.ToString(),
                Text = d.Name
            }).ToList();
            if (!ModelState.IsValid)
            {
               

                return View(viewModel);
            }

            // Save employee
            _repo.Add(viewModel.Employee);

            return RedirectToAction("Index");
        }




        public IActionResult Edit(int id)
        {
            var employee = _repo.GetById(id);
            if (employee == null)
                return NotFound();

            var departments = _repo.GetDepartments();
            var viewModel = new EmployeeCreateViewModel
            {
                Employee = employee,
                Department= departments.Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                }).ToList()
            };

            // Specify view name explicitly to use Create.cshtml
            return View("Create", viewModel);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var departments = _repo.GetDepartments();
                viewModel.Department = departments.Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                }).ToList();

                // Specify view name explicitly to use Create.cshtml
                return View("Create", viewModel);
            }

            _repo.Update(viewModel.Employee);
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id) => View(_repo.GetById(id));



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