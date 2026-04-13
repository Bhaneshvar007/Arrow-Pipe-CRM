using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;

namespace SalesCrm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientConfigurationController : ControllerBase
    {
        private readonly IDALClientConfigurationForm _clientFormService;

        public ClientConfigurationController(IDALClientConfigurationForm clientFormService)
        {
            _clientFormService = clientFormService;
        }



        [HttpPost("GetClientForm")]
        public IActionResult GetClientForm(RequestModel model)
        {
            var data = _clientFormService.GetClientForm(model);
            return Ok(data);
        }

        [HttpPost("SaveClientForm")]
        public IActionResult SaveClientForm([FromBody] ClientConfigurationModel model)
        {
            var response = _clientFormService.AddClientForm(model);
            return Ok(response);
        }

        [HttpPost("UpdateClientForm")]
        public IActionResult UpdateClientForm([FromBody]  ClientConfigurationModel model)
        {
            var response = _clientFormService.UpdateClientForm(model);
            return Ok(response);
        }
    }
}
