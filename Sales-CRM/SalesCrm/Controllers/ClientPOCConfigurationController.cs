using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;

namespace SalesCrm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientPOCConfigurationController : ControllerBase
    {
        private readonly IDALClientPOCConfigurationForm _service;

        public ClientPOCConfigurationController(IDALClientPOCConfigurationForm service)
        {
            _service = service;
        }

        [HttpPost("GetClientPOCForm")]
        public IActionResult GetClientPOCForm(RequestModel model)
        {
            var data = _service.GetClientPOCForm(model);
            return Ok(data);
        }

        [HttpPost("SaveClientPOCForm")]
        public IActionResult SaveClientPOCForm(ClientPOCConfigurationModel model)
        {
            var response = _service.AddClientPOCForm(model);
            return Ok(response);
        }

        [HttpPost("UpdateClientPOCForm")]
        public IActionResult UpdateClientPOCForm(ClientPOCConfigurationModel model)
        {
            var response = _service.UpdateClientPOCForm(model);
            return Ok(response);
        }
    }
}