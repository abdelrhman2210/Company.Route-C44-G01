using Company.Route_C44_G01.DAL.Models;
using Company.Route_C44_G01.PL.DTOS;
using Company.Route_C44_G01.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Company.Route_C44_G01.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(string? SearchEmp)
        {
            IEnumerable<RoleToReturnDTO> roles;
            if (string.IsNullOrEmpty(SearchEmp))
            {
                roles = _roleManager.Roles.Select(r => new RoleToReturnDTO
                {
                    Id = r.Id,
                    Name = r.Name
                });
            }
            else
            {
                roles = _roleManager.Roles
                    .Select(r => new RoleToReturnDTO
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .Where(r => r.Name != null && r.Name.ToLower().Contains(SearchEmp.ToLower()));
            }

            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleToReturnDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var roleExist = await _roleManager.FindByNameAsync(model.Name);

                if (roleExist is null)
                {
                    var role = new IdentityRole
                    {
                        Name = model.Name
                    };
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Role Name already exists");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest("Invalid Id"); // 400

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound(new { StatusCode = 404, message = $"Role With Id : {id} is not found" }); // 404

            var dto = new RoleToReturnDTO
            {
                Id = role.Id,
                Name = role.Name
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
        public async Task<IActionResult> Update([FromRoute] string id, RoleToReturnDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (id != model.Id)
                return BadRequest("Invalid Operations !!");

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound(new { StatusCode = 404, message = $"User With Id : {id} is not found" }); // 404

            var roleRes = await _roleManager.FindByNameAsync(model.Name);
            if (roleRes is null)
            {
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Role Name already exists");
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
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (id != model.Id)
                return BadRequest("Invalid Operations !!");

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound(new { StatusCode = 404, message = $"User With Id : {id} is not found" }); // 404

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsr(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound(new { StatusCode = 404, message = $"Role With Id : {roleId} is not found" }); // 404

            ViewData["RoleId"] = roleId;

            var usersInrole = new List<UsersInroleDTO>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = await _userManager.IsInRoleAsync(user, role.Name);
                usersInrole.Add(new UsersInroleDTO
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = userInRole
                });
            }
            return View(usersInrole);
        }

        

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsr(string roleId , List<UsersInroleDTO> usersInroleDTOs)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound(new { StatusCode = 404, message = $"Role With Id : {roleId} is not found" }); // 404

            if(ModelState.IsValid)
            {
                foreach (var item in usersInroleDTOs)
                {
                    var user = await _userManager.FindByIdAsync(item.UserId);
                    if (user is null)
                        continue;
                    IdentityResult result = null;
                    if (item.IsSelected && !await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (! item.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Something went wrong");
                    }
                }
                return RedirectToAction(nameof(Update), new {id = roleId});
            }
            return View(usersInroleDTOs);
        }
    }
}
