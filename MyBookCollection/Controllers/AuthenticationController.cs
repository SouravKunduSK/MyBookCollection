using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBookCollection.Helpers.Roles;
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

        public AuthenticationController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
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
                    if (passwordCheck && !await _userManager.IsLockedOutAsync(user))
                    {
                        var userLoggedIn = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
                        if (userLoggedIn.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.Message = GetRemainingAttempts(user);
                            return View("Login", loginVM);
                        }
                    }
                    else
                    {
                        if(!await _userManager.IsLockedOutAsync(user))
                        {
                            await _userManager.AccessFailedAsync(user);
                        }
                        
                        if (await _userManager.IsLockedOutAsync(user))
                        {
                            ViewBag.Message = GetRemainingTimes(user);
                            return View("Login", loginVM);
                        }
                        else
                        {
                            ViewBag.Message = GetRemainingAttempts(user);

                            return View("Login", loginVM);
                        }

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

        //Register New Users
        public async Task<IActionResult> Register()
        {
            ViewBag.Message = "Password must contain at least one digit, one uppercase and one lowercase letter.";
            return View(new RegisterVM());
        }

        public async Task<IActionResult> UserRegistration(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Password must contain at least one digit, one uppercase and one lowercase letter.";
                return View("Register", registerVM);
            }
            else
            {
                var emailExists = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
                if (emailExists != null)
                {
                    ViewBag.Message = "<strong>Warning!</strong> <br/> This email has already been used! Try another.";
                    return View("Register", registerVM);
                }
                else
                {
                    var newUser = new AppUser()
                    {
                        FullName = registerVM.FirstName + " " + registerVM.LastName,
                        UserName = registerVM.FirstName + "_" + registerVM.LastName,
                        Email = registerVM.EmailAddress,
                        LockoutEnabled = true
                    };

                    var userCreated = await _userManager.CreateAsync(newUser, registerVM.Password);
                    if (userCreated.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(newUser, Role.User);

                        //Automatically login after registration
                        await _signInManager.PasswordSignInAsync(newUser, registerVM.Password, false, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "<strong>Warning!</strong> <br/> Something went wrong! Try again.";
                        return View("Register", registerVM);
                    }

                }
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get Remaining Locked Out time
        private string GetRemainingTimes(AppUser user)
        {
            var lockedOutEnd = user.LockoutEnd?.UtcDateTime;
            if (lockedOutEnd.HasValue)
            {
                var remainingTime = lockedOutEnd.Value - DateTime.UtcNow;
                if (remainingTime > TimeSpan.Zero)
                {
                    return $"<strong>Your account is locked!</strong> <br/> Please try again after {remainingTime.Hours} hours, {remainingTime.Minutes} minutes, and {remainingTime.Seconds} seconds.";
                }
                else
                {
                    return "<strong>Your account is no longer locked out. Please try logging in again.</strong>";
                }
            }
            else
            {
                return GetRemainingTimes(user); 
            }
        }

        //Get remaining attempts
        private string GetRemainingAttempts(AppUser user)
        {
            var maxAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
            var remainingAttempts = maxAttempts - user.AccessFailedCount;

            if (remainingAttempts > 0)
            {
                return $"<strong>Invalid Login attempt!</strong> <br/> You have {remainingAttempts} attempt{(remainingAttempts > 1 ? "s" : "")} remaining before your account is locked.";
            }
            else
            {
                return GetRemainingTimes(user);
            }
        }



    }
}
