using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financialapp.Models
{
    public class Metal
    {
        public string Name { get; set; } 
        public string Code { get; set; } 
        public List<Price> Prices { get; set; } 
    }
}
