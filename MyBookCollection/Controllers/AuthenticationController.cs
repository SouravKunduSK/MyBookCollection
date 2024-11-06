using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBookCollection.Models.Data;
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
            return View("Login");
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}
