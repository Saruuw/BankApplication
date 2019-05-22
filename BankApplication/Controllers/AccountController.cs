using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApplication.Models;
using BankApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankApplication.Controllers
{
    public class AccountController : Controller
    {
        private BankAppDataContext _context;

        public AccountController(BankAppDataContext context)
        {
            _context = context;
        }

        public IActionResult Deposit(int id)
        {
            var model = new DepositWithdrawalViewModel();
            model.Account = _context.Accounts.SingleOrDefault(a => a.AccountId == id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Deposit(DepositWithdrawalViewModel values)
        {
            values.Transaction.AccountId = values.Account.AccountId;
            values.Transaction.Date = DateTime.Now;
            values.Transaction.Date.ToShortDateString();
            values.Transaction.Type = "Credit";
            values.Transaction.Operation = "Credit in Cash";

            var old = _context.Accounts.SingleOrDefault(a => a.AccountId == values.Account.AccountId);
            values.Account.Frequency = old.Frequency;
            values.Account.Created = old.Created;
            values.Transaction.Balance = old.Balance + values.Transaction.Amount;
            values.Account.Balance = values.Transaction.Balance;
            _context.Entry(old).CurrentValues.SetValues(values.Account);

            _context.Transactions.Add(values.Transaction);
            _context.SaveChanges();
            ModelState.Clear();

            return PartialView("_DepositDone", values);
        }

        public IActionResult Withdrawal(int id)
        {
            var model = new DepositWithdrawalViewModel();
            model.Account = _context.Accounts.SingleOrDefault(a => a.AccountId == id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Withdrawal(DepositWithdrawalViewModel values)
        {
            var old = _context.Accounts.SingleOrDefault(a => a.AccountId == values.Account.AccountId);

            if (values.Transaction.Amount > old.Balance)
            {
                return PartialView("_WithdrawalFailed");
            }
            else
            {
                values.Transaction.AccountId = values.Account.AccountId;
                values.Transaction.Date = DateTime.Now;
                values.Transaction.Date.ToShortDateString();
                values.Transaction.Type = "Debit";
                values.Transaction.Operation = "Withdrawal in Cash";

                values.Account.Frequency = old.Frequency;
                values.Account.Created = old.Created;
                values.Transaction.Balance = old.Balance + (-(values.Transaction.Amount));
                values.Account.Balance = values.Transaction.Balance;
                _context.Entry(old).CurrentValues.SetValues(values.Account);

                _context.Transactions.Add(values.Transaction);
                _context.SaveChanges();
                ModelState.Clear();

                return PartialView("_WithdrawalDone", values);
            }
        }

        public IActionResult AccountDetails(int id)
        {
            var model = new AccountTransactionViewModel();
            model.Account = _context.Accounts.SingleOrDefault(a => a.AccountId == id);
            model.TransactionList = _context.Transactions.Where(t => t.AccountId == id).ToList();

            return View(model);
        }
    }
}