using Models.Domain.Subscriptions;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStripeService
    {
        List<List<SubscriptionRevenue>> GetTotalRevenue();
    }
}
