using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.ViewModels
{
    public class TransactionDoneViewModel
    {
        public Transactions Transaction { get; set; }

        public decimal Amount { get; set; }

        public TransactionDoneViewModel()
        {
            Transaction = new Transactions();
            Amount = 0;
        }
    }
}
