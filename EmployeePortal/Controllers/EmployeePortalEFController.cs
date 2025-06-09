using EmployeePortal.Models;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeePortal.Interfaces;
using EmployeePortal.Models.EmployeePortalEF;

namespace EmployeePortal.Controllers
{
    public class EmployeePortalEFController : Controller
    {
        private readonly IEmployeeEFService _repo;

        public EmployeePortalEFController(IEmployeeEFService repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _repo.GetAllAsync();
            return View(employees);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _repo.GetDepartmentsAsync();
            var model = new EmployeeEFCreateViewModel
            {
                Employee = new Employee
                {
                    HireDate = DateTime.Today
                },
                Department = departments.Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeEFCreateViewModel model)
        {
            if (model.Employee.DepartmentId == 0)
            {
                ModelState.AddModelError("Employee.DepartmentId", "Please select a department.");
            }

            // Check email duplicate
            if (_repo.IsEmailDuplicate(model.Employee.Email))
            {
                ModelState.AddModelError("Employee.Email", "Email already exists.");
            }
           
            model.Department = (await _repo.GetDepartmentsAsync()).Select(d => new SelectListItem
            {
                Value = d.DepartmentId.ToString(),
                Text = d.Name
            }).ToList();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            await _repo.AddAsync(model.Employee);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _repo.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            var departments = await _repo.GetDepartmentsAsync();
            var model = new EmployeeEFCreateViewModel
            {
                Employee = employee,
                Department = departments.Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                }).ToList()
            };

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeEFCreateViewModel model)
        {
            if (id != model.Employee.EmployeeId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.Department = (await _repo.GetDepartmentsAsync()).Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                }).ToList();
                return View("Create", model);
            }

            await _repo.UpdateAsync(model.Employee);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var emp = await _repo.GetByIdAsync(id);
            if (emp == null)
                return NotFound();

            return View(emp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var emp = await _repo.GetByIdAsync(id);
            if (emp == null)
                return NotFound();

            return View(emp);
        }

    }
}
