using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;

namespace SalesCrm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadDataController : ControllerBase
    {
        private readonly IDALLeadData _leadService;

        public LeadDataController(IDALLeadData leadService)
        {
            _leadService = leadService;
        }

        [HttpPost("GetLeads")]
        public IActionResult GetLeads(RequestModel request)
        {
            var data = _leadService.GetLeadData(request);
            return Ok(data);
        }

        [HttpGet("GetLeadsById")]
        public IActionResult GetLeadById(int leadId)
        {
            var data = _leadService.GetLeadDataById(leadId);
            return Ok(data);
        }

        [HttpPost("AddLead")]
        public IActionResult AddLead(LeadDataModel model)
        {
            var response = _leadService.AddLeadData(model);
            return Ok(response);
        }

        [HttpPost("UpdateLead")]
        public IActionResult UpdateLead(LeadDataModel model)
        {
            var response = _leadService.UpdateLeadData(model);
            return Ok(response);
        }

        [HttpDelete("Delete")]
        public IActionResult DeleteLead(int leadId, int userId)
        {
            var response = _leadService.DeleteLeadData(leadId, userId);
            return Ok(response);
        }
    }
}