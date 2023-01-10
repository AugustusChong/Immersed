using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Services;
using Web.Controllers;
using Web.Models.Responses;
using Models.Requests;
using System;
using Models;
using Models.Domain;
using Models.Domain.SecurityEvents;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Models.Enums;

namespace Web.Api.Controllers
{
    [Route("api/securityevents")]
    [ApiController]
    public class SecurityEventApiController : BaseApiController
    {

        private ISecurityEventService _securityEventsService = null;
        private IAuthenticationService<int> _authService = null;
        public SecurityEventApiController(
            ISecurityEventService service,
            ILogger<PingApiController> logger,
            IAuthenticationService<int> authService) : base(logger)
        {
            _securityEventsService = service;
            _authService = authService;
        }

        [HttpGet("organizations/stats/{id:int}")]
        [Authorize(Roles = "SysAdmin")]
        public ActionResult<ItemsResponse<SecurityEventOrgStats>> GetOrganizationStats(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<List<SecurityEventOrgStats>> list = _securityEventsService.GetOrganizationStats(id);
                
                if(list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<List<SecurityEventOrgStats>> { Items = list };
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
