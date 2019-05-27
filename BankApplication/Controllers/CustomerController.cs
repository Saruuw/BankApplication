using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankApplication.Models;
using BankApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApplication.Controllers
{
    [Authorize(Roles = "Cashier")]
    public class CustomerController : Controller
    {
        private BankAppDataContext _context;

        public CustomerController(BankAppDataContext context)
        {
            _context = context;
        }

        public IActionResult NewCustomer()
        {
            var model = new CustomerMessageViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewCustomer(CustomerMessageViewModel values)
        {
            if (values.Customer.CustomerId == 0)
            {
                if (ModelState.IsValid)
                {
                    _context.Customers.Add(values.Customer);

                    Accounts newAccount = new Accounts();
                    newAccount.Balance = 0;
                    newAccount.Frequency = "Monthly";
                    newAccount.Created = DateTime.Now;
                    _context.Accounts.Add(newAccount);

                    Dispositions newDisposition = new Dispositions();
                    newDisposition.AccountId = newAccount.AccountId;
                    newDisposition.CustomerId = values.Customer.CustomerId;
                    newDisposition.Type = "OWNER";
                    _context.Dispositions.Add(newDisposition);

                    _context.SaveChanges();
                    ModelState.Clear();

                    values.Message = "Kunden har lagts till i kundregistret. Ett transaktionskonto för kunden har även skapats.";

                    return View(values);
                }
                else
                {
                    values.Message = "Kunden har ej lagts till i kundregistret. Vänligen kontrollera att alla obligatoriska fält är ifyllda.";
                    return View(values);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var old = _context.Customers.SingleOrDefault(c => c.CustomerId == values.Customer.CustomerId);

                    _context.Entry(old).CurrentValues.SetValues(values.Customer);
                    _context.SaveChanges();
                    ModelState.Clear();

                    values.Message = "Kundinformationen har uppdaterats";

                    return View(values);
                }
                values.Message = "Kundinformationen kunde ej sparas. Kontrollera att alla obligatoriska fält är ifyllda.";
                return View(values);
            }

        }

        public IActionResult EditCustomer(int id)
        {
            var model = new CustomerMessageViewModel();
            model.Customer = _context.Customers.SingleOrDefault(c => c.CustomerId == id);
            return View("NewCustomer", model);
        }

        public IActionResult SearchCustomerById()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SearchCustomerByName(CustomerSearchViewModel values, int page = 1)
        {
            if (values.SearchValue == null)
            {
                var model = new CustomerSearchViewModel();
                return View(model);
            }
            else
            {
                const int pageSize = 50;
                int offSet = pageSize * (page - 1);

                var model = new CustomerSearchViewModel();
                model.SearchValue = values.SearchValue;
                model.NameOrCity = values.NameOrCity;

                if (values.NameOrCity == "name")
                {
                    var customerList = _context.Customers.Where(c => c.Givenname.Contains(values.SearchValue) || c.Surname.Contains(values.SearchValue));
                    model.CustomerList = customerList.ToList();
                }
                else if (values.NameOrCity == "city")
                {
                    var customerList = _context.Customers.Where(c => c.City.Contains(values.SearchValue));
                    model.CustomerList = customerList.ToList();
                }
                else
                {
                    ViewData["Message"] = "Kryssa för ditt sökalternativ";
                    return View(model);
                }

                var total = model.CustomerList.Count();
                model.HasMorePages = offSet + pageSize < total;
                var firstCustomerList = model.CustomerList.Skip((page - 1) * pageSize).Take(pageSize);
                model.CustomerList = firstCustomerList.ToList();

                if (model.CustomerList.Count == 0)
                {
                    ViewData["Message"] = "Kunde ej hitta kund";
                    return View(model);
                }
                else
                {

                    if (total % 50 != 0)
                    {
                        model.TotalPages = (total / pageSize) + 1;
                    }
                    else
                    {
                        model.TotalPages = total / pageSize;
                    }

                }

                return View(model);
            }
        }

        public IActionResult ShowCustomer(int id)
        {
            var model = new AccountCustomerViewModel();
            var dispositionList = new List<Dispositions>();

            model.Customer = _context.Customers.SingleOrDefault(c => c.CustomerId == id);

            if(model.Customer == null)
            {
                ViewData["Message"] = "Kunde ej hitta kund med matchande kundnummer";
                return View("SearchCustomerById");
            }

            var firstDispositionList = _context.Dispositions.Where(d => d.CustomerId == id);
            dispositionList = firstDispositionList.ToList();

            foreach (var disp in dispositionList)
            {
                var acc = _context.Accounts.SingleOrDefault(a => a.AccountId == disp.AccountId);
                model.Accounts.Add(acc);
            }

            foreach (var acc in model.Accounts)
            {
                model.Total += acc.Balance;
            }

            return View(model);
        }
    }
}