using Microsoft.AspNetCore.Mvc;
using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.BLL.Repositories;
using Company.Route_C44_G01.DAL.Models;
using Company.Route_C44_G01.PL.DTOS;

namespace Company.Route_C44_G01.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepo _employeeRepository;

        public EmployeeController(IEmployeeRepo employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]  // GET: /Department/Index 
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();
            // Dictionary : 3 Property
            // 1.ViewData : Transfer Extra Information From Controller (Action) To View
            //ViewData["Message"] = "Hellooooooooooooooooooo";

            // 2.ViewBag : Transfer Extra Information From Controller (Action) To View
            //ViewBag.Message = "Hellooooooooooooooooooo from ViewBag";

            // 3.TempData : Transfer Extra Information From One Request To Another Request

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEmployeeDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var employee = new Employee()
                {
                    Address = model.Address,
                    Age = model.Age,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    HiringDate = model.HiringDate,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue) return BadRequest("Invalid Id"); // 400
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(); // 404

            return View(viewName, employee);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (!id.HasValue) return BadRequest(); // 400
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(); // 404
            var employeeDTO = new CreateEmployeeDTO()
            {
                Address = employee.Address,
                Age = employee.Age,
                Email = employee.Email,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                HiringDate = employee.HiringDate,
                Phone = employee.Phone,
                Salary = employee.Salary,
                Name = employee.Name,
                CreateAt = employee.CreateAt
            };
            return View(employeeDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromRoute] int id , CreateEmployeeDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var employee = new Employee()
                {
                    Id = id,
                    Address = model.Address,
                    Age = model.Age,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    HiringDate = model.HiringDate,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                var count = _employeeRepository.Update(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (!id.HasValue) return BadRequest(); // 400
            //var department = _departmentRepository.Get(id.Value);
            //if (department is null) return NotFound(); // 404

            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Employee model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var count = _employeeRepository.Delete(model);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    }
}
