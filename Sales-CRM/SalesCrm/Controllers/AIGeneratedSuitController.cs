using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;

namespace SalesCrm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIGeneratedSuitController : ControllerBase
    {
        private readonly IDALAIGeneratedSuit _service;

        public AIGeneratedSuitController(IDALAIGeneratedSuit service)
        {
            _service = service;
        }

        [HttpPost("GetAIGeneratedSuitEnquiry")]
        public IActionResult GetAIGeneratedSuitEnquiry([FromBody] AIGeneratedSuitRequest req)
        {
            var result = _service.GetAIGeneratedSuitEnquirys(req);
            return Ok(result);
        }
    }
}
