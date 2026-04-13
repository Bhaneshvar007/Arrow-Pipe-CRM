using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;

namespace SalesCrm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailConfigurationsController : ControllerBase
    {
    
        private readonly IDALEmailConfigurations _emailService;

        public EmailConfigurationsController(IDALEmailConfigurations emailService)
        {
            _emailService = emailService;
        }

        [Authorize]
        [HttpGet("GetEmailConfigurations")]
        public IActionResult GetEmailConfigurations(int PageNumber, int PageSize)
        {
            RequestModel req = new RequestModel
            {
                CompanyID = UserContext.GetCompanyId(User),
                CompanyUserID = UserContext.GetUserId(User),
                PageNumber = PageNumber,
                PageSize = PageSize
            };

            var data = _emailService.GetEmailConfigurations(req);

            return Ok(data);
        }
    }
}
