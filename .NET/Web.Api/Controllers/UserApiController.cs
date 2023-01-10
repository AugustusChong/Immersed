using Google.Apis.AnalyticsReporting.v4.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.Domain;
using Models.Domain.Users;
using Models.Enums;
using Models.Requests;
using Services;
using Services.Interfaces;
using Web.Controllers;
using Web.Models.Responses;
using SendGrid.Helpers.Mail;
using Stripe;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : BaseApiController
    {
        private IUserService _userService = null;
        private IAuthenticationService<int> _authService = null;
        private IEmailsService _emailsService = null;
        public UserApiController(IEmailsService emailsService, IUserService service, ILogger<UserApiController> logger, IAuthenticationService<int> authenticationService) : base(logger)
        {
            _userService = service;
            _emailsService = emailsService;
            _authService = authenticationService;
        }

        [HttpGet("status/totals")]
        [Authorize(Roles = "SysAdmin")]
        public ActionResult<ItemResponse<UserStatusReqId>> GetAllUserStatus()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int id = _authService.GetCurrentUserId();
                UserStatusReqId user = _userService.GetUserStatusTotals(id);

                if (user == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<UserStatusReqId> { Item = user };
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

        [HttpGet("status/overTime")]
        [Authorize(Roles = "SysAdmin")]
        public ActionResult<ItemsResponse<UserStatus>> GetStatusOverTime()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<UserStatus> list = _userService.GetUserStatusOverTime();

                if(list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found");
                }
                else
                {
                    response = new ItemsResponse<UserStatus> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }
    }
}
