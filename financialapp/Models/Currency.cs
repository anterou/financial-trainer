using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financialapp.Models
{
    public class Currency
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public List<CurrencyRate> Rates { get; set; }
    }
}
