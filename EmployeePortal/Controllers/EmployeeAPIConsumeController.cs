using Microsoft.AspNetCore.Mvc;
using EmployeePortal.Models.EmployeePortalEF;
using EmployeePortal.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure;
using EmployeePortal.Models;


public class EmployeeAPIConsumeController :  Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    public EmployeeAPIConsumeController(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {
        var baseUrl = _configuration["ApiBaseUrl"];
        var response = await _httpClient.GetAsync($"{baseUrl}/GetEmployeeList");
        if (response.IsSuccessStatusCode)
        {
            var employees = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeEFViewModel>>();
            return View(employees);
        }

        return View(new List<EmployeeEFViewModel>()); // Return an empty list if API call fails
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var baseUrl = _configuration["ApiBaseUrl"];

        // Make the API call
        var response = await _httpClient.GetAsync($"{baseUrl}/GetDepartmentList");

        // Check if the response is successful
        if (!response.IsSuccessStatusCode)
        {
            // Handle error (log or display message)
            ModelState.AddModelError(string.Empty, "Unable to load department list.");
            return View(new EmployeeEFCreateViewModel()); // Return empty model
        }

        // Deserialize response content
        var departments = await response.Content.ReadFromJsonAsync<List<Department>>();

        // Prepare the ViewModel
        var viewModel = new EmployeeEFCreateViewModel
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

        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> Create(EmployeeEFCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var baseUrl = _configuration["ApiBaseUrl"];
        var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/AddEmployee", model);

        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, "Server error. Please try again.");
        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var baseUrl = _configuration["ApiBaseUrl"];
        var response = await _httpClient.GetAsync($"{baseUrl}/GetEmployeeById/{id}");

        if (!response.IsSuccessStatusCode)
            return NotFound();

        var employee = await response.Content.ReadFromJsonAsync<EmployeeEFCreateViewModel>();
        return View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EmployeeEFCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var baseUrl = _configuration["ApiBaseUrl"];
        var response = await _httpClient.PutAsJsonAsync($"{baseUrl}/EditEmployee/{id}", model);

        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, "Update failed.");
        return View(model);
    }
    
}
