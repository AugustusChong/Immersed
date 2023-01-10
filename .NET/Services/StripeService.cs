using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Data;
using Data.Providers;
using Models;
using Models.AppSettings;
using Models.Domain;
using Models.Domain.Subscriptions;
using Models.Domain.Transactions;
using Models.Interfaces;
using Models.Requests;
using Models.Requests.Invoices;
using Models.Requests.Subscribe;
using Models.Requests.Transactions;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Subscription = Models.Domain.Subscriptions.Subscription;

namespace Services
{
    public class StripeService : IStripeService
    {
        private AppKeys _appKeys = null;
        private IDataProvider _data = null;
        
        public StripeService(IOptions<AppKeys> appKeys, IDataProvider data)
        {
            _appKeys = appKeys.Value;
            _data = data;
        }

        public List<List<SubscriptionRevenue>> GetTotalRevenue()
        {
            string procName = "[dbo].[SubscriptionTransactions_Select_TotalRevenue]";
            List<SubscriptionRevenue> weeklyRevenue = null;
            List<SubscriptionRevenue> monthlyRevenue = null;
            List<SubscriptionRevenue> yearlyRevenue = null;
            List<List<SubscriptionRevenue>> returnList = new List<List<SubscriptionRevenue>>();

            _data.ExecuteCmd
                (
                    storedProc: procName,
                    inputParamMapper: null,
                    singleRecordMapper: delegate (IDataReader reader, short set)
                    {
                        switch(set)
                        {
                            case 0:
                                {
                                    int columnIndex = 0;
                                    SubscriptionRevenue revenue = MapSingleRevenue(reader, columnIndex);
                                    if (weeklyRevenue == null)
                                    {
                                        weeklyRevenue = new List<SubscriptionRevenue>();
                                    }
                                    weeklyRevenue.Add(revenue);
                                    break;
                                }
                            case 1:
                                {
                                    int columnIndex = 0;
                                    SubscriptionRevenue revenue = MapSingleRevenue(reader, columnIndex);
                                    if (monthlyRevenue == null)
                                    {
                                        monthlyRevenue = new List<SubscriptionRevenue>();
                                    }
                                    monthlyRevenue.Add(revenue);
                                    break;
                                }
                            case 2:
                                {
                                    int columnIndex = 0;
                                    SubscriptionRevenue revenue = MapSingleRevenue(reader, columnIndex);
                                    if (yearlyRevenue == null)
                                    {
                                        yearlyRevenue = new List<SubscriptionRevenue>();
                                    }
                                    yearlyRevenue.Add(revenue);
                                    break;
                                }
                        }
                    }
                );
            returnList.Add(weeklyRevenue);
            returnList.Add(monthlyRevenue);
            returnList.Add(yearlyRevenue);
            return returnList;
        }

        private static SubscriptionRevenue MapSingleRevenue(IDataReader reader, int index)
        {
            SubscriptionRevenue revenue = new SubscriptionRevenue();
            revenue.TimePeriod = reader.GetSafeDateTime(index++);
            revenue.TotalRevenue = reader.GetSafeDecimal(index++);
            return revenue;
        }
    }
}
