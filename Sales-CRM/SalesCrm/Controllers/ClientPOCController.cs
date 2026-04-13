using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Models;
using SalesCrm.Services.IDAL;
using System.ComponentModel.Design;

namespace SalesCrm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientPOCController : ControllerBase
    {
        private readonly IDALClientPOC _clientPOCService;

        public ClientPOCController(IDALClientPOC clientPOCService)
        {
            _clientPOCService = clientPOCService;
        }

        [HttpGet("GetCustomerPOCById")]
        public IActionResult GetCustomerPOCById(int customer_poc_id)
        {
            var data = _clientPOCService.GetClientPOCById(customer_poc_id);
            return Ok(data);
        }

        [HttpGet("GetCustomerPOC")]
        public IActionResult GetCustomerPOC(int PageNumber, int PageSize)
        {

            int companyId = UserContext.GetCompanyId(User);
            int userId = UserContext.GetUserId(User);

            RequestModel reqmodel = new RequestModel
            {
                CompanyID = companyId,
                CompanyUserID = userId,
                PageNumber = PageNumber,
                PageSize = PageSize
            };

            var data = _clientPOCService.GetClientPOC(reqmodel);
            if (data == null)
            {
                return NotFound(new
                {
                    Status = false,
                    Message = "Data not found",
                });

            }
            return Ok(data);
        }

        [HttpPost("AddCustomerPOC")]
        public IActionResult AddCustomerPOC(CustomerPOCDataModel model)
        {
            model.CompanyID = UserContext.GetCompanyId(User);
            model.CompanyUserID = UserContext.GetUserId(User);

            var response = _clientPOCService.AddClientPOC(model);
            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("UpdateCustomerPOC")]
        public IActionResult UpdateCustomerPOC(CustomerPOCDataModel model)
        {
            model.CompanyID = UserContext.GetCompanyId(User);
            model.CompanyUserID = UserContext.GetUserId(User);
            model.LastUpdatedBy = UserContext.GetUserId(User);

            var response = _clientPOCService.UpdateClientPOC(model);
            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("DeleteCustomerPOC")]
        public IActionResult DeleteCustomerPOC(int customer_poc_id)
        {
            int userId = UserContext.GetUserId(User);

            var response = _clientPOCService.DeleteClientPOC(customer_poc_id, userId);
            if (response.Type == "error")
                return StatusCode(500, response);

            if (response.Type == "warning")
                return BadRequest(response);
            return Ok(response);
        }
    }
}