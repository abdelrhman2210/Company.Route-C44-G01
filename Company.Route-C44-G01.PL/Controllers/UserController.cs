using Company.Route_C44_G01.DAL.Models;
using Company.Route_C44_G01.PL.DTOS;
using Company.Route_C44_G01.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route_C44_G01.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]  
        public async Task<IActionResult> Index(string? SearchEmp)
        {
            IEnumerable<UserToReturnDTO> users;
            if (string.IsNullOrEmpty(SearchEmp))
            {
                users = _userManager.Users.Select(U => new UserToReturnDTO()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                });           
            }
            else
            {
                users = _userManager.Users.Select(U => new UserToReturnDTO()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U => U.FirstName.ToLower().Contains(SearchEmp.ToLower()));
            }

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id"); // 400
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound(new {StatusCode= 404 , message = $"User With Id : {id} is not found"}); // 404

            var dto = new UserToReturnDTO()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            };
            return View(viewName, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string? id)
        {
            return await Details(id, "Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] string id, UserToReturnDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                if (id != model.Id)
                    return BadRequest("Invalid Operations !!");

                var user =  await _userManager.FindByIdAsync(id);
                if (user is null)
                    return NotFound(new { StatusCode = 404, message = $"User With Id : {id} is not found" }); // 404
                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                var result = await _userManager.UpdateAsync(user); 
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {

            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserToReturnDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                if (id != model.Id)
                    return BadRequest("Invalid Operations !!");

                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                    return NotFound(new { StatusCode = 404, message = $"User With Id : {id} is not found" }); // 404

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    }
}
