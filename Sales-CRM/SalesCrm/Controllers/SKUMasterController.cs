using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;

namespace SalesCrm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SKUMasterController : ControllerBase
    {
        private readonly IDAlSKUMaster _service;

        public SKUMasterController(IDAlSKUMaster service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("GetSkuMaster")]
        public IActionResult GetSkuMaster([FromBody] SkuFilterModel skuFilterModel)
        {
            var data = _service.GetSkuMaster(skuFilterModel);

            return Ok(data);
        }

        [HttpPost("GetSkuSuggestion")]
        public IActionResult GetSkuSuggestion([FromBody] SkuSuggestionFilterModel req)
        {
            try
            {
                var result = _service.GetSkuSuggestion(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("GetSKUDropdownData")]
        public IActionResult GetSKUDropdownData(string type)
        {
            try
            {
                if (string.IsNullOrEmpty(type))
                    return BadRequest("Type is required");

                var result = _service.GetSkuDropDownMaster(type);

                if (result == null || !result.Any())
                    return NotFound("Data not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
