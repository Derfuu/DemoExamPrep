using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriyatniyShelestWPF
{
    public class Agent
    {
        public int ID { get; set; }
        public int Priority { get; set; }
        public int Sales { get; set; }

        public decimal TotalSalesBy { get; set; }

        public string AgentType { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public string DirectorName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }

        public int getDiscount()
        {
            int discount;
            if (TotalSalesBy <= 10000) { discount = 0; return discount; }
            else if (TotalSalesBy <= 50000) { discount = 5; return discount; }
            else if (TotalSalesBy <= 150000) { discount = 10; return discount; }
            else if (TotalSalesBy <= 500000) { discount = 15; return discount; }
            else { discount = 25; return discount; }
        }
    }
}
