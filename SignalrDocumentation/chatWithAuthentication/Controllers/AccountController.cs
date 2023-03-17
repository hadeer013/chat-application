using ChatWithAuth.DAL.Data;
using chatWithAuthentication.Hubs;
using chatWithAuthentication.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace chatWithAuthentication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHubContext<AllUsersHub> usersHubContext;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IHubContext<AllUsersHub> UsersHubContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            usersHubContext = UsersHubContext;
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Email = registerModel.Email,
                    UserName = registerModel.UserName
                };
                var result = await userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    await usersHubContext.Clients.All.SendAsync("AllUsers");
                    return RedirectToAction(nameof(LogIn));
                }
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(registerModel);
        }


        public IActionResult LogIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.CheckPasswordAsync(user, model.Password);
                    if (result)
                    {
                        var signIn = await signInManager.PasswordSignInAsync(user, model.Password, true, false);
                        if (signIn.Succeeded)
                        {
                           

                            return RedirectToAction("Index", "Home");
                        }


                    }
                }
                return NotFound();
            }
            return View(model);
        }
        public new async Task<IActionResult> SignOut()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(Email);

            
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(LogIn));
        }
    }
}
