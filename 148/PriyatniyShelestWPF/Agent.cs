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
            if (TotalSalesBy <= 10000) { return 0; }
            else if (TotalSalesBy <= 50000) { return 5; }
            else if (TotalSalesBy <= 150000) { return 10; }
            else if (TotalSalesBy <= 500000) { return 15; }
            else { return 25; }
        }
    }
}
