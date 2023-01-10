using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Models.AppSettings;
using Models.Domain;
using Models.Domain.Subscriptions;
using Models.Domain.Transactions;
using Models.Enums;
using Models.Interfaces;
using Models.Requests.Subscribe;
using Models.Requests.Transactions;
using Services;
using Web.Controllers;
using Web.Models.Responses;
using Stripe;
using Stripe.Checkout;
using Subscription = Models.Domain.Subscriptions.Subscription;

namespace Web.Api
{
    [Route("api/stripe")]
    [ApiController]
    public class StripeApiController : BaseApiController
    {
        private IStripeService _service = null;
        private IAuthenticationService<int> _authService = null;
        private AppKeys _appKeys;
        public StripeApiController(IStripeService service
            , ILogger<StripeApiController> logger
            , IAuthenticationService<int> authService
            , IOptions<AppKeys> appKeys) : base(logger)
        {
            _service = service;
            _authService = authService;
            _appKeys = appKeys.Value;
        }

        [HttpGet("revenue")]
        [Authorize(Roles = "SysAdmin")]
        public ActionResult<ItemsResponse<SubscriptionRevenue>> GetSubscriptionRevenue()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<List<SubscriptionRevenue>> list = _service.GetTotalRevenue();

                if(list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<List<SubscriptionRevenue>> { Items = list };
                }
            }
            catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }
    }
}
