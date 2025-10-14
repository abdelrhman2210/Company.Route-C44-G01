using AutoMapper;
using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.BLL.Repositories;
using Company.Route_C44_G01.DAL.Models;
using Company.Route_C44_G01.PL.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route_C44_G01.PL.Controllers
{
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepo _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(
            //IDepartmentRepo departmentRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]  // GET: /Department/Index 
        public IActionResult Index(string? SearchDept)
        {
            IEnumerable<Department> depts;
            if (string.IsNullOrEmpty(SearchDept))
            {
                depts = _unitOfWork.DepartmentRepo.GetAll();
            }
            else
            {
                depts = _unitOfWork.DepartmentRepo.GetByName(SearchDept);
            }

            return View(depts);
        }

        [HttpGet] 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                //var department = new Department()
                //{
                //    Code = model.Code,
                //    Name = model.Name,
                //    CreateAt = model.CreateAt
                //};

                var department = _mapper.Map<Department>(model);
                _unitOfWork.DepartmentRepo.Add(department);
                var count = _unitOfWork.SaveChanges();

                if (count > 0)
                {
                    TempData["Message"] = "Department Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id , string viewName = "Details")
        {
            if (!id.HasValue) return BadRequest("Invalid Id"); // 400
            var department = _unitOfWork.DepartmentRepo.Get(id.Value);
            if (department is null) return NotFound(); // 404

            return View(viewName , department);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (!id.HasValue) return BadRequest(); // 400
            var department = _unitOfWork.DepartmentRepo.Get(id.Value);
            if (department is null) return NotFound(); // 404
            var departmentDTO = new DepartmentDTO()
            {
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt
            };
            return View(departmentDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromRoute] int id,DepartmentDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = new Department()
                {
                    Id = id,
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                _unitOfWork.DepartmentRepo.Update(department);
                var count = _unitOfWork.SaveChanges();

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

            return Details(id , "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Department department)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                _unitOfWork.DepartmentRepo.Delete(department);
                var count = _unitOfWork.SaveChanges();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }
    }
}
