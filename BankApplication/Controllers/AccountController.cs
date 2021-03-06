﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApplication.Models;
using BankApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApplication.Controllers
{
    [Authorize(Roles = "Cashier")]
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
        [ValidateAntiForgeryToken]
        public IActionResult Deposit(DepositWithdrawalViewModel values)
        {
            if (ModelState.IsValid)
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
            }

            return PartialView("_DepositDone", values);
        }

        public IActionResult Withdrawal(int id)
        {
            var model = new DepositWithdrawalViewModel();
            model.Account = _context.Accounts.SingleOrDefault(a => a.AccountId == id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Withdrawal(DepositWithdrawalViewModel values)
        {
            if (ModelState.IsValid)
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

            return View();
        }

        public IActionResult Transaction(int id)
        {
            var model = new Transactions();
            model.AccountId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transaction(Transactions values)
        {
            if (ModelState.IsValid)
            {
                var transactionInfo = new TransactionDoneViewModel();
                transactionInfo.Transaction = values;
                transactionInfo.Amount = values.Amount;

                var transactionFrom = new Transactions();
                transactionFrom = values;
                transactionFrom.Date = DateTime.Now;
                transactionFrom.Date.ToShortDateString();
                transactionFrom.Type = "Debit";
                transactionFrom.Operation = "Remittance to Another Bank";

                var transactionTo = new Transactions();
                transactionTo.AccountId = Int32.Parse(values.Account);
                transactionTo.Date = transactionFrom.Date;
                transactionTo.Type = "Credit";
                transactionTo.Operation = "Collection from Another Bank";
                transactionTo.Amount = values.Amount;
                transactionTo.Symbol = values.Symbol;
                transactionTo.Bank = values.Bank;
                transactionTo.Account = values.AccountId.ToString();

                var oldTo = _context.Accounts.SingleOrDefault(a => a.AccountId == Int32.Parse(values.Account));

                if (oldTo == null)
                {
                    return PartialView("_TransactionFailed", values);
                }

                var newTo = oldTo;
                newTo.Balance = oldTo.Balance + values.Amount;
                transactionTo.Balance = newTo.Balance;

                var oldFrom = _context.Accounts.SingleOrDefault(a => a.AccountId == values.AccountId);
                if (oldFrom.Balance < values.Amount)
                {
                    return PartialView("_TransactionAmountToHigh");
                }

                transactionFrom.Amount = (-(values.Amount));

                var newFrom = oldFrom;
                newFrom.Balance = oldFrom.Balance + (values.Amount);
                transactionFrom.Balance = newFrom.Balance;

                _context.Entry(oldFrom).CurrentValues.SetValues(newFrom);
                _context.Entry(oldTo).CurrentValues.SetValues(newTo);
                _context.Transactions.Add(transactionFrom);
                _context.Transactions.Add(transactionTo);
                _context.SaveChanges();
                ModelState.Clear();
                return PartialView("_TransactionDone", transactionInfo);
            }

            return View();

        }

        public IActionResult AccountDetails(int id, int page = 1)
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            const int pageSize = 20;

            var model = new AccountTransactionViewModel();
            model.Account =  _context.Accounts.SingleOrDefault(a => a.AccountId == id);
            var firstTransactionList = _context.Transactions.Where(t => t.AccountId == id);
            model.TransactionList = firstTransactionList.ToList();
            var totalNumber = model.TransactionList.Count();

            var transactions = GetTransactions(id, pageSize, (page - 1) * pageSize);

            model.ListViewModel = new LongListViewModel
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalNumberOfItems = totalNumber,
                CanShowMore = page * pageSize < totalNumber,
                Transactions = transactions
            };


            if (isAjax)
            {
                return PartialView("_TransactionRows", model);
            }
            else
            {
                return View(model);
            };
        }

        public IList<Transactions> GetTransactions(int id, int take, int skip)
        {
            var list = _context.Transactions.Where(t => t.AccountId == id);

            return list
                .Skip(skip).Take(take)
                .ToList();
        }

    }
}