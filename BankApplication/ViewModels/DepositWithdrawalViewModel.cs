using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.ViewModels
{
    public class DepositWithdrawalViewModel
    {
        public Accounts Account { get; set; }
        public Transactions Transaction { get; set; }

        public DepositWithdrawalViewModel()
        {
            Account = new Accounts();
            Transaction = new Transactions();
        }
    }
}
