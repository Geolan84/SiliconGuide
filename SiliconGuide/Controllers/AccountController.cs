using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SiliconGuide.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace SiliconGuide.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersRepository _repo;
        public AccountController(IUsersRepository u)
        {
            _repo = u;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(User?.Identity?.Name == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Guide");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = _repo.Get(model.Email, model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email, user.AccessLevel);
                    return RedirectToAction("Index", "Guide");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = _repo.Get(model.Email, model.Password);
                if (user == null)
                {
                    int id = _repo.GetFreeID();
                    User newUser = new User {Login = model.Login, UserID = id,
                        Password = HashOperations.Encode(model.Password, model.Email),
                        Email = model.Email,  Name = model.Name, Surname = model.Surname, AccessLevel = 0,
                        Patronymic = model.Patronymic, Organisation = model.Organisation};
                    _repo.Create(newUser);
                    await Authenticate(model.Email, newUser.AccessLevel);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Эти данные уже заняты");
            }
            return View(model);
        }

        private async Task Authenticate(string userName, byte access)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimTypes.Role, access==0?"user":"admin")
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}