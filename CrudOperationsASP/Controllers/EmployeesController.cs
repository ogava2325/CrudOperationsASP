using CrudOperationsASP.Data;
using CrudOperationsASP.Data.NewFolder;
using CrudOperationsASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudOperationsASP.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly DemoDbContext _demoDbContext;

        public EmployeesController(DemoDbContext demoDbContext)
        {
            _demoDbContext = demoDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };

            await _demoDbContext.Employees.AddAsync(employee);
            await _demoDbContext.SaveChangesAsync();
            
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            var employees = await _demoDbContext.Employees.ToListAsync();

            return View(employees); 
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await _demoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var viewModel = new UpdateViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };

                return await Task.Run(()=> View("View",viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateViewModel model)
        {
            var employee = await _demoDbContext.Employees.FindAsync(model.Id);
            
            if(employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.Department = model.Department;
                employee.DateOfBirth = model.DateOfBirth;

                await _demoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateViewModel model)
        {
            var employee = await _demoDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {

                _demoDbContext.Employees.Remove(employee);
                await _demoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
