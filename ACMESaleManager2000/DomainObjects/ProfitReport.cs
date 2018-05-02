using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainObjects
{
    public class ProfitReport
    {
        public List<KeyValuePair<DateTime, decimal>> Incomes { get; set; }
        public List<KeyValuePair<DateTime, decimal>> Expenses { get; set; }
        public decimal Profit { get {
                return Incomes.Sum(e => e.Value) - Expenses.Sum(e => e.Value);
            }
        }
    }
}
