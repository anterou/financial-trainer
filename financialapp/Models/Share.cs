using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financialapp.Models
{
    public class Share
    {
        public string Name { get; set; }
        public string Ticker { get; set; }
        public string Price { get; set; }
        public string PriceChange { get; set; }
        public string AnnualReturn { get; set; }
        public string Risk { get; set; }
    }
}
