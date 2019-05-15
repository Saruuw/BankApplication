using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApplication.Models;
using BankApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApplication.Controllers
{
    public class BankController : Controller
    {
        private BankAppDataContext _context;

        public BankController(BankAppDataContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
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
    }
}
