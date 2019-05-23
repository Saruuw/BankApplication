using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.ViewModels
{
    public class CustomerSearchViewModel
    {
        public List<Customers> CustomerList { get; set; }
        public string SearchValue { get; set; }

        public string NameOrCity { get; set; }

        public CustomerSearchViewModel()
        {
            CustomerList = new List<Customers>();
        }
    }
}
