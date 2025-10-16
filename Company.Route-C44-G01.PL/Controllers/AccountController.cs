using Company.Route_C44_G01.DAL.Models;
using Company.Route_C44_G01.PL.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route_C44_G01.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        #region SignUp

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        //P@ssW0rd
        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDTO model)
        {
            if (ModelState.IsValid)
            {
                var usr = await _userManager.FindByNameAsync(model.UserName);
                if (usr is null)
                {
                    usr = await _userManager.FindByEmailAsync(model.Email);
                    if (usr is null)
                    {
                        usr = new AppUser
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree
                        };

                        var result = await _userManager.CreateAsync(usr, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid SignUp !!");
            }
            return View(model);
        }
        #endregion

        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        //P@ssW0rd
        [HttpPost]
        public async Task<IActionResult> SignIn(SigninDTO model)
        {
            if (ModelState.IsValid)
            {
                var usr = await _userManager.FindByEmailAsync(model.Email);
                if (usr is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(usr, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(usr, model.Password, model.RememberME, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid SignIn !!");
            }
            return View(model);
        }
        #endregion

        #region SignOut

        #endregion
    }
}
