using AutoMapper;
using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.BLL.Repositories;
using Company.Route_C44_G01.DAL.Models;
using Company.Route_C44_G01.PL.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route_C44_G01.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]  // GET: /Department/Index 
        public async Task<IActionResult> Index(string? SearchDept)
        {
            IEnumerable<Department> depts;
            if (string.IsNullOrEmpty(SearchDept))
            {
                depts = await _unitOfWork.DepartmentRepo.GetAll();
            }
            else
            {
                depts = await _unitOfWork.DepartmentRepo.GetByName(SearchDept);
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
        public async Task<IActionResult> Create(DepartmentDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                

                var department = _mapper.Map<Department>(model);
                await _unitOfWork.DepartmentRepo.Add(department);
                var count = await _unitOfWork.SaveChanges();

                if (count > 0)
                {
                    TempData["Message"] = "Department Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id , string viewName = "Details")
        {
            if (!id.HasValue) return BadRequest("Invalid Id"); // 400
            var department = await _unitOfWork.DepartmentRepo.Get(id.Value);
            if (department is null) return NotFound(); // 404

            return View(viewName , department);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue) return BadRequest(); // 400
            var department = await _unitOfWork.DepartmentRepo.Get(id.Value);
            if (department is null) return NotFound(); // 404
            //var departmentDTO = new DepartmentDTO()
            //{
            //    Code = department.Code,
            //    Name = department.Name,
            //    CreateAt = department.CreateAt
            //};
            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return View(departmentDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] int id,DepartmentDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                //var department = new Department()
                //{
                //    Id = id,
                //    Code = model.Code,
                //    Name = model.Name,
                //    CreateAt = model.CreateAt
                //};
                var department = _mapper.Map<Department>(model);
                department.Id = id;
                _unitOfWork.DepartmentRepo.Update(department);
                var count = await _unitOfWork.SaveChanges();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            

            return await Details(id , "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Department department)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                _unitOfWork.DepartmentRepo.Delete(department);
                var count = await _unitOfWork.SaveChanges();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }
    }
}
