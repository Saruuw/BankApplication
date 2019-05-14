using System;
using System.Collections.Generic;

namespace BankApplication.Models
{
    public partial class PermenentOrder
    {
        public int OrderId { get; set; }
        public int AccountId { get; set; }
        public string BankTo { get; set; }
        public string AccountTo { get; set; }
        public decimal? Amount { get; set; }
        public string Symbol { get; set; }

        public virtual Accounts Account { get; set; }
    }
}
