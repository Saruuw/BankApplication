using System;
using System.Collections.Generic;

namespace BankApplication.Models
{
    public partial class Dispositions
    {
        public Dispositions()
        {
            Cards = new HashSet<Cards>();
        }

        public int DispositionId { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public string Type { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Customers Customer { get; set; }
        public virtual ICollection<Cards> Cards { get; set; }
    }
}
