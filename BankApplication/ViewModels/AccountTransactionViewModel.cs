using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.ViewModels
{
    public class AccountTransactionViewModel
    {
        public Accounts Account { get; set; }
        public List<Transactions> TransactionList { get; set; }

        public AccountTransactionViewModel()
        {
            Account = new Accounts();
            TransactionList = new List<Transactions>();
        }
    }
}
