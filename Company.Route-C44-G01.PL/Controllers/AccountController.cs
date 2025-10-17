using Company.Route_C44_G01.DAL.Models;
using Company.Route_C44_G01.PL.DTOS;
using Company.Route_C44_G01.PL.Helpers;
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
        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }
        #endregion

        #region Forget Password
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPassDTO model)
        {
            if (ModelState.IsValid)
            {
                var usr = await _userManager.FindByEmailAsync(model.Email);
                if (usr is not null)
                {
                    //Generate Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(usr);


                    //Create Url
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    //Create email
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };

                    //Send Email
                    var flag = EmailSettings.SendEmail(email);
                    if (flag)
                    {
                        //Check your email
                        return RedirectToAction("CheckYourEmail");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid Email !!");
            return View(model);
        }

        [HttpGet]
        public IActionResult CheckYourEmail()
        {
            return View();
        }

        #endregion

        #region Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassDTO model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                if (email is null || token is null)
                {
                    return BadRequest("Invalid Operations");
                }
                var usr = await _userManager.FindByEmailAsync(email);
                if (usr != null)
                {
                    var result = await _userManager.ResetPasswordAsync(usr, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Reset password Operation !!");
                }
            }
            return View(model);
        }
        #endregion

    }
}
