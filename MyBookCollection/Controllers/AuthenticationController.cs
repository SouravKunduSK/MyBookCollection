using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBookCollection.Models.Data;
using MyBookCollection.Models.ViewModels;
using MyBookCollection.Services;

namespace MyBookCollection.Controllers
{
    public class AuthenticationController : Controller
    {
        //private IUserService _userService;
        private SignInManager<AppUser> _signInManager;
        private UserManager<AppUser> _userManager;

        public AuthenticationController( SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            //_userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        //Login users
        public async Task<IActionResult> Login()
        {
            return View(new LoginVM());
        }

        public async Task<IActionResult> LoginSubmitted(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", loginVM);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
                if (user != null)
                {
                    var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                    if (passwordCheck)
                    {
                        var userLoggedIn = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
                        if (userLoggedIn.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.Message = "<strong>Invalid Login attempt!</strong> <br/> Please, check your email and password.";
                            return View("Login", loginVM);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "<strong>Invalid Login attempt!</strong> <br/> Please, check your email and password.";
                        return View("Login", loginVM);
                    }
                }
                else
                {
                    ViewBag.Message = "<strong>Invalid Login attempt!</strong> <br/> Please, check your email and password.";
                    
                    return View("Login", loginVM);
                }
                
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Authentication");
        }
        public IActionResult Index()
        {
            return View();
        }


    }
}
