using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financialapp.Models
{
    public class MutualFund
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string AnnualReturn { get; set; }
        public string ManagementFee { get; set; }
        public string MinimumInvestment { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}
