using Amazon.Runtime.Internal.Util;
using MessagePack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Domain.DemoAccounts;
using Models.Domain.Emails;
using Models.Enums;
using Models.Requests.DemoAccounts;
using Services;
using Services.Interfaces;
using Web.Controllers;
using Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Web.Api.Controllers
{
    [Route("api/demoaccounts")]
    [ApiController]
    public class DemoAccountApiController : BaseApiController
    {
        private IDemoAccountService _service = null;
        private IAuthenticationService<int> _authService = null;
        private IUserService _userService = null;

        public DemoAccountApiController
            (
                IDemoAccountService service,
                IAuthenticationService<int> authService,
                IUserService userService,
                ILogger<DemoAccountApiController> logger
            ) : base(logger)
        {
            _service = service;
            _authService = authService;
            _userService = userService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(DemoAccountAddRequest model)
        {
            int iCode = 201;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);
                int demoRoleId = (int)Roles.OrgAdmin;
                int demoOrgId = 101;//101 is orgId of 'Demo'
                _userService.AddUserOrgAndRole(userId, demoRoleId, demoOrgId);

                if (id == 0)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found");
                }
                else
                {
                    response = new ItemResponse<int> { Item = id };
                }
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("Cannot insert duplicate key"))
                {
                    iCode = 400;
                    response = new ErrorResponse("Account exists");
                }
                else
                {
                    iCode = 500;
                    response = new ErrorResponse(ex.Message);
                }
            }
            return StatusCode(iCode, response);
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public ActionResult<ItemsResponse<DemoAccount>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<DemoAccount> list = _service.GetAll();
                
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<DemoAccount> { Items = list };
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

        [HttpGet("{id:int}")]
        [Authorize(Roles = "SysAdmin")]
        public ActionResult<ItemResponse<DemoAccount>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                DemoAccount demo = _service.GetById(id);
                if (demo == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found");
                }
                else
                {
                    response = new ItemResponse<DemoAccount> { Item = demo };
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

        [HttpGet("active")]
        [Authorize(Roles = "SysAdmin")]
        public ActionResult<ItemsResponse<DemoAccountData>> GetActive()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<DemoAccountData> list = _service.GetActiveDemoAccounts();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<DemoAccountData> { Items = list };
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
