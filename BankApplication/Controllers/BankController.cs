using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApplication.Data;
using BankApplication.Models;
using BankApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApplication.Controllers
{
    public class BankController : Controller
    {
        private BankAppDataContext _context;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public BankController(BankAppDataContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(RegisterUser user)
        {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, true, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Bank");
                    }
                }
                ViewData["Message"] = "Användarnamn eller lösenord stämmer ej. Försök igen";
                return View();
        }

        [Authorize]
        public async Task<IActionResult> LogOff()
        {

            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Bank");
        }

        [Authorize]
        public IActionResult Index()
        {
            var model = new IndexViewModel();

            model.nrOfCustomers = _context.Customers.Count();
            model.nrOfAccounts = _context.Accounts.Count();

            var accounts = _context.Accounts.ToList();

            foreach(var acc in accounts)
            {
                model.totalBalance += acc.Balance;
            }
            
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUser user)
        {
            if (user.Password == user.CheckPassword)
            {
                if (ModelState.IsValid)
                {
                    var userIdentity = new ApplicationUser { UserName = user.UserName };

                    var result = await _userManager.CreateAsync(userIdentity, user.Password);

                    var resultRole = await _userManager.AddToRoleAsync(userIdentity, user.RoleName);

                    if (result.Succeeded)
                    {
                        ViewData["Message"] = "Nya användare har skapats";
                    }
                }

                return View();
            }

            ViewData["Message"] = "Lösenorden matchar ej. Försök igen";
            return View();
        }
    }
}
