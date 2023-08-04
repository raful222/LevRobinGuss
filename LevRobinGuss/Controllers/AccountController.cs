using LevRobinGuss.Core.Entities.Identity;
using LevRobinGuss.VIewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LevRobinGuss.Controllers
{
    public class AccountController : Controller
    {

        private readonly ApplicationUserManager _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(ApplicationUserManager userManager
            , SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("index", "home", null);
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home", null);
        }

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    FirstName = model.Name,
                    MetaMaskKey = model.MetaMuskNumber,
                    UserName = model.Email,
                };
                var usersub= await _userManager.CreateAsync(user, model.Password);


                return RedirectToAction("index", "home", null);

            }
            return View(model);
        }
    }
}
