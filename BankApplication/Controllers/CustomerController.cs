using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankApplication.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApplication.Controllers
{
    public class CustomerController : Controller
    {
        private BankAppDataContext _context;

        public CustomerController(BankAppDataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewCustomer()
        {
            var model = new Customers();

            return View(model);
        }

        [HttpPost]
        public IActionResult NewCustomer(Customers values)
        {
            //if (ModelState.IsValid)
            //{
                values.Gender = "female"; //Fixa radiobuttons sen!
                _context.Customers.Add(values);
                _context.SaveChanges();
                ModelState.Clear();
            //}

            return RedirectToAction("Index", "Bank");
        }

        public IActionResult SearchCustomer()
        {
            var model = new Customers();
            return View(model);
        }

        [HttpPost]
        public IActionResult SearchCustomer(int mySearch)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.CustomerId == mySearch);

            return View(customer);
        }
    }
}
