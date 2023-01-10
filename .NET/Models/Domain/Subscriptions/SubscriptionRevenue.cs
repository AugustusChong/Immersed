using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain.Subscriptions
{
    public class SubscriptionRevenue
    {
        public DateTime TimePeriod { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
