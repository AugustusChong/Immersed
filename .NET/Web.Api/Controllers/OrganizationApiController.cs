using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.Domain;
using Models.Domain.Organizations;
using Models.Enums;
using Models.Requests.Organizations;
using Services;
using Services.Interfaces;
using Web.Controllers;
using Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Web.Api.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationApiController : BaseApiController
    {
        private IOrganizationService _service = null;
        private IAuthenticationService<int> _autService = null;

        public OrganizationApiController(IOrganizationService service,
            ILogger<OrganizationApiController> logger,
            IAuthenticationService<int> autService) : base(logger)
        {
            _service = service;
            _autService = autService;
        }

        [HttpGet("totalUsers")]
        [Authorize(Roles = "SysAdmin, OrgAdmin")]
        public ActionResult<ItemsResponse<OrgUserData>> GetTotalUsers()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<OrgUserData> list = _service.GetTotalUsers();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<OrgUserData> { Items = list };
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

        [HttpGet("totalTrainees")]
        [Authorize(Roles = "SysAdmin, OrgAdmin")]
        public ActionResult<ItemsResponse<OrgUserData>> GetTotalTrainees()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<OrgUserData> list = _service.GetTotalTrainees();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<OrgUserData> { Items = list };
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