using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApplication;

namespace BankApplication.ViewModels
{
    public class IndexViewModel
    {
        public int nrOfCustomers { get; set; }
        public int nrOfAccounts { get; set; }
        public decimal totalBalance { get; set; }

        public IndexViewModel()
        {
            nrOfCustomers = 0;
            nrOfAccounts = 0;
            totalBalance = 0;
        }
    }
}
