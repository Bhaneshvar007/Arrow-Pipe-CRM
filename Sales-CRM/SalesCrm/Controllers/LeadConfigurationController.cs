using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;

namespace SalesCrm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadConfigurationController : ControllerBase
    {
        private readonly IDALLeadConfiguration _leadService;

        public LeadConfigurationController(IDALLeadConfiguration leadService)
        {
            _leadService = leadService;
        }

        [HttpPost("GetLeadForm")]
        public IActionResult GetLeadForm(RequestModel requestModel)
        {
            var data = _leadService.GetLeadForm(requestModel);
            return Ok(data);
        }

        [HttpPost("AddLeadForm")]
        public IActionResult AddLeadForm(LeadConfigurationModel model)
        {
            var response = _leadService.AddLeadForm(model);
            return Ok(response);
        }

        [HttpPost("UpdateLeadForm")]
        public IActionResult UpdateLeadForm(LeadConfigurationModel model)
        {
            var response = _leadService.UpdateLeadForm(model);
            return Ok(response);
        }
    }
}
