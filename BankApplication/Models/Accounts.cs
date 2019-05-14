using System;
using System.Collections.Generic;

namespace BankApplication.Models
{
    public partial class Accounts
    {
        public Accounts()
        {
            Dispositions = new HashSet<Dispositions>();
            Loans = new HashSet<Loans>();
            PermenentOrder = new HashSet<PermenentOrder>();
            Transactions = new HashSet<Transactions>();
        }

        public int AccountId { get; set; }
        public string Frequency { get; set; }
        public DateTime Created { get; set; }
        public decimal Balance { get; set; }

        public virtual ICollection<Dispositions> Dispositions { get; set; }
        public virtual ICollection<Loans> Loans { get; set; }
        public virtual ICollection<PermenentOrder> PermenentOrder { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
