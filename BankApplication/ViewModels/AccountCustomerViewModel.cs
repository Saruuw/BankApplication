using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApplication.Models;
using BankApplication;

namespace BankApplication.ViewModels
{
    public class AccountCustomerViewModel
    {
        public Customers Customer { get; set; }
        public List<Accounts> Accounts { get; set; }

        public decimal Total { get; set; }

        public AccountCustomerViewModel()
        {
            Accounts = new List<Accounts>();
            Total = 0;
        }
    }
}
