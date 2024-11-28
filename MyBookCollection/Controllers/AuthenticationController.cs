using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBookCollection.Helpers.Roles;
using MyBookCollection.Models.Data;
using MyBookCollection.Models.ViewModels;
using MyBookCollection.Services;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using NuGet.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace MyBookCollection.Controllers
{
    public class AuthenticationController : Controller
    {
        private IUserService _userService;
        private SignInManager<AppUser> _signInManager;
        private UserManager<AppUser> _userManager;
        private IConfiguration _configuration;

        public AuthenticationController(IUserService userService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        [Authorize]
        public async Task<IActionResult> Users()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Authentication");
            }
            return View(user);
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
                        else if(userLoggedIn.IsNotAllowed)
                        {
                            ViewBag.Message = "<strong>Email is not verified!</strong> <br/> Email verification link " +
                                "has been sent to your Email Address: " + user.Email;
                            return View("Login", new LoginVM());
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
            ViewBag.Message = "Password must contain at least one digit, one special character, one uppercase and one lowercase letter.";
            return View(new RegisterVM());
        }

        public async Task<IActionResult> UserRegistration(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Password must contain at least one digit, one special character, one uppercase and one lowercase letter.";
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
                        Email = registerVM.EmailAddress,
                        UserName = registerVM.EmailAddress,
                        ImageLink = "~/images/NoImage.png",
                        LockoutEnabled = true
                    };

                    var userCreated = await _userManager.CreateAsync(newUser, registerVM.Password);
                    if (userCreated.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(newUser, Role.User);
                        if(await _userManager.IsEmailConfirmedAsync(newUser))
                        {
                            //Automatically login after registration
                            await _signInManager.PasswordSignInAsync(newUser, registerVM.Password, false, false);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            //Generate token
                            var activationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                            //Generate Email Confirmation Link
                            var confirmationLink = Url.Action("ConfirmEmail", "Authentication",new {userId = newUser.Id, token =activationToken},Request.Scheme);

                            //Send Email to User
                            await SendVerificationLinkEmail(newUser.Email, confirmationLink);
                            ViewBag.VerificationMessage = "<strong>Registration is successfully done!</strong> <br/> Account activation link " +
                                "has been sent to your Email Address: " + newUser.Email;
                            return View("Register", registerVM);
                        }
                        
                    }
                    else
                    {
                        ViewBag.Message = "<strong>Warning!</strong> <br/> Something went wrong! Try again.";
                        return View("Register", registerVM);
                    }

                }
            }
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            bool status = false;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.Message = "<strong>Warning!</strong> <br/> Something went wrong! Try again.";
                return View("Login");
            }
            else
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    status = true;
                    ViewBag.Status = status;
                    ViewBag.SuccessMessage = "<strong>Thank you!</strong> <br/> Your email has been confirmed. You can now log in!<br/>";
                    return View();
                }
                else
                {
                    ViewBag.Message = "<strong>Warning!</strong> <br/> Something went wrong! Try again.<br/>";
                    return View();
                }
                
            }
        }

     

        //Get Remaining Locked Out time
        [NonAction]
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
        [NonAction]
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

        //Sending verification email
        [NonAction]
        private async Task SendVerificationLinkEmail(string emailAddress, string confirmationLink)
        {
            var fromEmail = new MailAddress("sssk08844@gmail.com", "My Book Collection");
            var toEmail = new MailAddress(emailAddress);
            var fromEmailPassword = "ddvo rsdv klgu jliy";
            string subject = "Verify Email Address";
            string body = $"<h4 class='text-center'><b>My Book Collection</b></h4><br/><br/>" +
                          $"<strong>Hello! Your account has been created successfully.</strong><br/>" +
                          $"Please confirm your email by clicking the button below:<br/><br/>" +
                          $"<a href='{confirmationLink}' style='text-decoration: none;'>" +
                          $"<button style='background-color: #28a745; color: white; padding: 10px 20px; border: none; border-radius: 5px; cursor: pointer;'>Confirm Email</button>" +
                          $"</a><br/><br/>" +
                          $"Regards,<br/>My Book Collection<br/>" +
                          $"<hr/>" +
                          $"<small>If you’re having trouble clicking the \"Confirm Email\" button, copy and paste the URL below into your web browser: <br/>{confirmationLink}</small><br/><br/>" +
                          $"<small class='text-center'>&copy; {DateTime.Now.Year} My Book Collection, All Rights Reserved</small><br/><br/>";

            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                await smtpClient.SendMailAsync(message);
            }

        }

        

    }
}
