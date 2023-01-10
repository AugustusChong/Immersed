using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using Models.Domain.InviteMembers;
using Models.Enums;
using Models.Requests;
using Models.Requests.InviteMembers;
using Services;
using Services.Interfaces;
using Web.Controllers;
using Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Web.Api.Controllers
{
    [Route("api/invitemembers")]
    [ApiController]
    public class InviteMemberApiController : BaseApiController
    {
        private IInviteMemberService _inviteMemberService = null;
        private IUserService _userService = null;
        private IEmployeeService _employeeService = null;
        private IAuthenticationService<int> _authService = null;

        public InviteMemberApiController(IInviteMemberService service
            , IUserService userService
            , IEmployeeService employeeService
            , IAuthenticationService<int> authService
            , ILogger<InviteMemberApiController> logger) : base(logger)
        {
            _inviteMemberService = service;
            _userService = userService;
            _employeeService = employeeService;
            _authService = authService;
        }

        [HttpGet("status/overTime")]
        [Authorize(Roles = "SysAdmin")]
        public ActionResult<ItemsResponse<InviteMemberStatus>> GetPendingOverTime()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<InviteMemberStatus> list = _inviteMemberService.GetPendingUsersOverTime();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found");
                }
                else
                {
                    response = new ItemsResponse<InviteMemberStatus> { Items = list };
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