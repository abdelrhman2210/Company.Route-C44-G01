using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route_C44_G01.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepo _departmentRepository;

        public DepartmentController(IDepartmentRepo departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]  // GET: /Department/Index 
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();

            return View(departments);
        }
    }
}
