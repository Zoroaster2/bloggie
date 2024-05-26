using bloggie.web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace bloggie.web.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        //microsoft identity give us usermaniger service that we can use to creat users and register users in database very easily
        //it takes an type of user wich is TUser and our user is identity user 
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            //here i say that get those value from viewmodel
            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email
            };
            //here im crating the user and give the password from viewmodel 
            var IdentityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

            if (IdentityResult.Succeeded)
            {
                //here im asighn the role to user
                var RoleIdentityResult = await userManager.AddToRoleAsync(identityUser, "user");
                if (RoleIdentityResult.Succeeded)
                {
                    //show succsess notification
                    return RedirectToAction("Register");
                }
            }
            //show error notif
            return View("register");
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = ReturnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var SignInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);
            if (SignInResult != null && SignInResult.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            //show error
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}


