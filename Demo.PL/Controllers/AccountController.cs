using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManger, SignInManager<ApplicationUser> signInManager)
        {
            _userManger = userManger;
            _signInManager = signInManager;
        }
        #region Sign Up
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManger.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    user = await _userManger.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                        user = new ApplicationUser()
                        {
                            UserName = model.UserName,
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            IsAgree = model.IsAgree
                        };
                    }

                    var result = await _userManger.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);

                    }
                }
            }
            ModelState.AddModelError(string.Empty, "User Is Already Exist (:");
            return View(model);
        }
        #endregion

        #region Sign In
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {

            if (ModelState.IsValid)
            {

                var user = await _userManger.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var Flag = await _userManger.CheckPasswordAsync(user, model.Password);
                    if (Flag)
                    {
                        //////////////////////////////
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login!!");
            }
            return View(model);
        }
        #endregion

        #region Sign Out

        public new async Task<IActionResult> SignOut()
        {
            /////////////////////////////////
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion

        #region ForgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public async Task<IActionResult> SendResetPasswordUrl(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                /////////////////////////////
                var user = await _userManger.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                   var token = await _userManger.GeneratePasswordResetTokenAsync(user);
                  
                   var url = Url.Action("ResetPassword", "Account", new { email = model.Email , token = token}, Request.Scheme);


                    var email = new Email()
                    {
                        Subject = "Reset Your Password",
                        Recipients = model.Email,
                        Body = url
                    };

                     EmailSettings.SendEmail(email);


                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invalid Email");
            }
            return View(nameof(ForgotPassword),model);
        }

        public IActionResult CheckYourInbox()
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
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                var user = await _userManger.FindByEmailAsync(email);
                if (user is not null)
                {
                 var result =  await _userManger.ResetPasswordAsync(user, token, model.NewPassword);
                   if(result.Succeeded)         
                        return RedirectToAction(nameof(SignIn));
                   foreach (var error in result.Errors)
                   {
                       ModelState.AddModelError(string.Empty, error.Description);
                   }

                    ModelState.AddModelError(string.Empty, "Invalid Reset Password");

                }

            }
            return View(model);
        }
        #endregion

        public IActionResult AccessDenied()
        {

            return View();
        }


    }
}
