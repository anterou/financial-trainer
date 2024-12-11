using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financialapp.Models
{
    public class Deposit
    {
        public string BankName { get; set; }
        public double Rate { get; set; }
        public string Period { get; set; }
        public int AmountFrom { get; set; }
        public int AmountTo { get; set; }
    }
}
