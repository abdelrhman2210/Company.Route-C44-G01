using AutoMapper;
using Company.Route_C44_G01.BLL;
using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.BLL.Repositories;
using Company.Route_C44_G01.DAL.Models;
using Company.Route_C44_G01.PL.DTOS;
using Company.Route_C44_G01.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route_C44_G01.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public EmployeeController(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]  // GET: /Department/Index 
        public async Task<IActionResult> Index(string? SearchEmp)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchEmp))
            {
                employees = await _unitOfWork.EmployeeRepo.GetAll();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepo.GetByName(SearchEmp);
            }

            #region Dictionary Explanation
            // Dictionary : 3 Property
            // 1.ViewData : Transfer Extra Information From Controller (Action) To View
            //ViewData["Message"] = "Hellooooooooooooooooooo";

            // 2.ViewBag : Transfer Extra Information From Controller (Action) To View
            //ViewBag.Message = "Hellooooooooooooooooooo from ViewBag";

            // 3.TempData : Transfer Extra Information From One Request To Another Request 
            #endregion

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepo.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeDTO model)
        {
            var employee = _mapper.Map<Employee>(model);

            if (ModelState.IsValid) // Server Side Validation
            {
                if(model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                }

                await _unitOfWork.EmployeeRepo.Add(employee);
                var count = await _unitOfWork.SaveChanges();
                if (count > 0)
                {
                    TempData["Message"] = "Employee Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue) return BadRequest("Invalid Id"); // 400
            var employee = await _unitOfWork.EmployeeRepo.Get(id.Value);
            if (employee is null) return NotFound(); // 404

            return View(viewName, employee);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            var departments = await _unitOfWork.DepartmentRepo.GetAll();
            ViewData["departments"] = departments;
            if (!id.HasValue) return BadRequest(); // 400
            var model = await _unitOfWork.EmployeeRepo.Get(id.Value);
            if (model is null) return NotFound(); // 404
            var employee = _mapper.Map<CreateEmployeeDTO>(model);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] int id , CreateEmployeeDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                if (model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName , "Images");
                }

                if(model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                }
                
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.EmployeeRepo.Update(employee);
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

            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id ,Employee employee)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                if (ModelState.IsValid)
                {
                    if (id == employee.Id)
                    {
                        _unitOfWork.EmployeeRepo.Delete(employee);
                        var cnt = await _unitOfWork.SaveChanges();
                        if (cnt > 0)
                        {
                            DocumentSettings.DeleteFile(employee.ImageName, "Images");
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                        return BadRequest();
                }
            }
            return View(employee);
        }
    }
}
