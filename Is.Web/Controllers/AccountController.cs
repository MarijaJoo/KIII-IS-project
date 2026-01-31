using Is.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using Is.Domain.DomainModels;

namespace Is.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IsApplicationUser> userManager;

        private readonly SignInManager<IsApplicationUser> signInManager;

        public AccountController(UserManager<IsApplicationUser> userManager, SignInManager<IsApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Register()
        {
            UserRegistrationDto model = new UserRegistrationDto();
            return View(model);
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new IsApplicationUser
                    {
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email=request.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserCard = new Domain.DomainModels.MyCoursesCard()

                    };
                    var result = await userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login","Account");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exits.");
                    return View(request);
                }
            }
            return View(request);
        }
        [HttpGet,AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginDto model = new UserLoginDto();
            return View(model);
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                //var m = model.Email;
                var user = await userManager.FindByEmailAsync(model.Email?.Trim()); //nema normalized email vo excelot
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not cobfirmed yet");
                    return View(model);
                }
                if (await userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid credidentials");
                    return View(model);
                }
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }

        //[HttpPost, AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(UserLoginDto model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

           
        //    var email = model.Email?.Trim();

            
        //    var user = await userManager.FindByEmailAsync(email);

        //    if (user == null)
        //    {
        //        ModelState.AddModelError("message", "Invalid login attempt");
        //        return View(model);
        //    }

        //    if (!user.EmailConfirmed)
        //    {
        //        ModelState.AddModelError("message", "Email not confirmed yet");
        //        return View(model);
        //    }

            
        //    var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);

        //    if (result.Succeeded)
        //    {
               
        //        await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("UserRole", "Admin"));

                
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else if (result.IsLockedOut)
        //    {
        //        return View("AccountLocked");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("message", "Invalid login attempt");
        //        return View(model);
        //    }
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync(); 
            return RedirectToAction("Index", "Home"); 
        }

    }
}
